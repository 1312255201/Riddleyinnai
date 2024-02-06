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
            MyApi.SetNickName("九尾狐医疗兵", "cyan", player);
            Ui.PlayerMain.Send(player, "你是<color=#0F0>[九尾狐医疗兵]</color>手持医疗包10s将会将其转化为高级医疗包，高级医疗包可以使用5次。", 5, Pos.正中偏下,
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
                player.AddItem(ItemType.Medkit);
                player.AddItem(ItemType.Medkit);
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
                                Ui.PlayerMain.Send(player, "你是<color=#0F0>[九尾狐医疗兵]</color>你已经将这个血包转化为了高级血包。", 5, Pos.正中偏下,
                                    5);
                                break;
                            }
                        }
                    }
                    else
                    {
                        time = 0;
                    }
                }

            }
        }
        private static void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.九尾狐医疗兵))
            {
                if (ev.Item.Type == ItemType.Medkit)
                {
                    Timing.RunCoroutine(CheckTiming(ev.Player, ev.Item));
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
                    ev.IsAllowed = false;
                    ev.Player.UseItem(ItemType.Medkit);
                    Ui.PlayerMain.Send(ev.Player, "<color=#0F0>[高级血包]</color>使用成功。", 5, Pos.正中偏下,
                        5);
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
                Ui.PlayerMain.Send(ev.Player, "你捡起了<color=#0F0>[高级血包]</color>一共可以使用五次当前已使用"+ HealthItem[ev.Pickup.Serial] +"次", 5, Pos.正中偏下,
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