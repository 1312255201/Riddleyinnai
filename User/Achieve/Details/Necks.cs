using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Necks
    {
        private static int id = 6;
        private static Dictionary<string, int> SCP173KILLS = new Dictionary<string, int>();
        public static void Reset()
        {
            SCP173KILLS.Clear();
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scp173)
                {
                    if (SCP173KILLS.ContainsKey(ev.Player.UserId))
                    {
                        SCP173KILLS[ev.Player.UserId]++;
                    }
                    else
                    {
                        SCP173KILLS.Add(ev.Player.UserId, 1);
                    }
                    if (SCP173KILLS[ev.Player.UserId] >= 5)
                    {
                        SCP173KILLS[ev.Player.UserId] = -99999999;
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
