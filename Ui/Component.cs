using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using HarmonyLib;
using PlayerRoles;
using PluginAPI.Roles;
using Respawning;
using Riddleyinnai.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Ui
{
    public class Component:MonoBehaviour
    {
        public Player player;
        private float _counter;
        private List<Ui.Model> msgs = new List<Model>();
        private void Update()
        {
            _counter += Time.deltaTime;
            if (_counter < 2)
                return;
            _counter = 0;
            if(player != null)
            {
                var linecount = 0;
                msgs.RemoveAll(x => x.endtime < DateTime.Now);
                //start
                string msgshow = "<voffset=8.3em><size=70%><line-height=110%>";
                #region 顶部信息
                var topmsglist = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.顶部两行);
                if(topmsglist != null)
                {
                    IEnumerable<Ui.Model> query = from item in topmsglist
                                                orderby item.endtime, item.weight descending
                                                select item;
                    foreach (var item in query.Take(2))
                    {
                        if(item.countdown)
                        {
                            try
                            {
                                linecount++;
                                msgshow += $"[{(item.endtime - DateTime.Now).TotalSeconds.ToString("F0")}]" + item.text + "\n";
                            }
                            catch(Exception ex)
                            {
                                Log.Error(ex.Message);
                            }
                        }
                        else
                        {
                            linecount++;
                            msgshow += item.text + "\n";
                        }
                    }
                    for(int i = 0;i< 2 - query.Count(); i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }

                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                #endregion
                msgshow += "\n";//间隔行
                linecount++;
                #region 顶部2
                var top2msglist = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.顶部正中);
                if( top2msglist != null)
                {
                    IEnumerable<Ui.Model> query = from item in top2msglist
                                                  orderby item.endtime, item.weight descending
                                                  select item;
                    foreach (var item in query.Take(2))
                    {
                        if (item.countdown)
                        {
                            try
                            {
                                linecount++;
                                msgshow += $"[{(item.endtime - DateTime.Now).TotalSeconds.ToString("F0")}]" + item.text + "\n";
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex.Message);
                            }
                        }
                        else
                        {
                            linecount++;
                            msgshow += item.text + "\n";
                        }
                    }
                    for (int i = 0; i < 2 - query.Count(); i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                #endregion
                #region 九尾狐
                var scpcount = 0;
                if (player.Role.Side == Exiled.API.Enums.Side.Mtf)
                {
                    msgshow += "<align=\"right\"><size=75%><b>";
                    var mtfcount = Player.List.Count(x => x.Role.Side == Exiled.API.Enums.Side.Mtf);
                    if (mtfcount >= 1) 
                    {
                        linecount++;
                        msgshow += $"<size=23>[<color={RoleTypeId.NtfCaptain.GetColor().ToHex()}>基金会阵营</color>]当前人数:{mtfcount}</size>";
                        for (int i = 4; i >= 0; i--)
                        {
                            linecount++;
                            msgshow += "\n";
                        }
                    }
                    msgshow += "</size></align></b>";
                }
                #endregion
                #region 混沌
                else if (player.Role.Side == Exiled.API.Enums.Side.ChaosInsurgency)
                {
                    msgshow += "<align=\"right\"><size=75%><b>";
                    var mtfcount = Player.List.Count(x => x.Role.Side == Exiled.API.Enums.Side.ChaosInsurgency);
                    if (mtfcount >= 1)
                    {
                        linecount++;
                        msgshow += $"<size=23>[<color={RoleTypeId.ChaosConscript.GetColor().ToHex()}>混沌阵营</color>]当前人数:{mtfcount}</size>";
                        for (int i = 4; i >= 0; i--)
                        {
                            linecount++;
                            msgshow += "\n";
                        }
                    }
                    msgshow += "</size></align></b>";
                }
                #endregion
                #region Scps&Others
               
                else if(player.Role.Side == Exiled.API.Enums.Side.Scp)
                {
                    msgshow += "<align=\"right\"><size=75%><b>";
                    foreach (var p in Player.Get(x=>x.IsScp))
                    {
                        if (p.Role == RoleTypeId.Scp079)
                        {
                            var level = 1;
                            float power = 0;
                            if (p.Role.Is<Scp079Role>(out Scp079Role role))
                            {
                                level = role.Level;
                                power = role.Energy;
                            }
                            linecount++;
                            scpcount++;
                            msgshow += "\n<b><align=right><size=23><b>[</b>" + "<color=#cc0000>" + p.Role.Type + "</color>" + "<b>]</b> <b>等级</b> " + level + " 电量 " + "<color=#6699ff>" + power + "</color>" + "</size></align><b>";
                        }
                        else if(p.Role.Type != RoleTypeId.Scp0492)
                        {
                            var str = p.Role.Type.ToString();
                            var healthcolor = "#66ff66";
                            if (p.Health <= p.MaxHealth * 0.6f)
                            {
                                healthcolor = "#ffcc99";
                            }
                            else if (p.Health <= p.MaxHealth * 0.25f)
                            {
                                healthcolor = "#cc0000";
                            }
                            var zone = "";
                            switch (p.Zone)
                            {
                                case Exiled.API.Enums.ZoneType.Surface:zone = "地表";break;
                                case Exiled.API.Enums.ZoneType.Entrance: zone = "办公"; break;
                                case Exiled.API.Enums.ZoneType.LightContainment: zone = "轻收"; break;
                                case Exiled.API.Enums.ZoneType.HeavyContainment: zone = "重收"; break;
                                default: zone = "未知"; break;
                            }
                            linecount++;
                            scpcount++;
                            msgshow += "\n<b><align=right><size=23><b>[</b>" + "<color=#cc0000>" + p.Role.Type + "</color>" + "<b>]</b> <b>HP</b> " + $"<color={healthcolor}>" + p.Health + "</color>" + " HS " + "<color=#6699ff>" + p.HumeShield + "</color>" + $"[<color=#FF0000>{zone}</color>]" +"</size></align></b>";
                        }
                    }
                    var zombies = Player.Get(x => x.Role.Type == RoleTypeId.Scp0492).Count();
                    if (Player.Get(x => x.Role.Type == RoleTypeId.Scp0492).Count() >= 1)
                    {
                        linecount++;
                        scpcount++;
                        msgshow += "\n<b><align=right><size=23><b>[</b>" + "<color=#cc0000>" + "小僵尸:" + "</color>" + "<b>]</b> " + $"剩余数量 {zombies}" + "</size></align></b>";
                    }
                    for(int i = 4 - scpcount;i>=0;i--)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                    msgshow += "</size></align></b>";
                }
                else
                {
                    msgshow += "<align=\"right\"><size=75%><b>";
                    msgshow += "\n";//间隔行
                    msgshow += "\n";//间隔行
                    msgshow += "\n";//间隔行
                    msgshow += "\n";//间隔行
                    msgshow += "</size></align></b>";
                    linecount +=4;
                }
                if (scpcount < 4)
                {
                    scpcount = 4;
                }
                #endregion
                #region 聊天信息
                if(scpcount > 4)
                {
                    msgshow += "\n";
                    linecount++;
                }
                var chatmsglist = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.屏幕正中左边五行);
                if (chatmsglist != null)
                {
                    IEnumerable<Ui.Model> query = from item in chatmsglist
                                                  orderby item.endtime, item.weight descending
                                                  select item;
                    msgshow += "<align=\"left\"><size=70%><b>";
                    foreach (var item in query.Take(5))
                    {
                        try
                        {
                            linecount++;
                            if (item.countdown)
                            {
                                msgshow += $"[{(item.endtime - DateTime.Now).TotalSeconds.ToString("F0")}]" + item.text + "\n";
                            }
                            else {
                                msgshow += item.text + "\n";
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                        }
                    }
                    for (int i = 0; i < 5 - query.Count(); i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                msgshow += "</size></align></b>";
                msgshow += "\n"; //gap
                linecount++;
                #endregion
                #region 正下方
                var itemmsglist = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.正下方);
                if (itemmsglist != null)
                {
                    IEnumerable<Ui.Model> query = from item in itemmsglist
                                                  orderby item.endtime, item.weight descending
                                                  select item;
                    msgshow += "<align=\"right\"><size=70%><b>";

                    var querycount = 0;

                    foreach (var item in query.Take(5))
                    {
                        try
                        {
                            linecount++;
                            querycount++;
                            msgshow += $"[{(item.endtime - DateTime.Now).TotalSeconds.ToString("F0")}]" + item.text + "\n";
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                        }
                        
                    }
                    for (int i = 0; i < 5 - querycount; i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        linecount++;
                        msgshow += "\n";
                    }
                }
                msgshow += "</size></align></b>";
                msgshow += "\n";
                msgshow += "\n";
                linecount+=2;
                #endregion
                #region 死者信息
                if (player.Role.Type == PlayerRoles.RoleTypeId.Spectator || player.Role.Type == PlayerRoles.RoleTypeId.Overwatch)
                {
                    var bottomline = 0;
                    
                    var bottommsgs = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.正中偏下);
                    if (bottommsgs.Any())
                    {
                        IEnumerable<Ui.Model> query = from item in bottommsgs
                                                      orderby item.endtime, item.weight descending
                                                      select item;
                        foreach (var item in query.Take(2))
                        {
                            msgshow += $"{item.text}\n";
                            bottomline += 1;
                        }
                    }
                    
                    for (int i = 2 - bottomline; i >= 0; i--)
                    {
                        msgshow += "\n";
                    }

                    var nextteam = "";
                    if(Respawn.NtfTickets > Respawn.ChaosTickets)
                    {
                        nextteam = $"<color={RoleTypeId.NtfCaptain.GetColor().ToHex()}>九尾狐大军</color>";
                    }
                    else
                    {
                        nextteam = $"<color={RoleTypeId.ChaosMarauder.GetColor().ToHex()}>混沌分裂者</color>";
                    }

                    var msg = $"<size=25>" +
    $"\n<align=right><color=#ffffff>下次出生</color> <color=#ffffff>{Convert.ToInt32(TimeSpan.FromSeconds((int)RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds).TotalSeconds)}</color><color=#ffffff>秒后</color></align>" +
    $"\n<size=25><align=right><color=#ffffff>下次出生阵营:{nextteam}</size></align></size>";
                    
                    msgshow += msg;
                    msgshow += $"\n<align=right><color=#ffffff>Tips:{PlayerMain.Gettips(player)}</color></align>";
                    msgshow += "\n";
                    msgshow += "\n";
                    msgshow += "\n";
                    msgshow += $"{Exp.GetLowerline(player)}</voffset></size>";
                    
                }
                else
                {
                    var msg = "";
                    var bottomline = 0;
                    var bottommsgs = msgs.Where(x => x.endtime >= DateTime.Now && x.player == player && x.pos == Pos.正中偏下);
                    if(bottommsgs.Any())
                    {
                        IEnumerable<Ui.Model> query = from item in bottommsgs
                                                      orderby item.endtime, item.weight descending
                                                      select item;
                        foreach(var item in  query.Take(10 - scpcount))
                        {
                            msgshow += $"{item.text}\n";
                            bottomline += 1 ;
                        }
                    }
                    if (Round.IsLobby)
                    {
                        
                        var timer = GameCore.RoundStart.singleton.NetworkTimer;

                        var addmsg = "";
                        switch (timer)
                        {
                            case -2: addmsg = "<color=red>回合已暂停</color>"; break;
                            case 0:
                            case -1: addmsg = "<color=red>回合即将开始</color>"; break;

                            default: addmsg = $"<color=yellow> {timer} </color>秒后开始回合</color>"; break;
                        }
                        var msgborn = $"<b><color=green>{Player.List.Count}</color> 个玩家已连接至服务器 ,大厅状态: {addmsg}</b>\n";

                        msgshow += msgborn;

                        msgshow += $"<b>现在测试版本暂时不能参与角色自选哦~</b>";
                        bottomline+=2;
                        linecount++;
                    }
                    for (int i = 0;i<=10-scpcount - bottomline;i++)
                    {
                        msgshow += "\n";
                    }
                    msgshow += msg;
                    msgshow += "\n";
                    msgshow += "\n";
                    if(linecount == 15)
                    {
                        msgshow += "\n";
                        msgshow += "\n";
                    }
                    else
                    {
                        msgshow += "\n";
                    }
                    msgshow += $"{Exp.GetLowerline(player)}</voffset></size>";
                    
                }
                #endregion
                player.ShowHint(msgshow,1);
            }
        }
        public void Add(string text,int time = 5,Pos Pos = Pos.顶部正中,int weight = 5,bool countdown = true)
        {
            msgs.Add(new Model() { countdown = countdown,endtime = DateTime.Now.AddSeconds(time),text = text,player = player,pos = Pos,weight = weight });
        }
    }
}
