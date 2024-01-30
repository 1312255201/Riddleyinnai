using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
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
                Cassie.MessageTranslated("SCP 2 8 1 8 has been picked up","SCP-2818已被捡起，拾取者"+ev.Player.Nickname);
                PlayerMain.Send(ev.Player, $"<color=#01fdfd>[你捡起了物品SCP-2818]</color>击敌方单位时，击中人类则造成3500点基础伤害，击中scp则造成1000点基础伤害", 5, Pos.顶部两行,200);
            }
            if (scp2818aid.Contains(ev.Pickup.Serial))
            {
                PlayerMain.Send(ev.Player, $"<color=#01fdfd>[你捡起了物品SCP-2818-A]</color>和SCP-2818一同使用，可以强化SCP2818的伤害，并且在原地产生爆炸", 5, Pos.顶部两行,200);
            }
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
        var gun = Pickup.CreateAndSpawn(ItemType.GunCOM15,spawnpos,Quaternion.identity);
        Log.Debug(spawnpos);
        scp2818id.Add(gun.Serial);
        if (gun is FirearmPickup firearmPickup)
        {
            firearmPickup.Ammo = byte.MaxValue - 1;
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
    public static void Register()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.UsingItem += OnPlayerUsingItem;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.UsingItem -= OnPlayerUsingItem;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
    }
}