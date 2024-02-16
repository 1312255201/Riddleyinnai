using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class SCP2818
{
    public static List<ushort> scp2818id = new List<ushort>();
    public static List<ushort> scp2818aid = new List<ushort>();
    private static CoroutineHandle coroutineHandle;
    private static IEnumerator<float> CheckTiming()
    {
        yield return Timing.WaitForSeconds(5f);
        while(Round.IsStarted)
        {
            yield return Timing.WaitForSeconds(15f);
            foreach(Pickup pickup in Pickup.List)
            {
                if(scp2818id.Contains(pickup.Serial))
                {
                    if(Room.Get(pickup.Position).Zone == ZoneType.Surface)
                    {
                        pickup.Scale = Vector3.one * 3;
                    }
                }
            }
        }
    }
    private static void OnPlayerPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (ev.Pickup.Serial != 0)
        {
            if (scp2818id.Contains(ev.Pickup.Serial))
            {
                if (SpRoleManage.RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP2490))
                {
                    ev.IsAllowed = false;
                }
                Cassie.MessageTranslated("SCP 2 8 1 8 has been picked up","SCP-2818已被捡起，拾取者"+ev.Player.Nickname);
                YYYApi.MyApi.SetNickName("SCP-2818持有者","cyan",ev.Player);
                PlayerMain.Send(ev.Player, $"<color=#FFFFCC>你是:拾取了</color><color=#0066FF>[SCP-2818]</color>\n<color=#FFFFCC>1.这是个拥有无限子弹的</color><color=#FF3333>手枪</color>，当你攻击其他人时会对他人造成高额伤害，但同时你也会死亡\n<color=#FFFFCC>2.当有人因2818</color><color=#FF3333>献祭死亡时</color>，你可以捡起scp2818-A来获取更高伤害", 10, Pos.顶部两行,200);
            }
            if (scp2818aid.Contains(ev.Pickup.Serial))
            {
                PlayerMain.Send(ev.Player, $"<color=#01fdfd>[你捡起了物品SCP-2818-A]</color>和SCP-2818一同使用，可以强化SCP2818的伤害，并且在原地产生爆炸", 5, Pos.顶部两行,200);
            }
        }

    }

    private static void OnPlayerDropingItem(DroppingItemEventArgs ev)
    {
        if (scp2818id.Contains(ev.Item.Serial))
        {
            YYYApi.MyApi.SetNickName("","",ev.Player);
        }
    }
    private static void OnRoundStarted()
    {
        coroutineHandle = Timing.RunCoroutine(CheckTiming());
        Vector3 spawnpos =Vector3.one;
        switch (new System.Random().Next(1,4))
        {
            case 1:
                spawnpos = SpawnLocationType.Inside173Armory.GetPosition()+Vector3.up;
                break;
            case 2:
                spawnpos = Room.Get(RoomType.LczToilets).Position + Vector3.up;
                break;
            case 3:
                spawnpos = SpawnLocationType.InsideGr18.GetPosition();
                break;
        }

        if (Player.List.Count() >= 28)
        {
            var gun = Pickup.CreateAndSpawn(ItemType.GunCOM15,spawnpos,Quaternion.identity);
            Log.Debug(spawnpos);
            scp2818id.Add(gun.Serial);
            if (gun is FirearmPickup firearmPickup)
            {
                firearmPickup.Ammo = byte.MaxValue - 1;
            }
        }

    }
    private static void OnPlayerUsingItem(UsingItemEventArgs ev)
    {
        if(scp2818aid.Contains(ev.Item.Serial))
        {
            ev.IsAllowed = false;
        }
    }
    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        scp2818id.Clear();
        scp2818aid.Clear();
        if(coroutineHandle.IsRunning)
        {
            Timing.KillCoroutines(coroutineHandle);
        }
    }

    private static void OnPlayerDying(DyingEventArgs ev)
    {
        if (ev.Player.Items.Any(x => scp2818id.Contains(x.Serial)))
        {
            ev.Player.RankName = "";
        }
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.UsingItem += OnPlayerUsingItem;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem += OnPlayerDropingItem;
        Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.UsingItem -= OnPlayerUsingItem;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.DroppingItem -= OnPlayerDropingItem;
        Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
    }
}