using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using System.Linq;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Killbest
    {
        private static Dictionary<string,int> Kills = new Dictionary<string,int>();
        public static void Reset()
        {
            Kills.Clear();
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (Kills.ContainsKey(ev.Attacker.UserId))
                {
                    Kills[ev.Attacker.UserId]++;
                }
                else
                {
                    Kills.Add(ev.Attacker.UserId,1);
                }
            }
        }
        public static void Roundending(RoundEndedEventArgs ev)
        {
            var target = Kills.OrderByDescending(x => x.Value).FirstOrDefault();
            Main.UpdateAchievement(target.Key, new Database.Model.Achieve() { id = 3, name = "战神", description = "本回合击杀数最多", status = true, count = 0, hide = false, progress = 0 });

        }

        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Server.RoundEnded += Roundending;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Server.RoundEnded -= Roundending;
        }
    }
}
