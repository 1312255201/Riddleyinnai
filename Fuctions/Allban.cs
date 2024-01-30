using CommandSystem;
using Exiled.API.Features;
using LiteNetLib.Utils;
using MEC;
using Riddleyinnai.Database;
using Riddleyinnai.Database.Model;
using Riddleyinnai.Ui;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    /*
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AllbanCommand : ParentCommand
    {
        public override string Command => "allban";

        public override string[] Aliases => new string[] { "scpban" };

        public override string Description => "全服封禁一名玩家 用法:[1]scpban [steam64] [封禁到的日期(2077-12-31)] [理由]封禁一名玩家到指定的日期" +
            "\n[2]scpban unban [steam64] 解封一名玩家" +
            "\n[3]scpban list 查询所有封禁记录" +
            "\n[4]scpban [steam64] 查询该名玩家的封禁记录";

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2 && arguments.Count != 1 && arguments.Count != 3)
            {
                response = Description; return false;
            }
            else
            {
                if(arguments.Count == 1)
                {
                    if(arguments.At(0) == "list")
                    {
                        var res = Methods.Findban("all");
                        if(res != null)
                        {
                            if(res.Success)
                            {
                                var bans = res.Data;
                                var msg = "";
                                for (int i = 1; i <= bans.Count; i++)
                                {
                                    var model = bans[i - 1];
                                    msg += $"[{i}][{model.Userid}]被管理员[{model.Admin}]封禁至{model.Endtime} 原因:{model.Reason}\n";
                                }
                                response = msg;
                                return true;
                            }
                        }
                        response = "未能查询到相关记录";
                        return false;
                    }
                    else
                    {
                        var steamid = arguments.At(0);
                        var res = Methods.Findban(steamid);
                        if (res != null)
                        {
                            if (res.Success)
                            {
                                var bans = res.Data;
                                var msg = "";
                                for (int i = 1; i <= bans.Count; i++)
                                {
                                    var model = bans[i - 1];
                                    msg += $"[{i}][{model.Userid}]被管理员[{model.Admin}]封禁至{model.Endtime} 原因:{model.Reason}\n";
                                }
                                response = msg;
                                return true;
                            }
                        }
                        response = "未能查询到相关记录";
                        return false;
                    }
                }
                else if(arguments.Count == 2)
                {
                    if(arguments.At(0) == "unban")
                    {
                        var res = Methods.Delban(arguments.At(1));
                        if(res != null)
                        {
                            if (res.Success)
                            {
                                response = "删除此用户的封禁记录成功！";
                                return true;
                            }
                        }
                        response = "删除失败 请检查是否有此用户封禁的记录";
                        return false;
                    }
                    else
                    {
                        response = Description; return false;
                    }
                }
                else
                {
                    var date = arguments.At(1);
                    var ban = new Ban();
                    ban.Starttime = DateTime.Now.ToString("yyyy-MM-dd");
                    ban.Admin = Player.Get(sender).Nickname;
                    ban.Reason = arguments.At(2);
                    ban.Userid = arguments.At(0);
                    var result = DateTime.MinValue;
                    try
                    {
                        result = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        response = "日期输入有误 请重新输入(2077-12-31)"; return false;
                    }
                    if (result > DateTime.Now)
                    {
                        ban.Endtime = date;
                        var res = Methods.Addban(ban);
                        if(res != null)
                        {
                            if(res.Success)
                            {
                                response = "写入新的封禁记录成功!";
                                if (Player.TryGet(arguments.At(0), out var pl))
                                {
                                    pl.Kick($"你已被服务器封禁！解封时间:{ban.Endtime}\n封禁理由:{ban.Reason}");
                                }
                                return true;
                            }
                        }
                        response = "写入新的封禁记录失败了 请重试"; return false;
                    }
                    else
                    {
                        response = "日期输入有误 请重新输入(2077-12-31)"; return false;
                    }
                }
            }
        }
    }
    public class Allban
    {
        public static void Preauth(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if(ev != null)
            {
                new Task(() =>
                {
                    var apiresult = Methods.Checkban(ev.Player.UserId);

                    if (apiresult != null)
                    {
                        if (apiresult.Success)
                        {
                            var banrecord = apiresult.Data;
                            if (banrecord.IsBan)
                            {
                                Log.Info("banned!");
                                var endtime = DateTime.ParseExact(banrecord.Endtime, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                                ev.Player.Kick($"\n你已被服务器封禁！封禁理由:{banrecord.Msg}\n到期时间:{banrecord.Endtime}\n如您有异议，请向服务器管理员发起申诉。");
                            }
                            else
                            {
                                //
                            }
                        }
                        else
                        {
                            //失败请求 丢弃
                        }
                    }


                }).Start();
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Verified += Preauth;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Verified -= Preauth;
        }
    }*/
}
