using System;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using Riddleyinnai.Fuctions.SpRoles.ChaosRoles;
using Riddleyinnai.Fuctions.SpRoles.CllassD;
using Riddleyinnai.Fuctions.SpRoles.MTFRoles;
using Riddleyinnai.Fuctions.SpRoles.Scientists;
using Riddleyinnai.Fuctions.SpRoles.SCPRoles;
using Riddleyinnai.Fuctions.SpRoles.Tutroles;

namespace Riddleyinnai.Fuctions.Commands.AdminCommands;

   [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class spawnscpcommand : ICommand
    {
        private Player player;

        public string Command { get; } = "spawnscp";
        public string[] Aliases { get; } = { "spawnscp" };
        public string Description { get; } = "管理员指令 用于调试刷特殊角色";
        public bool SanitizeResponse { get; }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.Get(sender).RemoteAdminAccess)
            {
                Player.Get(sender).Ban(100,"[YYY-Anti-Cheat]请不要尝试越权执行指令");
                response = "请不要尝试越权执行命令";
                return false;
            }
            if (sender is PlayerCommandSender plr)
            {
                player = Player.Get(plr.PlayerId);
            }
            if(arguments.Count < 1)
            {
                player.RemoteAdminMessage("使用教程\nspawnscp [角色] [id] id可不填默认为自己\n角色列表 zhiyuan scp682 scp035 ntfsniper ntfhealther scp550 scp2490 scp1143 d9341 scp069 scp181 scp493 scpcn08");
                response = "参数不足";
                return false;
            }
            else
            {
                Player awaplayer = player;
                if(arguments.Count == 2)
                {
                    awaplayer = Player.Get(arguments.At(1));
                }
                switch(arguments.At(0))
                {
                    case"zhiyuan":
                        NTFHelper.SpawnAHelper(awaplayer);
                        break;
                    case "scp682":
                        Scp682Event.SpawnAScp682(awaplayer);
                        break;
                    case "scp999":
                        SCP999Event.SpawnAScp999(awaplayer);
                        break;
                    case "scp550":
                        SCP550Event.SpawnAScp550(awaplayer);
                        break;
                    case "scp2490":
                        SCP2490Event.SpawnAScp2490(awaplayer);
                        break;
                    case "scp1143":
                        SCP1143.SpawnAScp1143(awaplayer);
                        break;
                    case "d9341":
                        D9341Events.SpanwAD9341(awaplayer);
                        break;
                    case"scp069":
                        SCP069Event.SpawnAScp069(awaplayer);
                        break;
                    case "scp181":
                        SCP181Event.SpawnAScp181(awaplayer);
                        break;
                    case "scp493":
                        SCP493Event.SpawnASCP493(awaplayer);
                        break;
                    case "scpcn08":
                        scpcn08.SpawnACn08(awaplayer);
                        break;
                    case "ntfhealther":
                        NTFHealth.SpawnAHealther(awaplayer);
                        break;
                    case "ntfsniper":
                        NTFsniper.SpawnASniper(awaplayer);
                        break;
                    case "scp035":
                        SCP035Event.SpawnAScp035(awaplayer);
                        break;
                }
            }
            response = "执行完成";
            return true;

        }
    }