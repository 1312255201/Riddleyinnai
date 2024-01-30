using CommandSystem;
using Exiled.API.Features;
using Riddleyinnai.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ICommand = CommandSystem.ICommand;

namespace Riddleyinnai.User.Achieve
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Achievecommand : ICommand
    {
        public string Command => "achievement";

        public string[] Aliases => new string[] { "ach"};

        public string Description => "查询用户的成就完成情况";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var strline = "";
            var pl = Player.Get(sender);
            foreach(var item in Main.achievecache[pl.UserId].Achieves.OrderBy(x=>x.id))
            {
                if(item.hide)
                {
                    strline += $"[{item.id}] {item.name}";
                    if(item.status)
                    {
                        strline += $" 解锁条件:{item.description} [<color=green>√</color>]";
                    }
                    else
                    {
                        strline += $" 解锁条件:??? [<color=red>×</color>]";
                    }
                }
                else
                {
                    strline += $"[{item.id}] {item.name}";
                    if (item.status)
                    {
                        strline += $" 解锁条件:{item.description} [<color=green>√</color>]";
                    }
                    else
                    {
                        strline += $" 解锁条件:{item.description} [<color=red>×</color>]";
                    }
                }
                strline += "\n";
            }
            response = strline.ToString();
            return true;
        }
    }
    internal class Main
    {
        public static List<Database.Model.Achieve> achieves = new List<Database.Model.Achieve>()
        { 
            new Database.Model.Achieve() { id = 1,name="落地成盒",description="重生为MTF或者CI后30秒阵亡",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 2,name="破局者",description="本回合第一个击杀SCP",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 3,name="战神",description="本回合击杀数最多",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 4,name="地板烫脚",description="一回合内跳跃200次",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 5,name="换弹癌",description="一回合内换弹100次",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 6,name="颈部按摩享受者",description="一回合被SCP173击杀5次",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 7,name="幽灵猎手",description="一回合击杀5次SCP0492",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 8,name="艺术就是爆炸！",description="作为九尾引爆核弹时，一次性收容7名SCP",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 9,name="匠人精神",description="本回合使用914任意加工模式50次",status=false,count = 0,hide = false,progress = 0},
            new Database.Model.Achieve() { id = 10,name="和平主义者",description="没有拾取任何武器，成功在3分钟内逃离设施",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 11,name="好习惯",description="本局比赛开关气闸门100次",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 12,name="我不是药神",description="一回合中使用药品道具数量最多",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 13,name="雷霆之力",description="使用电磁炮在10秒内连续杀死3名以上的SCP",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 14,name="狂战士",description="血量仅为1时杀死一名SCP",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 15,name="(),启动！",description="被闪光弹致盲",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 16,name="熟悉的感觉",description="作为SCP096，在SCP106在身边的情况下拆毁106收容室的大门",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 17,name="耐摔王",description="一回合内受到的摔落伤害最高",status=false,count = 0,hide = true,progress = 0},
            new Database.Model.Achieve() { id = 18,name="口袋妖怪",description="一回合内成功从口袋空间逃出5次",status=false,count = 0,hide = true,progress = 0},
        };
        public static Dictionary<string, Database.Model.Achievement> achievecache = new Dictionary<string, Database.Model.Achievement>();
        public static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if(ev.Player != null)
            {
              /*  new Task(() =>
                {
                    var res = Methods.Getachievement(ev.Player.UserId);
                    if (res != null)
                    {
                        if (res.Success)
                        {
                            //成功获取
                            var list = achieves;
                            foreach (var ach in res.Data.Achieves)
                            {
                                list.RemoveAll(x => x.id == ach.id);
                                list.Add(ach);
                            }
                            res.Data.Achieves = list;
                            achievecache.Remove(ev.Player.UserId);
                            achievecache.Add(ev.Player.UserId, res.Data);
                        }
                        else
                        {
                            if (res.Code == 404)
                            {
                                var list = achieves;
                                Methods.Updateachievement(new Database.Model.Achievement() { Userid = ev.Player.UserId, Achieves = list });
                                achievecache.Remove(ev.Player.UserId);
                                achievecache.Add(ev.Player.UserId, new Database.Model.Achievement() { Userid = ev.Player.UserId, Achieves = list });
                            }
                            else
                            {
                                //别动
                            }
                        }
                    }

                }).Start();*/
            }
        }
        public static void UpdateAchievement(string userid,Database.Model.Achieve achieve)
        {
/*            if(achievecache.TryGetValue(userid,out var achievement))
            {
                var model = achievement.Achieves.Find(x => x.id == achieve.id);
                if(model != null)
                {
                    if (model.status) return;
                }
                if (achievement.Achieves.RemoveAll(x => x.id == achieve.id) >= 1)
                {
                    achievement.Achieves.Add(achieve);
                }
                achievecache[userid] = achievement;
                if(Player.TryGet(userid,out var pl))
                {
                    Ui.PlayerMain.Send(pl, $"<b>您已解锁成就:[<color=red>{achieve.name}</color>]</b>",Pos:Ui.Pos.正中偏下, weight:100);
                    Exp.Addexp(userid, 50, Exp.Exptype.解锁成就, 1);
                }
                new Task(() =>
                {
                    Methods.Updateachievement(achievement);

                }).Start();
            }
            else
            {
                new Task(() =>
                {
                    var res = Methods.Getachievement(userid);
                    if (res != null)
                    {
                        if (res.Success)
                        {
                            //成功获取
                            var model = res.Data.Achieves.Find(x => x.id == achieve.id);
                            if (model != null)
                            {
                                if (model.status) return;
                            }
                            var list = achieves;
                            foreach (var ach in res.Data.Achieves)
                            {
                                list.RemoveAll(x => x.id == ach.id);
                                list.Add(ach);
                            }
                            res.Data.Achieves = list;
                            if (res.Data.Achieves.RemoveAll(x => x.id == achieve.id) >= 1)
                            {
                                res.Data.Achieves.Add(achieve);
                            }
                            if (Player.TryGet(userid, out var pl))
                            {
                                Ui.PlayerMain.Send(pl, $"<b>您已解锁成就:[<color=red>{achieve.name}</color>]</b>", Pos: Ui.Pos.正中偏下);
                                Exp.Addexp(userid, 50, Exp.Exptype.解锁成就, 1);
                            }
                            Methods.Updateachievement(res.Data);
                        }
                        else
                        {
                            if (res.Code == 404)
                            {
                                var list = achieves;
                                if (list.RemoveAll(x => x.id == achieve.id) >= 1)
                                {
                                    list.Add(achieve);
                                }
                                if (Player.TryGet(userid, out var pl))
                                {
                                    Ui.PlayerMain.Send(pl, $"<b>您已解锁成就:[<color=red>{achieve.name}</color>]</b>", Pos: Ui.Pos.正中偏下);
                                    Exp.Addexp(userid, 50, Exp.Exptype.解锁成就, 1);
                                }
                                Methods.Updateachievement(new Database.Model.Achievement() { Userid = userid, Achieves = list });

                            }
                            else
                            {
                                //别动
                            }
                        }
                    }
                }).Start();
            }*/
        }
        public static void Reset()
        {
            achievecache.Clear();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Verified += OnJoined;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Verified -= OnJoined;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
        }

    }
}
