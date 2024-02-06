using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using MEC;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class LieSheDan
{
        public static List<ushort> items = new List<ushort>();
        public static int lastuse = 5;
    public static void GiveItem(Player player)
    {
        items.Add(player.AddItem(ItemType.GunE11SR).Serial);
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
            else
            {
                lastuse = ev.Firearm.Ammo + 1;
                ev.Firearm.Ammo = 0;
            }
        }
    }
    private static void OnPlayerPickingItem(PickingUpItemEventArgs ev)
    {
        if (items.Contains(ev.Pickup.Serial))
        {
            Ui.PlayerMain.Send(ev.Player,"你捡起了<color=#0F0>[猎蛇弹]</color>开火射出多枚子弹造成大额伤害",5,Pos.正中偏下,5);
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
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingItem;
        Exiled.Events.Handlers.Player.Shot -= OnPlayerShoot;
    }
}