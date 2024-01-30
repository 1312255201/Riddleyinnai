using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Genshin
    {
        public static void Flashed(Exiled.Events.EventArgs.Player.ReceivingEffectEventArgs ev)
        {
            if(ev.Effect is CustomPlayerEffects.Flashed Effect)
            {
                if(ev.Duration >= 3)
                {
                    var ach = Main.achieves.Find(x => x.id == 15);
                    ach.status = true;
                    Main.UpdateAchievement(ev.Player.UserId, ach);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.ReceivingEffect += Flashed;
        }
        public static void Unregister() {
            Exiled.Events.Handlers.Player.ReceivingEffect -= Flashed;
        }
    }
}
