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
using Riddleyinnai.Ui;
using Scp914;
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
            Ui.PlayerMain.Send(ev.Player,"你捡起了<color=#0F0>[SCP127]</color>他会自动补充子弹，可以放到914加工变成其他武器",7,Pos.正中偏下,5);
            Timing.RunCoroutine(CheckTiming2(ev.Player,ev.Pickup.Serial));
        }
    }
    private static void OnPlayerChangingItem(ChangingItemEventArgs ev)
    {
        if (ev.Item != null)
        {
            if (items.ContainsKey(ev.Item.Serial))
            {
                Ui.PlayerMain.Send(ev.Player,"你手中的物品为<color=#0F0>[SCP127]</color>他会自动补充子弹，可以放到914加工变成其他武器",7,Pos.正中偏下,5);
            }
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

    private static List<ItemType> _itemTypes = new List<ItemType>()
    {
        ItemType.GunFSP9,
        ItemType.GunCOM18,
        ItemType.GunRevolver,
        ItemType.GunCrossvec,
        ItemType.GunE11SR,
        ItemType.GunFRMG0,
        ItemType.GunAK,
        ItemType.GunShotgun,
        ItemType.GunLogicer,
        ItemType.ParticleDisruptor,
        ItemType.GunCom45
    };
    
    private static void OnScp914UpgrandeItem(UpgradingInventoryItemEventArgs ev)
    {
        if (items.ContainsKey(ev.Item.Serial))
        {
            if (ev.KnobSetting == Scp914KnobSetting.Fine || ev.KnobSetting == Scp914KnobSetting.VeryFine)
            {
                ev.IsAllowed = false;
                if (ev.Item.Type != ItemType.GunCom45)
                {
                    int tmp = 0;
                    for (int i = 0; i < _itemTypes.Count; i++)
                    {
                        if (_itemTypes[i] == ev.Item.Type)
                        {
                            tmp = i;
                        }
                    }

                    if (ev.Item.Type == ItemType.GunLogicer)
                    {
                        if (new System.Random().Next(1, 100) <= 60)
                        {
                            return;
                        }
                    }
                    if (ev.Item.Type == ItemType.ParticleDisruptor)
                    {
                        if (new System.Random().Next(1, 100) <= 90)
                        {
                            return;
                        }
                    }
                    items.Remove(ev.Item.Serial);
                    ev.Player.RemoveItem(ev.Item);
                    items.Add(ev.Player.AddItem(_itemTypes[tmp+1]).Serial,30);
                }
            }
            else if (ev.KnobSetting == Scp914KnobSetting.OneToOne)
            {
                ev.IsAllowed = false;
            }
            else
            {      
                ev.IsAllowed = false;
                if (ev.Item.Type != ItemType.GunFSP9)
                {
                    int tmp = 0;
                    for (int i = 0; i < _itemTypes.Count; i++)
                    {
                        if (_itemTypes[i] == ev.Item.Type)
                        {
                            tmp = i;
                        }
                    }
                    items.Remove(ev.Item.Serial);
                    ev.Player.RemoveItem(ev.Item);
                    items.Add(ev.Player.AddItem(_itemTypes[tmp-1]).Serial,30);
                }
            
            }
        }

    }
    private static void OnScp914UpgrandePickup(UpgradingPickupEventArgs ev)
    {
        if (items.ContainsKey(ev.Pickup.Serial))
        {
            if (ev.KnobSetting == Scp914KnobSetting.Fine || ev.KnobSetting == Scp914KnobSetting.VeryFine)
            {
                ev.IsAllowed = false;
                if (ev.Pickup.Type != ItemType.GunCom45)
                {
                    int tmp = 0;
                    for (int i = 0; i < _itemTypes.Count; i++)
                    {
                        if (_itemTypes[i] == ev.Pickup.Type)
                        {
                            tmp = i;
                        }
                    }

                    if (ev.Pickup.Type == ItemType.GunLogicer)
                    {
                        if (new System.Random().Next(1, 100) <= 60)
                        {
                            ev.Pickup.Position = ev.OutputPosition + Vector3.up;
                            return;
                        }
                    }
                    if (ev.Pickup.Type == ItemType.ParticleDisruptor)
                    {
                        ev.Pickup.Position = ev.OutputPosition + Vector3.up;
                        if (new System.Random().Next(1, 100) <= 90)
                        {
                            return;
                        }
                    }
                    items.Remove(ev.Pickup.Serial);
                    ev.Pickup.Destroy();
                    items.Add(Pickup.CreateAndSpawn(_itemTypes[tmp +1],ev.OutputPosition+ Vector3.up,Quaternion.identity ).Serial,30);
                }
            }
            else if (ev.KnobSetting == Scp914KnobSetting.OneToOne)
            {
                ev.IsAllowed = false;
                ev.Pickup.Position = ev.OutputPosition + Vector3.up;
            }
            else
            {      
                ev.IsAllowed = false;
                if (ev.Pickup.Type != ItemType.GunFSP9)
                {
                    int tmp = 0;
                    for (int i = 0; i < _itemTypes.Count; i++)
                    {
                        if (_itemTypes[i] == ev.Pickup.Type)
                        {
                            tmp = i;
                        }
                    }
                    items.Remove(ev.Pickup.Serial);
                    ev.Pickup.Destroy();
                    items.Add(Pickup.CreateAndSpawn(_itemTypes[tmp - 1],ev.OutputPosition+ Vector3.up,Quaternion.identity ).Serial,30);
                }
            
            }
        }

    }
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingItem;
        Exiled.Events.Handlers.Player.ChangingItem += OnPlayerChangingItem;
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
        Exiled.Events.Handlers.Player.ChangingItem -= OnPlayerChangingItem;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= OnPlayerReloading;
        Exiled.Events.Handlers.Player.Shooting -= OnPlayerShooting;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnScp914UpgrandeItem;
        Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnScp914UpgrandePickup;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayer;
    }
}