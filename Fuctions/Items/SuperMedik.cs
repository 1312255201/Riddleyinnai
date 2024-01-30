using System.Collections.Generic;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items;

public class SuperMedik
{
    public static List<ushort> supermeddikid = new List<ushort>();
    public static List<int> suppermeddikuseid = new List<int>();
    public static void OnUpgradingPickup(UpgradingPickupEventArgs ev)
    {
        if (ev.KnobSetting == Scp914.Scp914KnobSetting.VeryFine)
        {
            if (ev.Pickup.Type == ItemType.Medkit)
            {
                if (new System.Random().Next(1, 100) >= 80)
                {
                    ev.IsAllowed = false;
                    ev.Pickup.Destroy();
                    var tmp = Pickup.CreateAndSpawn(ItemType.Medkit, ev.OutputPosition, Quaternion.identity);
                    supermeddikid.Add(tmp.Serial);
                }
            }
        }
    }

    private static void OnPlayerPickingUpItem(PickingUpItemEventArgs ev)
    {
        if (supermeddikid.Contains(ev.Pickup.Serial))
        {
            PlayerMain.Send(ev.Player, $"<color=#01fdfd>[你捡起了物品超精血包]</color>使用后，你受到所有伤害将变成1，持续5分钟，效果结束后将变成1HP", 5, Pos.顶部两行,200);
        }
    }

    private static void OnPlayershooting(ShootingEventArgs ev)
    {
        if (suppermeddikuseid.Contains(ev.Player.Id))
        {
            ev.IsAllowed = false;
        }
    }
    private static void OnPlayerUsingItem(UsingItemEventArgs ev)
    {
        if (supermeddikid.Contains(ev.Item.Serial))
        {
            ev.IsAllowed = false;
            supermeddikid.Remove(ev.Item.Serial);
            ev.Player.UseItem(ItemType.Medkit);
            ev.Player.RemoveItem(ev.Item);
            suppermeddikuseid.Add(ev.Player.Id);
            Timing.CallDelayed(300f, () => {
                if (suppermeddikuseid.Contains(ev.Player.Id))
                {
                    suppermeddikuseid.Remove(ev.Player.Id);
                    ev.Player.Health = 1;
                }
            });
        }
    }
    private static void OnPlayerChangingRole(ChangingRoleEventArgs ev)
    {
        if (suppermeddikuseid.Contains(ev.Player.Id))
        {
            suppermeddikuseid.Remove(ev.Player.Id);
        }
    }
    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        supermeddikid.Clear();
        suppermeddikuseid.Clear();
    }

    private static void OnPlayerDied(DiedEventArgs ev)
    {
        if (suppermeddikuseid.Contains(ev.Player.Id))
        {
            suppermeddikuseid.Remove(ev.Player.Id);
        }
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Scp914.UpgradingPickup += OnUpgradingPickup;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.UsingItem += OnPlayerUsingItem;
        Exiled.Events.Handlers.Player.Shooting += OnPlayershooting;
        Exiled.Events.Handlers.Player.ChangingRole += OnPlayerChangingRole;
        Exiled.Events.Handlers.Player.Died += OnPlayerDied;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnUpgradingPickup;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPlayerPickingUpItem;
        Exiled.Events.Handlers.Player.UsingItem -= OnPlayerUsingItem;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.ChangingRole -= OnPlayerChangingRole;
        Exiled.Events.Handlers.Player.Shooting -= OnPlayershooting;
        Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
    }
}