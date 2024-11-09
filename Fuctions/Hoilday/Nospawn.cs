using CommandSystem;
using Exiled.Events.EventArgs.Server;
using System;

namespace Riddleyinnai.Fuctions.Hoilday
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RespawnCommand : ParentCommand
    {
        public override string Command => "norespawn";

        public override string[] Aliases => new string[] { };

        public override string Description => "阻止后续刷新";

        public override void LoadGeneratedCommands()
        {
            
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Nospawn.Spawn = !Nospawn.Spawn;
            response = $"成功修改，是否可刷新状态:{Nospawn.Spawn}";
            return true;
        }
    }
    internal class Nospawn
    {
        public static bool Spawn = true;
        public static void Spawning(RespawningTeamEventArgs ev)
        {
            if(!Spawn)
            {
                ev.IsAllowed = false;
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += Spawning;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= Spawning;
        }
    }
}
