using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Peaceful
    {
        private static int id = 10;
        private static Dictionary<string, int> weapons = new Dictionary<string, int>();
        public static void Reset()
        {
            weapons.Clear();
        }
        public static void OnJumped(Exiled.Events.EventArgs.Player.PickingUpItemEventArgs ev)
        {
            if(ev.Pickup.Type.IsWeapon(true) && (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scientist || ev.Player.Role.Type == PlayerRoles.RoleTypeId.ClassD))
            {
                if (weapons.ContainsKey(ev.Player.UserId))
                {
                    weapons[ev.Player.UserId]++;
                }
                else
                {
                    weapons.Add(ev.Player.UserId, 1);
                }
            }
        }
        public static void Onescaping(EscapingEventArgs ev)
        {
            if(ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scientist || ev.Player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
            {
                if(!weapons.ContainsKey(ev.Player.UserId))
                {
                    var ach = Main.achieves.Find(x => x.id == id);
                    ach.status = true;
                    Main.UpdateAchievement(ev.Player.UserId, ach);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.PickingUpItem += OnJumped;
            Exiled.Events.Handlers.Player.Escaping += Onescaping;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnJumped;
            Exiled.Events.Handlers.Player.Escaping -= Onescaping;
        }
    }
}
