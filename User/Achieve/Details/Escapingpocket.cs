using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Escapingpocket
    {
        public static Dictionary<string, int > Pockets = new Dictionary<string, int>();
        public static void Reset()
        {
            Pockets.Clear();
        }
        public static void Escaping(EscapingPocketDimensionEventArgs ev)
        {
            if (Pockets.ContainsKey(ev.Player.UserId))
            {
                Pockets[ev.Player.UserId] += 1;
            }
            else
            {
                Pockets.Add(ev.Player.UserId, 1);
            }
            if (Pockets[ev.Player.UserId] >= 5)
            {
                Pockets[ev.Player.UserId] = -99999;
                var ach = Main.achieves.Find(x => x.id == 15);
                ach.status = true;
                Main.UpdateAchievement(ev.Player.UserId, ach);
            }

        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += Escaping;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= Escaping;
        }
    }
}
