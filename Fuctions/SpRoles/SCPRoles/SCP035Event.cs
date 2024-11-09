using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;

namespace Riddleyinnai.Fuctions.SpRoles.SCPRoles;

public class SCP035Event
{
    public static bool scp035runaway;
    public static CoroutineHandle checktiming;
    public static void SpawnAScp035(Player player)
    {
        player.Role.Set(RoleTypeId.Tutorial);
        RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP035,"",Side.Scp,false);
        Ui.PlayerMain.Send(player, "你是<color=#0F0>[SCP-035]</color>[SCP阵营]有67%的减伤\n无法激怒SCP-096\n正在为你伪装身份", 5,
            Pos.正中偏下, 5);
        var pos = RoleTypeId.ClassD.GetRandomSpawnLocation().Position;
        var roletmp = RoleTypeId.ClassD;
        switch (new System.Random().Next(1,6))
        {
            case 1:
            case 2:
                pos = RoleTypeId.ClassD.GetRandomSpawnLocation().Position;
                roletmp = RoleTypeId.ClassD;
                break;
            case 3:
            case 4:
                pos = RoleTypeId.Scientist.GetRandomSpawnLocation().Position;
                roletmp = RoleTypeId.Scientist;
                break;
            case 5:
                pos = RoleTypeId.FacilityGuard.GetRandomSpawnLocation().Position;
                roletmp = RoleTypeId.FacilityGuard;
                player.AddItem(ItemType.KeycardGuard);
                player.AddItem(ItemType.GunFSP9);
                player.AddItem(ItemType.Medkit);
                player.AddItem(ItemType.GrenadeFlash);
                player.AddItem(ItemType.Radio);
                player.AddItem(ItemType.ArmorLight);
                player.SetAmmo(AmmoType.Nato9, 120);
                break;
        }
        Timing.CallDelayed(3f, () =>
        {
            player.ChangeAppearance(roletmp);
            player.Position = pos;
        });
        checktiming = Timing.RunCoroutine(CheckTiming(player,roletmp));
        Scp173Role.TurnedPlayers.Add(player);
        Scp096Role.TurnedPlayers.Add(player);
    }

    private static void OnDropingItem(DroppingItemEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP035))
        {
            if (ev.Item.IsKeycard)
            {
                if (ev.IsThrown)
                {
                    ev.IsAllowed = false;
                }
            }
        }
    }
    private static void OnAddScp096Target(AddingTargetEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP035))
        {
            ev.IsAllowed = false;
        }
    }
    
    public static IEnumerator<float> CheckTiming(Player player,RoleTypeId roleTypeId)
    {
        yield return Timing.WaitForSeconds(5f);
        while (RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP035) && player.IsAlive)
        {
            player.ChangeAppearance(roleTypeId);
            yield return Timing.WaitForSeconds(300);
        }
    }
    public static void OnPlayerOpenDoor(InteractingDoorEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP035))
        {
            if (ev.Door.Type is DoorType.Scp914Gate)
            {
                ev.IsAllowed = false;
                ev.Door.Lock(0.1f,DoorLockType.AdminCommand);
                if (ev.Door.IsOpen)
                {
                    ev.Door.IsOpen = false;
                }
            }
        }
    }

    private static void OnUsingItem(UsingItemEventArgs ev)
    {

    }

    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        scp035runaway = false;
    }
    private static void OnPlayerDied(DiedEventArgs ev)
    {
        if (ev.Attacker != null)
        {
            if (RoleManger.IsRole(ev.Attacker.Id, RoleManger.RoleName.SCP035))
            {
                Ui.PlayerMain.Send(ev.Player, "你被<color=#F00>[SCP-035]</color>击杀", 5, Pos.正中偏下, 5);
            }
        }
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Scp096.AddingTarget += OnAddScp096Target;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem+= OnDropingItem;
        Exiled.Events.Handlers.Player.Died+= OnPlayerDied;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddScp096Target;
        Exiled.Events.Handlers.Player.DroppingItem-= OnDropingItem;
        Exiled.Events.Handlers.Player.Died-= OnPlayerDied;
    }
}