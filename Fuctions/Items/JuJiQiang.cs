using System.Collections.Generic;
using CommandSystem.Commands.RemoteAdmin.Inventory;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.BasicMessages;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class JuJiQiang
{
    public static List<ushort> items = new List<ushort>();

    public static void GiveItem(Player player)
    {
        if (items.Count < 3)
        {
            items.Add(player.AddItem(ItemType.GunE11SR).Serial);
        }
    }
    private static void OnChangingItem(ChangingItemEventArgs ev)
    {
        if (ev.Item != null)
        {
            if(items.Contains(ev.Item.Serial))
            {
                if (ev.Item.Type == ItemType.GunE11SR)
                {
                    if(ev.Item is Firearm firearm) {
                        if(firearm.Ammo >=1)
                        {
                            firearm.Ammo = 1;
                        }
                    }
                }
            }
        }
    }

    private static void OnPlayerPickingItem(PickingUpItemEventArgs ev)
    {
        if (items.Contains(ev.Pickup.Serial))
        {
            Ui.PlayerMain.Send(ev.Player,"你捡起了<color=#0F0>[狙击步枪]</color>每次只可以发射一发子弹，但是有巨额伤害",5,Pos.正中偏下,5);
        }
    }
    public static void OnPlayerReloading(ReloadingWeaponEventArgs ev)
    {
        if(items.Contains(ev.Item.Serial))
        {
            if (ev.Firearm.Type == ItemType.GunE11SR)
            {
                if (ev.Firearm.Ammo >= 1)
                {
                    ev.IsAllowed = false;
                    ev.Firearm.Ammo = 1;
                }
                else
                {
                    ev.IsAllowed = false;
                    if (ev.Player.GetAmmo(Exiled.API.Enums.AmmoType.Nato556) >= 5)
                    {
                        ev.Player.SetAmmo(Exiled.API.Enums.AmmoType.Nato556, (ushort)(ev.Player.GetAmmo(Exiled.API.Enums.AmmoType.Nato556) - ushort.Parse("5")));
                        ev.Firearm.Ammo = 1;
                        ev.Player.Connection.Send<RequestMessage>(new RequestMessage(ev.Firearm.Serial, RequestType.Reload));
                    }
                }
            }
        }
    }
    private static IEnumerator<float> CheckTiming()
    {
        yield return Timing.WaitForSeconds(5f);
        while(Round.IsStarted)
        {
            yield return Timing.WaitForSeconds(15f);
            foreach(Pickup pickup in Pickup.List)
            {
                if(items.Contains(pickup.Serial))
                {
                    if(pickup.Scale != Vector3.one * 3)
                    {
                        pickup.Scale = Vector3.one * 3;
                    }
                }
            }
        }
    }
    private static void OnRoundStart()
    {
        Timing.RunCoroutine(CheckTiming());
    }

    private static void OnRoundRestart()
    {
     items.Clear();   
    }
    public static void Reg()
    {
        Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
        Exiled.Events.Handlers.Player.ReloadingWeapon += OnPlayerReloading;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingItem;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= OnPlayerReloading;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingItem;
    }
}