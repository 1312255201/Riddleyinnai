using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
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
    }
    private static void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP999))
        {
            ev.IsAllowed = false;
        }
    }
    
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
    }
}