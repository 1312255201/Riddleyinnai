using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using System;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    internal class Roledefault
    {
        public static void OnSpawned(SpawnedEventArgs ev)
        {
            switch(ev.Player.Role.Type)
            {
                case PlayerRoles.RoleTypeId.NtfPrivate:
                    ev.Player.RemoveItem(x=>x.Type == ItemType.GunCrossvec);
                    ev.Player.AddItem(ItemType.GunE11SR);
                    break;
                case PlayerRoles.RoleTypeId.NtfSpecialist:
                case RoleTypeId.NtfSergeant:
                    ev.Player.AddItem(ItemType.SCP1853);
                    ev.Player.AddItem(ItemType.GrenadeFlash);
                    break;
                case PlayerRoles.RoleTypeId.ChaosRifleman:
                case RoleTypeId.ChaosConscript:
                    ev.Player.RemoveItem(x => x.Type == ItemType.ArmorCombat);
                    ev.Player.AddItem(ItemType.ArmorHeavy);
                    break;
                case RoleTypeId.ChaosRepressor:
                    ev.Player.AddItem(ItemType.SCP1853);
                    ev.Player.AddItem(ItemType.GrenadeFlash);
                    break;
                case RoleTypeId.Scientist:
                case RoleTypeId.ClassD:
                    ev.Player.EnableEffect<CustomPlayerEffects.MovementBoost>();
                    ev.Player.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>(5);
                    break;

            }
            if(ev.Player.IsCHI || ev.Player.IsNTF)
            {
                ev.Player.MaxHealth = 110;
                ev.Player.Health = 110;
                ev.Player.EnableEffect<CustomPlayerEffects.MovementBoost>();
                ev.Player.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>(8);
            }
            if(ev.Player.IsHuman)
            {
                if(ev.Player.Role != RoleTypeId.Tutorial)
                {
                    Array values = Enum.GetValues(typeof(CandyKindID));
                    var random = new System.Random();
                    int randomIndex = random.Next(values.Length);
                    var type = (CandyKindID)values.GetValue(randomIndex);
                    ev.Player.TryAddCandy(type);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        }
    }
}
