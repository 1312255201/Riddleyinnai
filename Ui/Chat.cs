using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Ui
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatCommand3 : ICommand
    {
        public string Command => "ac";

        public string[] Aliases => new string[] { };

        public string Description => "发送管理求助信息 用法.ac [求助内容]";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pl = Player.Get(sender: sender);
            if (pl == null)
            {
                response = "[错误]您的角色未被找到，请重新登录后再尝试使用命令";
                return false;
            }
            var msg = "";
            foreach (var arg in arguments)
            {
                msg += arg + " ";
            }
            if(!Publicmethod.IsMsgOk(msg,out var reason))
            {
                response = reason;
                return false;
            }
            PlayerMain.Send(Player.List.Where(x => x.RemoteAdminAccess).ToList(), $"<color=#01fdfd>[管理员求助]</color>{pl.Nickname}:{msg}", 20, Pos.顶部两行,200);
            response = "成功发送了信息";
            foreach (var item in Player.List.Where(x => x.RemoteAdminAccess))
            {
                item.SendConsoleMessage($"[管理员求助]" + $"{pl.Nickname}:{msg}", "white");
            }
            return true;
        }
    }
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatCommand : ICommand
    {
        public string Command => "bc";

        public string[] Aliases => new string[] { };

        public string Description => "发送全体聊天信息 用法.bc [聊天内容]";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pl = Player.Get(sender:sender);
            if(pl == null)
            {
                response = "[错误]您的角色未被找到，请重新登录后再尝试使用命令";
                return false;
            }
            var msg = "";
            foreach(var arg in arguments)
            {
                msg += arg + " ";
            }
            if(!Publicmethod.IsMsgOk(msg,out var reason))
            {
                response = reason;
                return false;
            }
            PlayerMain.Send(Player.List.ToList(), "<color=#33FF99><b>[聊天]</b></color><color=#ffffff>" +pl.Nickname +":"+ msg + "</color>", 10, Pos.屏幕正中左边五行);
            response = "成功发送了信息";
            foreach (var item in Player.List)
            {
                item.SendConsoleMessage($"[全体聊天]" + $"{pl.Nickname}:{msg}", "white");
            }
            return true;
        }
    }

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatCommand2 : ICommand
    {
        public string Command => "c";

        public string[] Aliases => new string[] { };

        public string Description => "发送阵营聊天信息 用法.c [聊天内容]";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pl = Player.Get(sender: sender);
            if (pl == null)
            {
                response = "[错误]您的角色未被找到，请重新登录后再尝试使用命令";
                return false;
            }
            var msg = "";
            foreach (var arg in arguments)
            {
                msg += arg + " ";
            }
            if(!Publicmethod.IsMsgOk(msg,out var reason))
            {
                response = reason;
                return false;
            }
            var msgcolor = "";
            switch (pl.Role.Type)
            {
                case PlayerRoles.RoleTypeId.ClassD:
                    msgcolor = "#ff8000"; break;
                case PlayerRoles.RoleTypeId.FacilityGuard:
                    msgcolor = RoleTypeId.FacilityGuard.GetColor().ToHex(); break;  
                case PlayerRoles.RoleTypeId.NtfCaptain:
                    msgcolor = "#003dca";break;
                case PlayerRoles.RoleTypeId.NtfSergeant:
                    msgcolor = "#0093fa";break;
                case PlayerRoles.RoleTypeId.NtfSpecialist:
                    msgcolor = "#0096ff"; break;
                case PlayerRoles.RoleTypeId.NtfPrivate:
                    msgcolor = "#70c3ff"; break;
                case PlayerRoles.RoleTypeId.ChaosConscript:
                case PlayerRoles.RoleTypeId.ChaosMarauder:
                case PlayerRoles.RoleTypeId.ChaosRifleman:
                case PlayerRoles.RoleTypeId.ChaosRepressor:
                    msgcolor = "#008f1c"; break;
                case PlayerRoles.RoleTypeId.Scientist:
                    msgcolor = "#ffff7c"; break;
                case PlayerRoles.RoleTypeId.Scp049:
                case PlayerRoles.RoleTypeId.Scp106:
                case PlayerRoles.RoleTypeId.Scp096:
                case PlayerRoles.RoleTypeId.Scp173:
                case PlayerRoles.RoleTypeId.Scp939:
                case RoleTypeId.Scp079:
                case PlayerRoles.RoleTypeId.Scp0492:
                    msgcolor = "#ff0000"; break;
                case PlayerRoles.RoleTypeId.Tutorial:
                    msgcolor = "#ff00b0";break;
                case PlayerRoles.RoleTypeId.Overwatch:
                    msgcolor = "#01fdfd"; break;
                case PlayerRoles.RoleTypeId.Spectator:
                    msgcolor = "#A0A0A0";break;
            }
            if(pl.Role.Type != PlayerRoles.RoleTypeId.ClassD && pl.Role.Type != PlayerRoles.RoleTypeId.Scientist && pl.Role.Type != PlayerRoles.RoleTypeId.Tutorial)
            {
                PlayerMain.Send(Player.List.Where(x => x.Role.Team == pl.Role.Team || x.IsOverwatchEnabled).ToList(), $"<color={msgcolor}>{pl.Nickname}</color>:{msg}", 5, Pos.屏幕正中左边五行);
                response = "成功发送了信息";

                foreach (var item in Player.List)
                {
                    item.SendConsoleMessage($"[阵营聊天]" + $"{pl.Nickname}:{msg}", "white");
                }
                return true;
            }
            else
            {
                PlayerMain.Send(Player.List.Where(x => x.Role.Team == pl.Role.Team || x.IsOverwatchEnabled).ToList(), $"<color={msgcolor}>{pl.Nickname}</color>:{msg}", 5, Pos.屏幕正中左边五行);
                response = "成功发送了信息";

                foreach (var item in Player.List)
                {
                    item.SendConsoleMessage($"[阵营聊天]" + $"{pl.Nickname}:{msg}", "white");
                }
                return true;
            }
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Chat3Command : ParentCommand
    {
        public override string Command => "abc";

        public override string[] Aliases => new string[] { };

        public override string Description => "发送管理员广播信息 用法.abc [聊天内容]";

        public override void LoadGeneratedCommands()
        {
            //
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pl = Player.Get(sender: sender);
            if (pl == null)
            {
                response = "[错误]您的角色未被找到，请重新登录后再尝试使用命令";
                return false;
            }
            var msg = "";
            foreach (var arg in arguments)
            {
                msg += arg + " ";
            }
            if (!Publicmethod.IsMsgOk(msg, out var reason))
            {
                response = reason;
                return false;
            }
            PlayerMain.Send(Player.List.ToList(), "<color=#ff0000><b>[管理员广播]</b></color><color=#ffffff>" + msg + "</color>", 10, Pos.顶部两行,countdown:false);
            response = "成功发送了信息";
            foreach (var item in Player.List)
            {
                item.SendConsoleMessage($"[管理员广播]" + $"{pl.Nickname}:{msg}", "white");
            }
            return true;
        }
    }
}
