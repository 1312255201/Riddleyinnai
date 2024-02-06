using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs.Player;

namespace Riddleyinnai.Misc
{
    internal class Infammo
    {
        public static void Register()
        {
            Exiled.Events.Handlers.Player.ReloadingWeapon += Onreload;
            Exiled.Events.Handlers.Player.DroppingAmmo += OnPlayerDropingAmmo;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.ReloadingWeapon -= Onreload;
            Exiled.Events.Handlers.Player.DroppingAmmo -= OnPlayerDropingAmmo;
        }

        private static void OnPlayerDropingAmmo(DroppingAmmoEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        public static void Onreload(Exiled.Events.EventArgs.Player.ReloadingWeaponEventArgs ev)
        {
            if(ev.Firearm.Type != ItemType.ParticleDisruptor)
            {
                var maxammo = ev.Firearm.MaxAmmo;
                var ammotype = ev.Firearm.AmmoType;

                if (ev.Player.GetAmmo(ammotype) < maxammo)
                {
                    ev.Player.SetAmmo(ammotype, maxammo);
                }
            }
        }
    }
}
