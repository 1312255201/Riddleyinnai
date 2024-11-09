using System;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using Riddleyinnai.Fuctions.Items;

namespace Riddleyinnai.Fuctions.Commands.AdminCommands;

   [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class scpitemcommand : ICommand
    {
        private Player player;

        public string Command { get; } = "scpitem";
        public string[] Aliases { get; } = { "scpitem" };
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
                player.RemoteAdminMessage("使用教程\nscpitem [物品] \n物品列表scp127");
                response = "参数不足";
                return false;
            }
            else
            {
                Player awaplayer = player;
                switch(arguments.At(0))
                {
                    case "scp127":
                        Scp127.GiveItem(awaplayer);
                        break;
                }
            }
            response = "执行完成";
            return true;

        }
    }