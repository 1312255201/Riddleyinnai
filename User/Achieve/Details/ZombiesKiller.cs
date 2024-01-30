using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class ZombiesKiller
    {
        private static int id = 7;
        private static Dictionary<string, int> SCP0492KILLS = new Dictionary<string, int>();
        public static void Reset()
        {
            SCP0492KILLS.Clear();
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp0492)
                {
                    if (SCP0492KILLS.ContainsKey(ev.Attacker.UserId))
                    {
                        SCP0492KILLS[ev.Attacker.UserId]++;
                    }
                    else
                    {
                        SCP0492KILLS.Add(ev.Attacker.UserId, 1);
                    }
                    if (SCP0492KILLS[ev.Attacker.UserId] >= 5)
                    {
                        SCP0492KILLS[ev.Attacker.UserId] = -99999999;
                        var ach = Main.achieves.Find(x => x.id == id);
                        ach.status = true;
                        Main.UpdateAchievement(ev.Player.UserId, ach);
                    }
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }
    }
}
