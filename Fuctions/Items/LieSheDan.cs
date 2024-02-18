using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.BasicMessages;
using MEC;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class LieSheDan
{
        public static List<ushort> items = new List<ushort>();
        public static int lastuse = 4;
    public static void GiveItem(Player player)
    {
        items.Add(player.AddItem(ItemType.GunRevolver).Serial);
    }

    private static void OnPlayerShoot(ShotEventArgs ev)
    {
        if (items.Contains(ev.Firearm.Serial))
        {
            if (ev.Firearm.Ammo >= 4)
            {
                ev.Firearm.Ammo -= 4;
                lastuse = 4;
            }
        }
    }

    private static void OnPlayerChangingItem(ChangingItemEventArgs ev)
    {
        if (ev.Item != null)
        {
            if (items.Contains(ev.Item.Serial))
            {
                Ui.PlayerMain.Send(ev.Player,"你手中的物品为<color=#0F0>[M500]</color>每次攻击射出4发子弹，攻击两次需要重新装弹",7,Pos.正中偏下,5);
            }
        }
    }
    private static void OnPlayerPickingItem(PickingUpItemEventArgs ev)
    {
        if (items.Contains(ev.Pickup.Serial))
        {
            Ui.PlayerMain.Send(ev.Player,"你捡起了<color=#0F0>[M500]</color>每次攻击射出4发子弹，攻击两次需要重新装弹",7,Pos.正中偏下,5);
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
                    if(pickup.Scale != Vector3.one * 1.5f)
                    {
                        pickup.Scale = Vector3.one * 1.5f;
                    }
                }
            }
        }
    }

    private static void OnPlayerReloadWeapen(ReloadingWeaponEventArgs ev)
    {
        if (items.Contains(ev.Firearm.Serial))
        {
            if (ev.Firearm.Ammo < 8)
            {
                byte tmp = ev.Firearm.Ammo;
                ev.IsAllowed = false;
                ev.Player.Connection.Send<RequestMessage>(new RequestMessage(ev.Firearm.Serial, RequestType.Reload));
                Timing.CallDelayed(2f, () =>
                {
                    if (ev.Player.CurrentItem != null && items.Contains(ev.Player.CurrentItem.Serial))
                    {
                        if (ev.Firearm.Ammo >= tmp)
                        {
                            ev.Firearm.Ammo = 8;
                        }
                    }
                });
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
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingItem;
        Exiled.Events.Handlers.Player.Shot += OnPlayerShoot;
        Exiled.Events.Handlers.Player.ReloadingWeapon += OnPlayerReloadWeapen;
        Exiled.Events.Handlers.Player.ChangingItem += OnPlayerChangingItem;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingItem;
        Exiled.Events.Handlers.Player.Shot -= OnPlayerShoot;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= OnPlayerReloadWeapen;
        Exiled.Events.Handlers.Player.ChangingItem -= OnPlayerChangingItem;
    }
}