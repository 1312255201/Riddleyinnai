using CommandSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions.Hoilday
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Killrankclear : ParentCommand
    {
        public override string Command => "killrankclear";

        public override string[] Aliases => new string[] { };

        public override string Description => "清理击杀榜";

        public override void LoadGeneratedCommands()
        {
            throw new NotImplementedException();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Killrank.Reset();
            response = "Ok!";
            return true;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class KillRankCommand : ParentCommand
    {
        public override string Command => "killrank";

        public override string[] Aliases => new string[] { };

        public override string Description => "查看击杀榜";

        public override void LoadGeneratedCommands()
        {
            
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ranks = Killrank.Kills.OrderByDescending(x => x.Value);
            response = "";
            if(ranks.Any())
            {
                var count = 1;
                foreach(var rank in ranks)
                {
                    if(Player.TryGet(rank.Key,out var player))
                    {
                        response += $"[{count++}] 玩家[{player.Nickname}] 杀死了 [{rank.Value}]名玩家";
                    }
                }
            }
            return true;
        }
    }

    internal class Killrank
    {
        public static void Reset()
        {
            Kills.Clear();
        }
        public static Dictionary<string, int> Kills = new Dictionary<string, int>();
        public static void Ondied(DiedEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if(Kills.ContainsKey(ev.Attacker.UserId))
                {
                    Kills[ev.Attacker.UserId]++;
                }
                else
                {
                    Kills.Add(ev.Attacker.UserId, 1);
                }
            }
        }

        public static void Register()
        {
            Exiled.Events.Handlers.Player.Died += Ondied;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Died -= Ondied;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
        }
    }
}
