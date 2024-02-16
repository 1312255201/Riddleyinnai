using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using MEC;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class Scp127
{
    public static Dictionary<ushort, int> items = new();
    
    public static void GiveItem(Player player)
    {
            items.Add(player.AddItem(ItemType.GunFSP9).Serial,30);
    }
    public static void SpawnItem(Vector3 pos)
    {
            items.Add(Pickup.CreateAndSpawn(ItemType.GunFSP9,pos,Quaternion.identity).Serial,30);
    }
    private static IEnumerator<float> CheckTiming()
    {
        yield return Timing.WaitForSeconds(5f);
        while(Round.IsStarted)
        {
            yield return Timing.WaitForSeconds(15f);
            foreach(Pickup pickup in Pickup.List)
            {
                if(items.ContainsKey(pickup.Serial))
                {
                    pickup.Scale = Vector3.one * 1.5f;
                }
            }
        }
    }

    private static void OnRoundStart()
    {
        Timing.RunCoroutine(CheckTiming());
    }

    private static void OnWaitingForPlayer()
    {
        SpawnItem(Room.Get(ZoneType.Entrance).GetRandomValue().Position + Vector3.up);
    }
    private static void OnPlayerReloading(ReloadingWeaponEventArgs ev)
    {
        if (items.ContainsKey(ev.Firearm.Serial))
        {
            ev.IsAllowed = false;
        }
    }

    private static IEnumerator<float> CheckTiming2(Player player, ushort item)
    {
        yield return Timing.WaitForSeconds(1);
        while (player.Items.Any(x=>x.Serial == item))
        {
            var item2 = player.Items.First(x => x.Serial == item);
            if (item2 is Firearm firearm)
            {
                if (firearm.Ammo < byte.MaxValue - 1)
                {
                    firearm.Ammo++;
                }

                items[firearm.Serial]++;
            }
            yield return Timing.WaitForSeconds(1);
        }
    }
    private static void OnPlayerPickingItem(PickingUpItemEventArgs ev)
    {
        if (items.ContainsKey(ev.Pickup.Serial))
        {
            Timing.RunCoroutine(CheckTiming2(ev.Player,ev.Pickup.Serial));
        }
    }

    private static void OnPlayerShooting(ShootingEventArgs ev)
    {
        if (items.ContainsKey(ev.Firearm.Serial))
        {
            items[ev.Firearm.Serial]--;
            if (items[ev.Firearm.Serial] >= byte.MaxValue - 1)
            {
                ev.Firearm.Ammo = byte.MaxValue - 1;
            }
        }
    }
    private static void OnScp914UpgrandeItem(UpgradingInventoryItemEventArgs ev)
    {
        
    }
    private static void OnScp914UpgrandePickup(UpgradingPickupEventArgs ev)
    {

    }
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingItem;
        Exiled.Events.Handlers.Player.Shooting += OnPlayerShooting;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.ReloadingWeapon += OnPlayerReloading;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnScp914UpgrandeItem;
        Exiled.Events.Handlers.Scp914.UpgradingPickup += OnScp914UpgrandePickup;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayer;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingItem;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= OnPlayerReloading;
        Exiled.Events.Handlers.Player.Shooting -= OnPlayerShooting;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnScp914UpgrandeItem;
        Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnScp914UpgrandePickup;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayer;
    }
}