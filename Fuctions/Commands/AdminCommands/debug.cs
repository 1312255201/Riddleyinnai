using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using RemoteAdmin;
using Riddleyinnai.Fuctions.SpRoleManage;

namespace Riddleyinnai.Fuctions.Commands.AdminCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class debugcommand : ICommand
{
    private Player player;

    public string Command { get; } = "debug";
    public string[] Aliases { get; } = { "debug" };
    public string Description { get; } = "管理员指令 用于debug插件";
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
        if(arguments.Count == 1)
        {
            switch (arguments.At(0))
            {
                case "1":
                    bool scp = Player.List.Any(x => RoleManger.GetSide(x.Id) == Side.Scp && x.IsAlive);
                    bool ntf = Player.List.Any(x => x.Role.Team == Team.FoundationForces);
                    bool chaos = Player.List.Any(x => x.Role.Team == Team.ChaosInsurgency  && x.Role.Type != RoleTypeId.ClassD && !RoleManger.IsRole(x.Id,RoleManger.RoleName.SCP493)&& !RoleManger.IsRole(x.Id,RoleManger.RoleName.SCP2490));
                    bool dd = Player.List.Any(x => x.Role.Type == RoleTypeId.ClassD);
                    Log.Info("回合结束判定Debug 是否存在SCP：" + scp + "是否存在九尾：" + ntf + "是否存在混沌：" + chaos + "是否存在DD：" + dd);
                    player.RemoteAdminMessage("回合结束判定Debug 是否存在SCP：" + scp + "是否存在九尾：" + ntf + "是否存在混沌：" + chaos + "是否存在DD：" + dd);
                    break;
            }
        }

        response = "执行完成";
        return true;

    }
}