using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Riddleyinnai.Fuctions.Items;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.CllassD;

public class D9341Events
{
    private static Vector3 D9341Pos;
    private static RoleTypeId D9341role;
    private static float D9341Hp;
    private static bool D9341cd;
    private static int savetime;
    private static bool havescp2818;
    private static List<ItemType> D9341Items = new List<ItemType>();
    public static void SpanwAD9341(Player player)
    {
        SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.D9341,"",Side.ChaosInsurgency,false);
        player.AddItem(ItemType.Coin);
        player.AddItem(ItemType.Coin);
        savetime = 0;
        player.AddItem(ItemType.Flashlight);
        D9341role = player.Role.Type;
        D9341Hp = player.Health;
        D9341Pos = player.Position;
        D9341cd = true;
        Timing.CallDelayed(300f, () =>
        {
            D9341cd = false;
        });
        foreach (var item in player.Items)
        {
            D9341Items.Add(item.Type);
        }
        player.RankName = "D-9341";
        player.RankColor = "yellow";
        Ui.PlayerMain.Send(player, $"<b><size=25>你是 D-9341</b></size>存档：右键丢弃一枚硬币保存一次角色在此时此刻的状态，位置，背包道具等。\n读档：右键手电筒时回到最近一次存档的状态。手电筒开局CD五分钟\n死亡读档：若角色已存档，在死亡时回到最近一次存档的状态并失去角色特性。",10,Ui.Pos.正中偏下,5);
    }

    private static void OnDropingItem(DroppingItemEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.D9341))
        {
            if (ev.Item.Type == ItemType.Coin)
            {
                ev.IsAllowed = false;
                if(ev.Player.CurrentRoom.Type != RoomType.Pocket)
                {
                    ev.Player.RemoveItem(ev.Item);
                    savetime++;
                    if (savetime <= 2)
                    {
                        D9341role = ev.Player.Role.Type;
                        D9341Hp = ev.Player.Health;
                        D9341Pos = ev.Player.Position;
                        D9341Items.Clear();
                        havescp2818 = false;
                        foreach (var item in ev.Player.Items)
                        {
                            D9341Items.Add(item.Type);
                            if(SCP2818.scp2818id.Contains(item.Serial))
                            {
                                havescp2818 = true;
                            }
                        }
                        Ui.PlayerMain.Send(ev.Player, $"<b><size=25>存档成功</b></size>",3,Ui.Pos.正中偏下,5);
                    }
                    else
                    {
                        Ui.PlayerMain.Send(ev.Player, $"<b><size=25>你没有存档次数了</b></size>",3,Ui.Pos.正中偏下,5);
                    }
                }
            }

            if (ev.Item.Type == ItemType.Flashlight)
            {
                ev.IsAllowed = false;
                if (!D9341cd)
                {
                    D9341cd = true;
                    ev.Player.RemoveItem(ev.Item);
                    if (ev.Player.Role.Type != D9341role)
                    {
                        ev.Player.Role.Set(D9341role,SpawnReason.Respawn,RoleSpawnFlags.None);
                    }
                    ev.Player.ClearInventory();
                    bool flag = false;
                    foreach (var id in D9341Items)
                    {
                        var tmp = ev.Player.AddItem(id);
                        if(havescp2818)
                        {
                            if(!flag)
                            {
                                if (id == ItemType.GunCOM15)
                                {
                                    if(tmp is Firearm firearm)
                                    {
                                        firearm.Ammo = byte.MaxValue - 1;
                                    }
                                    SCP2818.scp2818id.Add(tmp.Serial);
                                    Cassie.MessageTranslated("SCP 2 8 1 8 has been picked up", "SCP-2818已被捡起，拾取者" + ev.Player.Nickname);
                                    flag = true;
                                }
                            }

                        }
                    }
                    ev.Player.Health = D9341Hp;
                    ev.Player.Position = D9341Pos +Vector3.up;
                    Timing.CallDelayed(300f, () =>
                    {
                        D9341cd = false;
                    });
                }
            }
        }
    }

    public static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        Reset();
        havescp2818 = false;
    }

    public static void Reset()
    {
        D9341Pos = Vector3.one;
        D9341Hp = 0;
        D9341cd = true;
        savetime = 0;
        D9341Items.Clear();
    }

    private static void OnPlayerDying(DyingEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.D9341))
        {
            ev.IsAllowed = false;
            if (ev.Player.Role.Type != D9341role)
            {
                ev.Player.Role.Set(D9341role, SpawnReason.Respawn, RoleSpawnFlags.None);
            }
            ev.Player.ClearInventory();
            bool flag = false;
            foreach (var id in D9341Items)
            {
                var tmp = ev.Player.AddItem(id);
                if (havescp2818)
                {
                    if (!flag)
                    {
                        if (id == ItemType.GunCOM15)
                        {
                            SCP2818.scp2818id.Add(tmp.Serial);
                            flag = true;
                        }
                    }

                }
            }
            ev.Player.Health = D9341Hp;
            ev.Player.DisableAllEffects();
            Timing.CallDelayed(0.02f, () =>
            {
                ev.Player.Position = D9341Pos + Vector3.up;
                Reset();
            });
            ev.Player.RankName = "";
        }
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem += OnDropingItem;
        Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem -= OnDropingItem;
        Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
    }
}