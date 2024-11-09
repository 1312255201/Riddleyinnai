using Exiled.Events.EventArgs.Player;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Crazyer
    {
        public static void OnDied(DiedEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(ev.Attacker.Health == 1)
                {
                    if(ev.Player.IsScp && ev.Player.Role.Type != PlayerRoles.RoleTypeId.Scp0492)
                    {
                        var ach = Main.achieves.Find(x => x.id == 14);
                        ach.status = true;
                        Main.UpdateAchievement(ev.Attacker.UserId, ach);
                    }
                }
            }
        }
        public static void Reset()
        {
            
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
