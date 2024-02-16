using System;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using RemoteAdmin;
using Riddleyinnai.Fuctions.SpRoleManage;

namespace Riddleyinnai.Fuctions.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class cz : ICommand
{
    public string Command { get; } = "ignorescp682";
    public string[] Aliases { get; } = { "ignorescp682","de682" };
    public string Description { get; } = "放弃SCP682身份";
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is PlayerCommandSender plr)
        {
            Player player = Player.Get(plr.PlayerId);
            if (SpRoleManage.RoleManger.IsRole(player.Id, RoleManger.RoleName.SCP682))
            {
                if (player.IsOverwatchEnabled)
                {
                    RoleManger.DelRolePlayer(player.Id,true);
                    player.IsOverwatchEnabled = false;
                    if (player.Role.Type != RoleTypeId.Spectator)
                    {
                        player.Role.Set( RoleTypeId.Spectator);
                    }

                    response = "清除682身份";
                    return true;
                }
                else
                {
                    response = "活着不可以放弃";
                    return true;
                }
            }
        }
        response = "未知错误";
        return false;

    }
}