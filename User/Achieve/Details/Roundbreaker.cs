using Exiled.Events.EventArgs.Player;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Roundbreaker
    {
        private static int SCPKILL = 0;
        public static void Reset()
        {
            SCPKILL = 0;
        }
        public static void OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if(ev.Player.IsScp && ev.TargetOldRole != PlayerRoles.RoleTypeId.Scp0492)
                {
                    SCPKILL++;
                }
                if(SCPKILL == 1)
                {
                    Main.UpdateAchievement(ev.Attacker.UserId, new Database.Model.Achieve() { id = 2, name = "破局者", description = "本回合第一个击杀SCP", status = true, count = 0, hide = false, progress = 0 });
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }
        public static void Unregister() {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }
    }
}
