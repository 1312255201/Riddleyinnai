using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using System.Linq;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Fallinggod
    {
        private static Dictionary<string,float> Falldamages = new Dictionary<string,float>();
        public static void Onhurting(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Falldown)
            {
                if(Falldamages.ContainsKey(ev.Player.UserId))
                {
                    Falldamages[ev.Player.UserId] += ev.Amount;
                }
                else
                {
                    Falldamages.Add(ev.Player.UserId, ev.Amount);
                }
            }
        }
        public static void Reset()
        {
            Falldamages.Clear();
        }
        public static void Roundending(RoundEndedEventArgs ev)
        {
            var target = Falldamages.OrderByDescending(x => x.Value).FirstOrDefault();
            var ach = Main.achieves.Find(x => x.id == 17);
            ach.status = true;
            Main.UpdateAchievement(target.Key, ach);
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Hurting += Onhurting;
            Exiled.Events.Handlers.Server.RoundEnded += Roundending;
        }
        public static void Unregister() {

            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Hurting -= Onhurting;
            Exiled.Events.Handlers.Server.RoundEnded -= Roundending;
        }
    }
}
