using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Airlock
    {
        private static int id = 11;
        private static Dictionary<string, int> airlocks = new Dictionary<string, int>();
        public static void Reset()
        {
            airlocks.Clear();
        }
        public static void OnJumped(Exiled.Events.EventArgs.Player.InteractingDoorEventArgs ev)
        {
            if(ev.Door.Type == Exiled.API.Enums.DoorType.Airlock)
            {
                if (airlocks.ContainsKey(ev.Player.UserId))
                {
                    airlocks[ev.Player.UserId]++;
                }
                else
                {
                    airlocks.Add(ev.Player.UserId, 1);
                }
                if (airlocks[ev.Player.UserId] >= 100)
                {
                    airlocks[ev.Player.UserId] = -99999999;
                    var ach = Main.achieves.Find(x => x.id == id);
                    ach.status = true;
                    Main.UpdateAchievement(ev.Player.UserId, ach);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.InteractingDoor += OnJumped;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnJumped;
        }
    }
}
