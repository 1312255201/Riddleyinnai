using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using Riddleyinnai.YYYApi;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.MTFRoles;

   public class NTFHealth
    {
        public static Dictionary<ushort, int> HealthItem = new Dictionary<ushort, int>();
        public static void SpawnAHealther(Player player)
        {
            if (player.Role.Type != RoleTypeId.NtfPrivate)
            {
                player.Role.Set(RoleTypeId.NtfPrivate);
            }
            MyApi.SetNickName("九尾狐医疗兵", "", player);
            Ui.PlayerMain.Send(player, ".你是[九尾狐医疗兵]你可以手持[普通医疗包]制作[高级医疗包]\n2.制作时间为30s，逃生点内10s\n3.你出生携带两个[高级医疗包]，可使用五次", 5, Pos.正中偏下,
                5);
            if (player.Role.Type != RoleTypeId.NtfSergeant)
            {
                player.Role.Set(PlayerRoles.RoleTypeId.NtfSergeant);
            }
            SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.九尾狐医疗兵,"",Side.Mtf,false);
            Timing.CallDelayed(0.04f, () =>
            {
                player.ClearInventory();
                player.AddItem(ItemType.GunCOM18);
                HealthItem.Add(player.AddItem(ItemType.Medkit).Serial,0);
                HealthItem.Add(player.AddItem(ItemType.Medkit).Serial,0);
                player.AddItem(ItemType.ArmorLight);
                player.AddItem(ItemType.KeycardMTFOperative);
                player.AddItem(ItemType.Radio);
            });

        }
        public static IEnumerator<float> CheckTiming(Player player, Item item)
        {
            yield return Timing.WaitForSeconds(1f);
            int time = 0;
            if(!HealthItem.ContainsKey(item.Serial))
            {
                while (player.CurrentItem.Serial == item.Serial)
                {
                    yield return Timing.WaitForSeconds(1f);
                    time++;
                    if (Vector3.Distance(player.Position, new Vector3(132, 988, 22)) <= 5)
                    {
                        player.ClearBroadcasts();
                        player.Broadcast(1, time.ToString());
                        if (time >= 10)
                        {
                            time = 0;
                            if (!HealthItem.ContainsKey(item.Serial))
                            {
                                HealthItem.Add(item.Serial, 0);
                                Ui.PlayerMain.Send(player, "你是<color=#0F0>[九尾狐医疗兵]</color>你已经将这个血包转化为了高级医疗包。", 5, Pos.正中偏下,
                                    5);
                                break;
                            }
                        }
                    }
                    else
                    {
                        player.ClearBroadcasts();
                        player.Broadcast(1, time.ToString());
                        if (time >= 30)
                        {
                            time = 0;
                            if (!HealthItem.ContainsKey(item.Serial))
                            {
                                HealthItem.Add(item.Serial, 0);
                                Ui.PlayerMain.Send(player, "你是<color=#0F0>[九尾狐医疗兵]</color>你已经将这个血包转化为了高级医疗包。", 5, Pos.正中偏下,
                                    5);
                                item.Scale = Vector3.one * 1.5f;
                                break;
                            }
                        }
                    }
                }

            }
        }
        private static void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.九尾狐医疗兵))
            {
                if(ev.Item != null)
                {
                    if (ev.Item.Type == ItemType.Medkit)
                    {
                        Timing.RunCoroutine(CheckTiming(ev.Player, ev.Item));
                    }
                }
            }

            if (ev.Item != null)
            {
                if(HealthItem.ContainsKey(ev.Item.Serial))
                {
                    Ui.PlayerMain.Send(ev.Player, "<color=#81CAFF>[高级医疗包]</color>剩余" + (5-  HealthItem[ev.Item.Serial])+ "次"  , 5, Pos.正中偏下,
                        5);
                } 
            }
        }
        private static void OnPlayerUsingItem(UsingItemEventArgs ev)
        {
            if (HealthItem.ContainsKey(ev.Item.Serial))
            {   
                HealthItem[ev.Item.Serial]++;
                if (HealthItem[ev.Item.Serial] <= 4)
                {
                    Timing.CallDelayed(0.5f, () =>
                    {
                        ev.IsAllowed = false;
                        ev.Player.UseItem(ItemType.Medkit);
                        Ui.PlayerMain.Send(ev.Player, "<color=#81CAFF>[高级医疗包]</color>剩余" + (5-  HealthItem[ev.Item.Serial])+ "次"  , 5, Pos.正中偏下,
                            5);
                    });
                }
                else
                {
                    HealthItem.Remove(ev.Item.Serial);
                }
            }
        }
        public static void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(HealthItem.ContainsKey(ev.Pickup.Serial))
            {
                Ui.PlayerMain.Send(ev.Player, "<color=#81CAFF>[高级医疗包]</color>剩余" + (5-  HealthItem[ev.Pickup.Serial])+ "次"  , 5, Pos.正中偏下,
                    5);
            }
        }
        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            HealthItem.Clear();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.UsingItem += OnPlayerUsingItem;
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
        }
        public static void UnRegister()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
            Exiled.Events.Handlers.Player.UsingItem -= OnPlayerUsingItem;
        }
    }