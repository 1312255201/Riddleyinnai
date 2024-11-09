using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Database;
using Riddleyinnai.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;

namespace Riddleyinnai.User
{
    public class Exp
    {
        public static int Server_plus = 1;//服务器倍率

        public static int scpnum;
        //缓存信息 用于插件内部读取
        public static void Onjoining(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if (ev != null)
            {
                /*new Task(() =>
                {
                    var res = Methods.Get_user(ev.Player.UserId);
                    if (res != null)
                    {
                        if (res.Success)
                        {
                            //很明显 这里是成功获取了
                            var expmodel = res.Data;
                            expmodel.Ipaddress = ev.Player.IPAddress;
                            expmodel.Nickname = ev.Player.Nickname;
                            expmodel.Lastkjointime = DateTime.Now.ToString("yyyy-MM-dd");
                            var update_res = Methods.Update_user(expmodel);
                            //更新用户信息
                            //然后处理用户信息
                            if (ev.Player.DoNotTrack)
                            {
                                Ui.PlayerMain.Send(ev.Player, "<b>您开启了DNT模式，我们不会记录您的隐私信息至服务器</b>", 5, Pos.正中偏下, 5, true);
                            }
                            else
                            {
                                Ui.PlayerMain.Send(ev.Player, $"<b>欢迎！尊敬的 <color=green>{ev.Player.Nickname}</color> 欢迎游玩<color=#FFC0CB>迷子音奈</color><color=#1E90FF>轻插件服</color></b>", 5, Pos.正中偏下, 5, true);
                                if (expmodel.Joinboardcast != "")
                                {
                                    var msgs = expmodel.Joinboardcast.Split('#');
                                    Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30>「<color=#DC143C>进服提示</color>」{msgs[0]} {ev.Player.Nickname} {msgs[1]} 服务器</size>", 5));
                                }
                                Usercache.Add(ev.Player.UserId, expmodel);
                            }
                        }
                        else
                        {
                            //小心处理 这里有两种情况
                            //1.真的没有
                            //2.服务器寄了 别覆盖
                            if (res.Code == 404)
                            {
                                //完全没找到
                                var expmodel = new Database.Model.User();
                                expmodel.Ipaddress = ev.Player.IPAddress;
                                expmodel.Nickname = ev.Player.Nickname;
                                expmodel.Userid = ev.Player.UserId;
                                expmodel.Lastkjointime = DateTime.Now.ToString("yyyy-MM-dd");
                                var update_res = Methods.Update_user(expmodel);
                                if (ev.Player.DoNotTrack)
                                {
                                    Ui.PlayerMain.Send(ev.Player, "<b>您开启了DNT模式，我们不会记录您的隐私信息至服务器</b>", 5, Pos.正中偏下, 5, true);
                                }
                                else
                                {
                                    Ui.PlayerMain.Send(ev.Player, $"<b>欢迎！尊敬的 <color=green>{ev.Player.Nickname}</color> 欢迎游玩<color=#FFC0CB>迷子音奈</color><color=#1E90FF>轻插件服</color></b>", 5, Pos.正中偏下, 5, true);
                                    if (expmodel.Joinboardcast != "")
                                    {
                                        var msgs = expmodel.Joinboardcast.Split('#');
                                        Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30>「<color=#DC143C>进服提示</color>」{msgs[0]} {ev.Player.Nickname} {msgs[1]} 服务器</size>", 5));
                                    }
                                    Usercache.Add(ev.Player.UserId, expmodel);
                                }
                            }
                            else
                            {
                                //服务器寄了
                                var expmodel = new Database.Model.User();
                                expmodel.Ipaddress = ev.Player.IPAddress;
                                expmodel.Nickname = ev.Player.Nickname;
                                expmodel.Userid = ev.Player.UserId;
                                expmodel.Lastkjointime = DateTime.Now.ToString("yyyy-MM-dd");
                                if (ev.Player.DoNotTrack)
                                {
                                    Ui.PlayerMain.Send(ev.Player, "<b>您开启了DNT模式，我们不会记录您的隐私信息至服务器</b>", 5, Pos.正中偏下, 5, true);
                                }
                                else
                                {
                                    Ui.PlayerMain.Send(ev.Player, $"<b>欢迎！尊敬的 <color=green>{ev.Player.Nickname}</color> 欢迎游玩<color=#FFC0CB>迷子音奈</color><color=#1E90FF>轻插件服</color></b>", 5, Pos.正中偏下, 5, true);
                                    Usercache.Add(ev.Player.UserId, expmodel);
                                }
                            }
                        }
                    }
                }).Start();*/
            }
        }
        public static void Reset()
        {
            var timenow = DateTime.Now;
            if((timenow.Hour >= 19 && timenow.Hour <= 23) || (timenow.Hour >= 0 && timenow.Hour <= 4) || timenow.Hour == 13)
            {
                if(timenow.Hour == 23 || timenow.Hour == 13)
                {
                    Server_plus = 13;
                }
                else
                {
                    Server_plus = 6;
                }
            }
            else
            {
                Server_plus = Main.Singleton.Config.Server_plus;
            }
            KillCount.Clear();
            Levellist.Clear();
            for(int i = 1;i<=1500;i++)
            {
                Levellist.Add(i, Level_exp(i));
            }
        }
        public static int Getlevel(long exp)
        {
            if(Levellist.Count(x => x.Value >= exp) >= 1)
            {
                return Levellist.FirstOrDefault(x => x.Value >= exp).Key;
            }
            return -1;
        }
        public enum Exptype
        {
            击杀MTF,
            击杀CI,
            击杀SCP,
            击杀D级人员,
            击杀科学家,
            承受伤害,
            复活尸体,
            协助SCP队友,
            捕获,
            存活时间,
            回合奖励,
            逃离设施,
            协助逃脱,
            使用SCP物品,
            并肩作战,
            逃离口袋,
            侵入设施,
            掩护队友,
            阻拦人类,
            解锁成就,
            击杀SCP0492,
            击杀狂暴的SCP096,
            击杀SCP096,
            击杀SCP939,
            击杀SCP049,
            击杀SCP173
        }
        public static float Addexp(string userid, float exp, Exptype exptype, int kills = 1)
        {
            exp *= Server_plus;
            {
                if (exp <= 10)
                {
                    if (Player.TryGet(userid, out var player))
                    {
                        Ui.PlayerMain.Send(player, $"<color=green>「▶{exptype}◀」经验 + <color=white>{exp}</color></color>", 4, Pos.正中偏下, 20, false);
                    }
                }
                else if (exp <= 20)
                {
                    if (Player.TryGet(userid, out var player))
                    {
                        Ui.PlayerMain.Send(player, $"<color=yellow>「▶{exptype}◀」经验 + <color=white>{exp}</color></color>", 4, Pos.正中偏下, 20, false);
                    }
                }
                else if (exp <= 35)
                {
                    if (Player.TryGet(userid, out var player))
                    {
                        Ui.PlayerMain.Send(player, $"<color=orange>「▶{exptype}◀」经验 + <color=white>{exp}</color></color>", 4, Pos.正中偏下, 20, false);
                    }
                }
                else
                {
                    if (Player.TryGet(userid, out var player))
                    {
                        Ui.PlayerMain.Send(player, $"<color=red>「▶{exptype}◀」经验 + <color=white>{exp}</color></color>", 4, Pos.正中偏下, 20, false);
                    }
                }
                Methods.AddExp(userid, exp);
            }
            return exp;
        }
        public class Kills
        {
            public string userid { get; set; }
            public int count { get; set; } = 1;
            public DateTime lasttime { get; set; } = DateTime.MinValue;
        }
        private static List<Kills> KillCount = new List<Kills>();
        public static void OnDied(DyingEventArgs ev)
        {
            if (ev != null && ev.Attacker != null)
            {
                if (ev.Attacker == ev.Player) return;
                if (ev.Attacker.IsScp)
                {
                    var mulkill = 1;
                    if (KillCount.Count(x => x
                    .userid == ev.Attacker.UserId) >= 1)
                    {
                        //判断连杀
                        var model = KillCount.Find(x => x
                    .userid == ev.Attacker.UserId);
                        if (model.lasttime - DateTime.Now <= TimeSpan.FromSeconds(5))
                        {
                            KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).count += 1;
                            KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).lasttime = DateTime.Now;
                            mulkill = model.count + 1;
                        }
                    }
                    else
                    {
                        KillCount.Add(new Kills() { userid = ev.Attacker.UserId, lasttime = DateTime.Now, count = 1 });
                    }
                    switch (ev.Player.Role.Type)
                    {
                        case PlayerRoles.RoleTypeId.ChaosConscript:
                        case PlayerRoles.RoleTypeId.ChaosMarauder:
                        case PlayerRoles.RoleTypeId.ChaosRepressor:
                        case PlayerRoles.RoleTypeId.ChaosRifleman:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀CI, mulkill); break;
                        case PlayerRoles.RoleTypeId.NtfCaptain:
                        case PlayerRoles.RoleTypeId.NtfPrivate:
                        case PlayerRoles.RoleTypeId.NtfSergeant:
                        case PlayerRoles.RoleTypeId.NtfSpecialist:
                        case PlayerRoles.RoleTypeId.FacilityGuard:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀MTF, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scientist:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀科学家, mulkill); break;
                        case PlayerRoles.RoleTypeId.ClassD:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀D级人员, mulkill); break;


                    }

                }
                else
                {
                    var mulkill = 1;
                    if (KillCount.Count(x => x
                    .userid == ev.Attacker.UserId) >= 1)
                    {
                        //判断连杀
                        var model = KillCount.Find(x => x
                    .userid == ev.Attacker.UserId);
                        if (model.lasttime - DateTime.Now <= TimeSpan.FromSeconds(5))
                        {
                            KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).count += 1;
                            KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).lasttime = DateTime.Now;
                            mulkill = KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).count ;
                        }
                        else
                        {
                            KillCount.Find(x => x
.userid == ev.Attacker.UserId).count = 1;
                            KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).lasttime = DateTime.Now;
                            mulkill = KillCount.Find(x => x
                    .userid == ev.Attacker.UserId).count ;
                        }
                    }
                    else
                    {
                        KillCount.Add(new Kills() { userid = ev.Attacker.UserId, lasttime = DateTime.Now, count = 1 });
                    }
                    switch (ev.Player.Role.Type)
                    {
                        case PlayerRoles.RoleTypeId.ChaosConscript:
                        case PlayerRoles.RoleTypeId.ChaosMarauder:
                        case PlayerRoles.RoleTypeId.ChaosRepressor:
                        case PlayerRoles.RoleTypeId.ChaosRifleman:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀CI, mulkill); break;
                        case PlayerRoles.RoleTypeId.NtfCaptain:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀MTF, mulkill); break;
                        case PlayerRoles.RoleTypeId.NtfPrivate:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀MTF, mulkill); break;
                        case PlayerRoles.RoleTypeId.NtfSergeant:
                        case PlayerRoles.RoleTypeId.NtfSpecialist:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀MTF, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scientist:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀科学家, mulkill); break;
                        case PlayerRoles.RoleTypeId.ClassD:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀D级人员, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scp0492:
                            Addexp(ev.Attacker.UserId, 1, Exptype.击杀SCP0492, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scp096:
                            if(ev.Player.Role.Is<Scp096Role>(out var role))
                            {
                                if(role.EnragedTimeLeft >= 1)
                                {
                                    Addexp(ev.Attacker.UserId, 10, Exptype.击杀狂暴的SCP096, mulkill); break;
                                }
                                else
                                {
                                    Addexp(ev.Attacker.UserId, 10, Exptype.击杀SCP096, mulkill); break;
                                }
                            }
                            break;
                        case PlayerRoles.RoleTypeId.Scp049:
                            Addexp(ev.Attacker.UserId, 10, Exptype.击杀SCP049, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scp173:
                            Addexp(ev.Attacker.UserId, 10, Exptype.击杀SCP173, mulkill); break;
                        case PlayerRoles.RoleTypeId.Scp939:
                            Addexp(ev.Attacker.UserId, 10, Exptype.击杀SCP939, mulkill); break;
                    }
                }
            }
        }
        public static void OnEscaping(EscapingEventArgs ev)
        {
            if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
            {
                if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedScientist)
                {
                    Addexp(ev.Player.UserId, 8 , Exptype.逃离设施, 1);
                    foreach (var item in Player.Get(x => x.Role.Side == ev.Player.Cuffer.Role.Side && x != ev.Player))
                    {
                        Addexp(item.UserId, 1, Exptype.协助逃脱, 1);
                    }
                }
                else if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.Scientist)
                {
                    Addexp(ev.Player.UserId, 8, Exptype.逃离设施, 1);
                    foreach (var item in Player.Get(x => x.Role.Side == ev.Player.Role.Side && x != ev.Player))
                    {
                        Addexp(item.UserId, 1, Exptype.协助逃脱, 1);
                    }
                }
            }
            else if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
            {
                if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedClassD)
                {
                    Addexp(ev.Player.UserId, 8, Exptype.逃离设施, 1);
                    foreach (var item in Player.Get(x => x.Role.Side == ev.Player.Cuffer.Role.Side && x != ev.Player))
                    {
                        Addexp(item.UserId, 1, Exptype.协助逃脱, 1);
                    }
                }
                else if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.ClassD)
                {
                    Addexp(ev.Player.UserId, 8, Exptype.逃离设施, 1);
                    foreach (var item in Player.Get(x => x.Role.Side == ev.Player.Role.Side && x != ev.Player))
                    {
                        Addexp(item.UserId, 1, Exptype.协助逃脱, 1);
                    }
                }
            }
        }
        public static void OnUsedSCPitems(UsedItemEventArgs ev)
        {

        }
        

        public static void OnPocketFailed(FailingEscapePocketDimensionEventArgs ev)
        {
            foreach (var variablPlayer in Player.List.Where(x=>x.Role.Type == RoleTypeId.Scp106))
            {
                Addexp(variablPlayer.UserId, 1, Exptype.逃离口袋, 1);
            }
        }
        public static void OnEscapingpocket(EscapingPocketDimensionEventArgs ev)
        {
            Addexp(ev.Player.UserId, 1, Exptype.逃离口袋, 1);
        }
        public static  void Gainexp(GainingExperienceEventArgs ev)
        {
            if(ev.GainType == PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationAssist)
            {
                Addexp(ev.Player.UserId, 1, Exptype.协助SCP队友);
            }
            else if(ev.GainType == PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTerminationDirect)
            {
                Addexp(ev.Player.UserId, 2, Exptype.侵入设施);
            }
            else if(ev.GainType == PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainTeammateProtection || ev.GainType == PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainHidStopped)
            {
                Addexp(ev.Player.UserId, 1, Exptype.掩护队友);
            }
            else if(ev.GainType == PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ExpGainBlockingHuman)
            {
                Addexp(ev.Player.UserId, 1, Exptype.阻拦人类);
            }
        }
        public static void Revive(FinishingRecallEventArgs ev)
        {
            if(ev.IsAllowed)
            {
                Addexp(ev.Player.UserId, 1, Exptype.复活尸体);
            }
        }
        public static void OnRoundending(RoundEndedEventArgs ev)
        {
            foreach (var item in Player.List.Where(x => !x.IsOverwatchEnabled))
            {
                Addexp(item.UserId, 2, Exptype.回合奖励);
            }
        }
        public static void OnLeft(LeftEventArgs ev)
        {
            KillCount.RemoveAll(x => x.userid == ev.Player.UserId);
           /*if(Usercache.TryGetValue(ev.Player.UserId,out var model))
            {
                new Task(() =>
                {
                    Methods.Updateuser(model);
                }).Start();
            }
            Usercache.Remove(ev.Player.UserId);*/
        }
        private static Dictionary<int,long> Levellist = new Dictionary<int, long>();
        public static long Level_exp(long level)
        {
            return level * level / 2 + 150 * level;
        }
        public static string GetLowerline(Player player)
        {
            var hour = DateTime.Now.Hour;
            var time = "";
            if (hour >= 0 && hour <= 6)
            {
                time = "夜深了";
            }
            else if (hour > 6 && hour <= 9)
            {
                time = "天亮了";
            }
            else if (hour > 9 && hour <= 12)
            {
                time = "早上好";
            }
            else if(hour > 12 && hour <= 18)
            {
                time = "下午好";
            }
            else if(hour > 18 && hour <= 23)
            {
                time = "晚上好";
            }
            else
            {
                time = "夜深了";
            }
            var sprank = "";
            long model = 0;
            var Level = 0;
            model = Methods.GetExp(player.UserId);
            Level = Getlevel(model);
            sprank = RoleManger.IsRole(player.Id) ? RoleManger.GetRole(player.Id).ToString() : YYYApi.MyApi.TranslateOfRoleType[player.Role.Type];
            var str = $"{time} <color=green><b>{player.Nickname}</b></color><b>丨</b>{sprank}<b>丨</b><b>Lv.{Level}</b> 玩家群号::<b>875268324</b> <size=10>测试品质不代表最终品质 <color=red>Beta测试版本 TPS:{Server.Tps}</color></size></voffset></size>";
            return str;
        }
        public static string GetItemMsg(long EXP)
        {
            var Level = Getlevel(EXP);
            string msg = "";
            if (Level >= 1001)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:区域总监钥匙卡、急救包、HP提升、糖果×2";
            }
            else if (Level >= 801)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:区域总监钥匙卡、急救包、HP提升、糖果×2";
            }
            else if (Level >= 501)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:科学家钥匙卡、手电筒、急救包、轻型护甲、糖果×1";
            }
            else if (Level >= 301)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:科学家钥匙卡、手电筒、急救包、肾上腺素、糖果×1";
            }
            else if (Level >= 201)
            {
                msg = $"您当前等级{Level} 科学家钥匙卡、手电筒、急救包、肾上腺素";
            }
            else if (Level >= 101)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:清洁工钥匙卡、手电筒、急救包";
            }
            else if (Level >= 51)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:清洁工钥匙卡、手电筒、止痛药";
            }
            else if (Level >= 0)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:黄卡（萌新保护）";
            }
            else if (Level == -1)
            {
                msg = $"您当前等级{Level} 您已获取开局福利:区域总监钥匙卡、急救包、HP提升、糖果×2";
            }
            return msg;
        }
        public static List<ItemType> GetItemType(long EXP)
        {
            var Level = Getlevel(EXP);
            var Items = new List<ItemType>();
            if (Level >= 1001)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardZoneManager,
                    ItemType.Medkit,
                    ItemType.SCP330,
                    ItemType.SCP330
                };
            }
            else if (Level >= 801)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardZoneManager,
                    ItemType.Medkit,
                    ItemType.SCP330,
                    ItemType.SCP330
                };
            }
            else if (Level >= 501)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardScientist,
                    ItemType.Medkit,
                    ItemType.Flashlight,
                    ItemType.ArmorLight,
                    ItemType.SCP330
                };
            }
            else if (Level >= 301)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardScientist,
                    ItemType.Medkit,
                    ItemType.Flashlight,
                    ItemType.Adrenaline,
                    ItemType.SCP330
                };
            }
            else if (Level >= 201)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardScientist,
                    ItemType.Medkit,
                    ItemType.Flashlight,
                    ItemType.Adrenaline,
                };
            }
            else if (Level >= 101)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardJanitor,
                    ItemType.Medkit,
                    ItemType.Flashlight,
                };
            }
            else if (Level >= 51)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardJanitor,
                    ItemType.Painkillers,
                    ItemType.Flashlight,
                };
            }
            else if (Level >= 0)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardScientist,
                };
            }
            else if (Level == -1)
            {
                Items = new List<ItemType>()
                {
                    ItemType.KeycardScientist,
                };
            }
            return Items;
        }
        private static void OnRoundStart()
        {
            Timing.CallDelayed(5f, () =>
            {
                scpnum = Player.List.Count(x => x.Role.Team == Team.SCPs);
            });
        }
        public static void SpawnedItems(SpawnedEventArgs ev)
        {
            if(ev.Reason == Exiled.API.Enums.SpawnReason.RoundStart)
            {
                if(ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scientist || ev.Player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                {
                    Timing.CallDelayed(1f, () =>
                    {
                        long model = 0;
                        var level = 0;
                        model = Methods.GetExp(ev.Player.UserId);
                        level = Getlevel(model);
                        var items = GetItemType(model);
                        var msg = GetItemMsg(model);
                        foreach (var type in items)
                        {
                            if (!ev.Player.HasItem(type))
                            {
                                ev.Player.AddItem(type);
                            }
                        }
                        if (level >= 600)
                        {
                            //每一百级增加五点初始血量
                            var plus = level - 600;
                            plus /= 100;
                            ev.Player.MaxHealth += plus * 5;
                            ev.Player.Health += plus * 5;
                        }
                        Ui.PlayerMain.Send(ev.Player, msg, 5, Pos.顶部正中, 5, false);
                    });
                }
            }
        }
        public static void Rolechanged(ChangingRoleEventArgs ev)
        {
            if(ev.Reason == Exiled.API.Enums.SpawnReason.Escaped)
            {
                if(ev.Player.Role.Type != PlayerRoles.RoleTypeId.ClassD && ev.Player.Role.Type != PlayerRoles.RoleTypeId.Scientist)
                {
                    Addexp(ev.Player.UserId, 8, Exptype.逃离设施, 1);
                }
            }
        }
        public static void Onescaping()
        {
            foreach (var fg in Player.Get(PlayerRoles.RoleTypeId.FacilityGuard))
            {
                if (Vector3.Distance(fg.Position, new Vector3(123.722f, 988.792f, 19.140f)) <= 5)
                {
                    fg.Role.Set(PlayerRoles.RoleTypeId.NtfSergeant, reason: Exiled.API.Enums.SpawnReason.Escaped);
                }

            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Spawned += SpawnedItems;
            Exiled.Events.Handlers.Player.ChangingRole += Rolechanged;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundending;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Verified += Onjoining;
            Exiled.Events.Handlers.Player.Dying += OnDied;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedSCPitems;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += OnPocketFailed;
            Exiled.Events.Handlers.Scp079.GainingExperience += Gainexp;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += OnEscapingpocket;
            Exiled.Events.Handlers.Scp049.FinishingRecall += Revive;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Spawned -= SpawnedItems;
            Exiled.Events.Handlers.Player.ChangingRole -= Rolechanged;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundending;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Verified -= Onjoining;
            Exiled.Events.Handlers.Player.Dying -= OnDied;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedSCPitems;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= OnPocketFailed;
            Exiled.Events.Handlers.Scp079.GainingExperience -= Gainexp;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= OnEscapingpocket;
            Exiled.Events.Handlers.Scp049.FinishingRecall -= Revive;
        }
    }
}
