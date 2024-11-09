using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class SCP914god
    {
        private static int id = 9;
        private static Dictionary<string, int> SCP914 = new Dictionary<string, int>();
        public static void Reset()
        {
            SCP914.Clear();
        }
        public static void OnJumped(Exiled.Events.EventArgs.Scp914.ActivatingEventArgs ev)
        {
            if (SCP914.ContainsKey(ev.Player.UserId))
            {
                SCP914[ev.Player.UserId]++;
            }
            else
            {
                SCP914.Add(ev.Player.UserId, 1);
            }
            if (SCP914[ev.Player.UserId] >= 50)
            {
                SCP914[ev.Player.UserId] = -99999999;
                var ach = Main.achieves.Find(x => x.id == id);
                ach.status = true;
                Main.UpdateAchievement(ev.Player.UserId, ach);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Scp914.Activating += OnJumped;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Scp914.Activating -= OnJumped;
        }
    }
}
