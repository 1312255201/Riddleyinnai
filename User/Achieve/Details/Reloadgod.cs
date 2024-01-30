using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Reloadgod
    {
        private static int id = 5;
        private static Dictionary<string, int> Reloads = new Dictionary<string, int>();
        public static void Reset()
        {
            Reloads.Clear();
        }
        public static void Onreload(ReloadingWeaponEventArgs ev)
        {
            if (Reloads.ContainsKey(ev.Player.UserId))
            {
                Reloads[ev.Player.UserId]++;
            }
            else
            {
                Reloads.Add(ev.Player.UserId, 1);
            }
            if (Reloads[ev.Player.UserId] >= 100)
            {
                Reloads[ev.Player.UserId] = -99999999;
                var ach = Main.achieves.Find(x => x.id == id);
                ach.status = true;
                Main.UpdateAchievement(ev.Player.UserId, ach);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.ReloadingWeapon += Onreload ;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.ReloadingWeapon -= Onreload;
        }
    }
}
