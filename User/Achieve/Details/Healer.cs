using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Healer
    {
        private static int id = 12;
        private static Dictionary<string, int> Uses = new Dictionary<string, int>();
        public static void Reset()
        {
            Uses.Clear();
        }
        public static void Onused(UsedItemEventArgs ev)
        {
            if (ev.Player != null)
            {
                if (Uses.ContainsKey(ev.Player.UserId))
                {
                    Uses[ev.Player.UserId]++;
                }
                else
                {
                    Uses.Add(ev.Player.UserId, 1);
                }
            }
        }
        public static void Roundending(EndingRoundEventArgs ev)
        {
            if(ev.IsRoundEnded)
            {
                var target = Uses.OrderByDescending(x => x.Value).FirstOrDefault();
                var ach = Main.achieves.Find(x => x.id == id);
                ach.status = true;
                Main.UpdateAchievement(target.Key, ach);
            }
        }

        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.UsedItem += Onused;
            Exiled.Events.Handlers.Server.EndingRound += Roundending;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.UsedItem -= Onused;
            Exiled.Events.Handlers.Server.EndingRound -= Roundending;
        }
    }
}
