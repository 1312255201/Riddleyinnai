using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.Tutroles;

public class SCP999Event
{
    public static void SpawnAScp999(Player player)
    {
        player.Role.Set(RoleTypeId.Tutorial);
        player.Health = 6000;
        player.Position = RoleTypeId.FacilityGuard.GetRandomSpawnLocation().Position + Vector3.up;
        player.AddItem(ItemType.GunFSP9);
        player.AddItem(ItemType.ArmorLight);
        player.AddItem(ItemType.KeycardZoneManager);
        SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP999,"",Side.Tutorial,false);
        YYYApi.MyApi.SetNickName("SCP-999","",player);
        Timing.RunCoroutine(CheckTiming(player));
    }

    private static IEnumerator<float> CheckTiming(Player player)
    {
        yield return Timing.WaitForSeconds(1);
        while (RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP999))
        {
            var tmpplayers = Player.List.Where(x => x.IsAlive && Vector3.Distance(x.Position, player.Position) <= 5);
            if (tmpplayers.Count()> 0)
            {
                if (tmpplayers.Any(x => x.Role.Team == Team.SCPs))
                {
                    player.Heal(new System.Random().Next(1,4));
                }
                else
                {
                    player.Heal(1);
                }
            }
            yield return Timing.WaitForSeconds(1);
        }
    }

    private static IEnumerator<float> HealPlayer(Player player)
    {
        for (int i = 0; i < 30; i++)
        {
            yield return Timing.WaitForSeconds(1);
            if (player.IsAlive)
            {
                if (player.Role.Team == Team.SCPs)
                {
                    player.Heal(10);
                }
                else
                {
                    player.Heal(1);
                }
            }
            else
            {
                break;
            }
        }
    }
    private static void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP999))
        {
            ev.IsAllowed = false;
        }
    }

    private static void OnPlayerUsedItem(UsedItemEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP999))
        {
            if(ev.Item.Type == ItemType.Adrenaline)
            {
                foreach (var varp in Player.List.Where(x=>Vector3.Distance(x.Position,ev.Player.Position) <= 5 && x.IsAlive && x.Role.Team != Team.SCPs))
                {
                    varp.ReferenceHub.playerStats.GetModule<AhpStat>().ServerAddProcess(10, 100, 0, 0.7f, 0.0f, false);
                }
                foreach (var varp in Player.List.Where(x=>Vector3.Distance(x.Position,ev.Player.Position) <= 5 && x.IsAlive && x.Role.Team == Team.SCPs))
                {
                    varp.HumeShield += 90;
                }
            }

            if (ev.Item.Type == ItemType.Medkit)
            {
                foreach (var varp in Player.List.Where(x=>Vector3.Distance(x.Position,ev.Player.Position) <= 5 && x.IsAlive && x.Role.Team != Team.SCPs))
                {
                    varp.MaxHealth += 3;
                    varp.Heal(30);
                }
                foreach (var varp in Player.List.Where(x=>Vector3.Distance(x.Position,ev.Player.Position) <= 5 && x.IsAlive && x.Role.Team == Team.SCPs))
                {
                    varp.MaxHealth += 10;
                    varp.Heal(50);
                }
            }
            if (ev.Item.Type == ItemType.SCP500)
            {
                foreach (var varp in Player.List.Where(x=>Vector3.Distance(x.Position,ev.Player.Position) <= 5 && x.IsAlive ))
                {
                    HealPlayer(varp);
                }
            }
        }
    }
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
        Exiled.Events.Handlers.Player.UsedItem += OnPlayerUsedItem;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
        Exiled.Events.Handlers.Player.UsedItem -= OnPlayerUsedItem;
    }
}