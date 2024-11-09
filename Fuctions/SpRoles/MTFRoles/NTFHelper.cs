using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;

namespace Riddleyinnai.Fuctions.SpRoles.MTFRoles;

public class NTFHelper
{
    public static bool state;
    public static void SpawnAHelper(Player player)
    {
        if (!RoleManger.GetRole(RoleManger.RoleName.九尾狐支援兵).Any())
        {
            state = false;
            YYYApi.MyApi.SetNickName("九尾狐支援", "", player);
            Ui.PlayerMain.Send(player, "你是[九尾狐支援]丢弃硬币将会放置一个充电桩\n丢弃对讲机将会防止一个子弹补给站\n丢弃一个血包将会变成治疗站", 5, Pos.正中偏下, 5);
            player.Role.Set(PlayerRoles.RoleTypeId.NtfSergeant);
            Timing.RunCoroutine(ZhiYuanFuc(player));
            RoleManger.AddRole(player.Id,RoleManger.RoleName.九尾狐支援兵,"",Side.Mtf,false);
            Timing.CallDelayed(0.04f, () =>
            {
                player.ClearInventory();
                player.AddItem(ItemType.GrenadeHE);
                player.AddItem(ItemType.GrenadeFlash);
                player.SetAmmo(AmmoType.Nato9,300);
                player.SetAmmo(AmmoType.Ammo44Cal,300);
                player.SetAmmo(AmmoType.Nato762,300);
                player.SetAmmo(AmmoType.Ammo12Gauge,300);
                player.SetAmmo(AmmoType.Nato556,300);
            });
        }
    }

    private static IEnumerator<float> ZhiYuanFuc(Player player)
    {
        yield return Timing.WaitForSeconds(1);
        while (RoleManger.IsRole(player.Id,RoleManger.RoleName.九尾狐支援兵))
        {
            switch (new System.Random().Next(1, 6))
            {
                case 1:
                    player.AddAmmo(AmmoType.Nato9,30);
                    break;
                case 2:
                    player.AddAmmo(AmmoType.Ammo44Cal,30);
                    break;
                case 3:
                    player.AddAmmo(AmmoType.Nato762,30);
                    break;
                case 4:
                    player.AddAmmo(AmmoType.Ammo12Gauge,30);
                    break;
                case 5:
                    player.AddAmmo(AmmoType.Nato556,30);
                    break;
            }
            yield return Timing.WaitForSeconds(30);
        }
    }
    private static void OnPlayerPickingUpItme(PickingUpItemEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.九尾狐支援兵))
        {
            if (ev.Pickup.Type == ItemType.MicroHID)
            {
                if (ev.Pickup is MicroHIDPickup microHidPickup)
                {
                    if (microHidPickup.Energy <= 0.3)
                    {
                        if (!state)
                        {
                            state = true;
                            ev.IsAllowed = false;
                            ev.Pickup.Destroy();
                            ev.Player.AddItem(ItemType.MicroHID);
                            Ui.PlayerMain.Send(ev.Player, "以为你电磁炮充电", 5, Pos.正中偏下, 5);
                        }
                    }
                }

            }
        }
    }
    private static void OnRoundRestart()
    {
        state = false;
    }

    private static void OnTeamRespawn(RespawningTeamEventArgs ev)
    {
        if (RoleManger.GetRole(RoleManger.RoleName.九尾狐支援兵).Any())
        {
            var p = Player.Get(RoleManger.GetRole(RoleManger.RoleName.九尾狐支援兵).First().player);
            if (p.IsAlive)
            {
                p.AddAmmo(AmmoType.Nato9,100);
                p.AddAmmo(AmmoType.Ammo44Cal,100);
                p.AddAmmo(AmmoType.Nato762,100);
                p.AddAmmo(AmmoType.Ammo12Gauge,100);
                p.AddAmmo(AmmoType.Nato556,100);
                Ui.PlayerMain.Send(p, "子弹已经补充", 5, Pos.正中偏下, 5);
            }
        }
    }
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingUpItme;
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        Exiled.Events.Handlers.Server.RespawningTeam += OnTeamRespawn;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingUpItme;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnTeamRespawn;
    }
}