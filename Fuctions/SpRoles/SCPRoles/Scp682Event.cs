using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.SCPRoles;

public class Scp682Event
{
    public static bool spawned;
    public static int fuhuotime;
    public static CoroutineHandle CoroutineHandle;
    public static void SpawnAScp682(Player player)
    {
        if (player.IsOverwatchEnabled)
        {
            player.IsOverwatchEnabled = false;
        }
        spawned = true;
        fuhuotime++;
        player.Role.Set( RoleTypeId.Scp939);
        Timing.CallDelayed(1f, () =>
        {
            switch (fuhuotime)
            {
                case 1:
                    player.Scale = new Vector3(1.25f,1.1f,1.25f);
                    break;
                case 2:
                    player.Scale = new Vector3(1.25f,1.1f,1.25f);
                    break;
                case 3:
                    player.Scale = new Vector3(1.25f,1.1f,1.25f);
                    player.EnableEffect<Scp207>();
                    break;
                default:
                    player.EnableEffect<Scp207>();
                    player.Scale = new Vector3(1.25f,1.1f,1.25f);
                    break;
            }
            player.Health = 1000 + 500 * fuhuotime;
            if (Warhead.IsDetonated)
            {
                player.Position = SpawnLocationType.InsideSurfaceNuke.GetPosition() + Vector3.up;
            }
            else
            {
                player.Position = RoleTypeId.Scp049.GetRandomSpawnLocation().Position + Vector3.up;
            }
            SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP682,"",Side.Scp,false);
            YYYApi.MyApi.SetNickName("SCP-682","",player);
            Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#FF3300>[SCP-682]</color>无限复活越复活后越强但是代价是之后的每一次复活时间都会变长，之后的每次复活都会加10%抗性直到80%抗性后不再增长。\n",15,Pos.正中偏下,5);
        });
        if (CoroutineHandle.IsRunning)
        {
            Timing.KillCoroutines(CoroutineHandle);
        }
        CoroutineHandle = Timing.RunCoroutine(CheckTiming(player));
    }
    private static IEnumerator<float> CheckTiming(Player player)
    {
        int time = 0;
        yield return Timing.WaitForSeconds(5f);
        while (SpRoleManage.RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP682))
        {
            if (!player.IsAlive || player.IsOverwatchEnabled)
            {
                time++;
                Ui.PlayerMain.Send(player,"<color=#FFFFCC>SCP-682重生记时 .ignorescp682 放弃682身份</color><color=#FF3300>"+time+"</color>。\n",1,Pos.正中偏下,5);
                if (time > 120 + fuhuotime * 45)
                {
                    if (Player.List.Any(x =>
                            Vector3.Distance(x.Position, RoleTypeId.Scp049.GetRandomSpawnLocation().Position) <= 15 &&
                            RoleManger.GetSide(x.Id) != Side.Scp))
                    {
                        foreach (var varp in Player.List.Where(x=>x.Role.Team == Team.SCPs || RoleManger.IsRole(x.Id,RoleManger.RoleName.SCP682)))
                        {
                            Ui.PlayerMain.Send(varp,"<color=#FFFFCC>SCP682被压制无法复活</color><color=#FF3300></color>。\n",1,Pos.正中偏下,5);
                        }
                    }
                    else
                    {
                        time = 0;
                        SpawnAScp682(player);
                    }
                }
            }
            yield return Timing.WaitForSeconds(1);
        }
    }

    private static void OnPlayerInteringDoor(InteractingDoorEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP682))
        {
            if (fuhuotime >= 4)
            {
                ev.IsAllowed = true;
                if (ev.Door.Base is IDamageableDoor door)
                {
                    door.ServerDamage(65535f, DoorDamageType.ServerCommand);
                }
            }
        }
    }
    private static void OnPlayerDying(DyingEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP682))
        {
            ev.IsAllowed = false;
            ev.Player.IsOverwatchEnabled = true;
        }
    }
    private static void OnRoundRestart()
    {
        spawned = false;
        fuhuotime = 0;
    }

    public static void Reg()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerInteringDoor;
        Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnPlayerInteringDoor;
        Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
    }
}