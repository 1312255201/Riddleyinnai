using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Interactables.Interobjects;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.SCPRoles;

public class SCP550Event
{

    public static void SpawnAScp550(Player player)
    {
        player.Role.Set(RoleTypeId.Tutorial);
        SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP550,"",Side.Scp,false);
        YYYApi.MyApi.SetNickName("SCP-550","",player);
        Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#FF3300>[SCP-550]</color>\n1.特殊收容措施已失效\u2586\u2586-2\u25866\u2586，你的目标是与其他SCP合作侵占整个设施\n2.你只能使用钥匙卡、手枪、冲锋枪、投掷物、医疗品、SCP268\n3.你有50%的子弹抗性，攻击敌人恢复少量HP与AHP"/*\n4.你的宿敌可能后方逃生点\n*/,15,Pos.正中偏下,5);
        Timing.CallDelayed(0.3f, () =>
        {
            player.SetAmmo(AmmoType.Nato9, 135);
            player.SetAmmo(AmmoType.Nato762, 54);
            player.SetAmmo(AmmoType.Ammo44Cal, 108);
            player.AddItem(ItemType.GunCOM15);
            player.AddItem(ItemType.GunRevolver);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.ArmorLight);
            player.Position = RoleTypeId.NtfCaptain.GetRandomSpawnLocation().Position;
        });
        Timing.CallDelayed(3f, () =>
        {
            player.ChangeAppearance(RoleTypeId.ChaosRifleman);
        });
        Timing.RunCoroutine(CheckTiming(player));
        Scp173Role.TurnedPlayers.Add(player);
        Scp096Role.TurnedPlayers.Add(player);
    }
    private static IEnumerator<float> CheckTiming(Player player)
    {
        yield return Timing.WaitForSeconds(5f);
        int time = 0;
        int time2 = 0;
        while (SpRoleManage.RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP550) && player.IsAlive)
        {
            yield return Timing.WaitForSeconds(1f);
            time2++;
            if(time2 > 300)
            {
                player.ChangeAppearance(RoleTypeId.ChaosRifleman);
                time2 = 0;
            }
            BasicRagdoll[] array = UnityEngine.Object.FindObjectsOfType<BasicRagdoll>();
            foreach (BasicRagdoll ragdoll in array)
            {
                if (Vector3.Distance(ragdoll.transform.position, player.Position) <= 3)
                {
                    time++;
                    if (time >= 10)
                    {
                        time = 0;
                        if (ragdoll.Info.RoleType.IsHuman() || ragdoll.Info.RoleType == RoleTypeId.Scp0492)
                        {
                            player.Heal(10);
                        }
                        else
                        {
                            player.Heal(30);
                        }
                        NetworkServer.Destroy(ragdoll.gameObject);
                    }
                    break;
                }
            }
        }
    }
    private static void OnDropingItem(DroppingItemEventArgs ev)
    {
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP550))
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
    public static void OnPlayerOpenDoor(InteractingDoorEventArgs ev)
    {
/*        var v1 = ev.Door.Position + ev.Door.Transform.forward;
        var v2 = ev.Door.Position - ev.Door.Transform.forward;
        if(Vector3.Distance(ev.Player.Position, v1) < Vector3.Distance(ev.Player.Position, v2))
        {
            ev.Player.Position = v2+Vector3.up;
            ev.IsAllowed = false;
        }
        else
        {
            ev.Player.Position = v1 + Vector3.up;
            ev.IsAllowed = false;
        }*/
        if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP550))
        {
            if (ev.Door.IsCheckpoint)
            {
                ev.IsAllowed = true;
            }
        }
        if (SpRoleManage.RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP550))
        {
            if (!ev.IsAllowed)
            {
                if (ev.Door.IsFullyClosed)
                {
                    if (ev.Door.Base is PryableDoor pryableDoor)
                    {
                        pryableDoor.TryPryGate(ev.Player.ReferenceHub);
                    }       
                }
            }
            if (ev.Door.Type is DoorType.Scp914Gate or DoorType.CheckpointLczA or DoorType.CheckpointLczB)
            {
                ev.IsAllowed = false;
            }
            /*
             if (ev.Door.Type == DoorType.GateA || ev.Door.Type == DoorType.GateB)
            {
                if (!ev.Door.IsOpen)
                {
                    ev.Door.IsOpen = true;
                    Timing.CallDelayed(2f, () =>
                    {
                        ev.Door.IsOpen = false;
                    });
                }
            }*/
        }


    }

    private static void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP550)) return;
        if (ev.Player.Health >= 100)
        {
            if (ev.Item.Type.IsMedical() || ev.Item.Type == ItemType.SCP207)
            {
                ev.IsAllowed = false;
            }
        }
        if (ev.Item.Type.IsArmor() || ev.Item.Type is ItemType.SCP268  || 
            ev.Item.Type is ItemType.GunCOM15 or ItemType.GunCOM18 or ItemType.GunRevolver or ItemType.GunCom45 or ItemType.GunFSP9 || ev.Item.Type.IsAmmo() || ev.Item.Type.IsMedical() || ev.Item.Type.IsThrowable())
        {
            return;
        }
        ev.IsAllowed = false;
    }

    private static void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (!RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP550)) return;
        /*if (ev.Pickup.Type.IsArmor() || ev.Pickup.Type is ItemType.SCP268 || ev.Pickup.Type.IsKeycard() ||
            (ev.Pickup.Type is ItemType.GunCOM15 or ItemType.GunCOM18 or ItemType.GunRevolver or ItemType.GunCom45 or ItemType.GunCrossvec)
                 || ev.Pickup.Type.IsAmmo() || ev.Pickup.Type.IsMedical() ||
            ev.Pickup.Type.IsThrowable())
        {
            return;
        }*/
        ev.IsAllowed = false;
    }
    private static void OnPlayerShooting(ShootingEventArgs ev)
    {
        if (!RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP550)) return;
        if(ev.Player.CurrentItem != null)
        {
            if(ev.Player.CurrentItem.Type != ItemType.GunCOM15 && ev.Player.CurrentItem.Type != ItemType.GunCrossvec && ev.Player.CurrentItem.Type != ItemType.GunCOM18 && ev.Player.CurrentItem.Type != ItemType.GunRevolver && ev.Player.CurrentItem.Type != ItemType.GunCom45 && ev.Player.CurrentItem.Type != ItemType.GunFSP9)
            {
                ev.IsAllowed = false;
                Ui.PlayerMain.Send(ev.Player,"你是<color=#0F0>[SCP-550]</color>无法使用这个枪械",5,Pos.正中偏下,5);
            }
        }
    }

    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
    }

    public static void Register()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.Shooting += OnPlayerShooting;
        Exiled.Events.Handlers.Player.DroppingItem += OnDropingItem;
        Log.Debug("SCP550加载");
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.Shooting -= OnPlayerShooting;
        Exiled.Events.Handlers.Player.DroppingItem -= OnDropingItem;

    }
}