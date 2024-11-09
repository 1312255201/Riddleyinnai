using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Jumpgod
    {
        private static int id = 4;
        private static Dictionary<string,int> Jumps = new Dictionary<string,int>();
        public static void Reset()
        {
            Jumps.Clear();
        }
        public static void OnJumped(JumpingEventArgs ev)
        {
            if(Jumps.ContainsKey(ev.Player.UserId))
            {
                Jumps[ev.Player.UserId]++;
            }
            else
            {
                Jumps.Add(ev.Player.UserId, 1);
            }
            if (Jumps[ev.Player.UserId] >= 200)
            {
                Jumps[ev.Player.UserId] = -99999999;
                var ach = Main.achieves.Find(x => x.id == id);
                ach.status = true;
                Main.UpdateAchievement(ev.Player.UserId, ach);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Jumping += OnJumped;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Jumping -= OnJumped;
        }
    }
}
