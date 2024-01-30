using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    internal class SCPBoost
    {
        public static void Onhurting(HurtingEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp049)
                {
                    ev.Amount = 9999;
                }
                if(ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp106)
                {
                    /*
                    if(!ev.DamageHandler.Type.IsStatusEffect())
                    {
                        ev.Player.EnableEffect(Exiled.API.Enums.EffectType.SinkHole, 1);
                    }
                    */
                }
                if(ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp096 || ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp939)
                {
                    ev.Amount += 10;
                }
                if(ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp106)
                {
                    if(ev.DamageHandler.Type.IsWeapon(false))
                    {
                        ev.Amount *= 0.65f;
                    }
                }
            }
        }
        public static void OnSkill(Exiled.Events.EventArgs.Scp173.PlacingTantrumEventArgs ev)
        {
            if(ev.IsAllowed)
            {
                ev.Player.Heal(100f);
            }
        }
        public static void Lockdown(Exiled.Events.EventArgs.Scp079.LockingDownEventArgs ev)
        {
            if(ev.IsAllowed)
            {
                foreach(var lift in ev.Room.Doors)
                {
                    lift.Lock(5, Exiled.API.Enums.DoorLockType.Lockdown079);
                }
            }
        }
        public static void Eat(Exiled.Events.EventArgs.Scp0492.ConsumingCorpseEventArgs ev)
        {
            if(ev.ErrorCode == PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConsumeAbility.ConsumeError.None)
            {
                if(ev.Player.TryGetEffect<CustomPlayerEffects.MovementBoost>(out var statusEffect))
                {
                    if(statusEffect != null)
                    {
                        if(statusEffect.Intensity + 3 >= 30)
                        {
                            byte ins = 30;
                            ev.Player.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>((byte)(ins));
                        }
                        else
                        {
                            ev.Player.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>((byte)(statusEffect.Intensity + 3));
                        }
                    }
                }
            }
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp096)
                {
                    ev.Attacker.AddAhp(20, 500);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += Onhurting;
            Exiled.Events.Handlers.Scp173.PlacingTantrum += OnSkill;
            Exiled.Events.Handlers.Scp079.LockingDown += Lockdown;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse += Eat;
        }
        public static void Unregister() 
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= Onhurting;
            Exiled.Events.Handlers.Scp173.PlacingTantrum -= OnSkill;
            Exiled.Events.Handlers.Scp079.LockingDown -= Lockdown;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse -= Eat;
        }
    }
}
