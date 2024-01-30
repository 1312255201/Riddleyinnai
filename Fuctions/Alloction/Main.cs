

using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MapGeneration;
using MEC;
using PlayerRoles;
using PluginAPI.Roles;
using Riddleyinnai.Database;
using Riddleyinnai.Database.Model;
using Riddleyinnai.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Alloction
{
    internal class Main
    {
        public class Request
        {
            public string userid { get; set; }
            public RoleTypeId role { get; set; }
            public int luck { get; set; }
            public bool ischoice { get; set; } = false;
        }
        private static int SCPCount = 0;
        private static int DDCount = 0;
        private static int DRCount = 0;
        private static int FGCount = 0;
        public static bool Check(RoleTypeId roleTypeId)
        {
            if (roleTypeId.GetSide() == Exiled.API.Enums.Side.Scp)
            {
                return SCPCount > 0;
            }
            else if (roleTypeId == RoleTypeId.ClassD)
            {
                return DDCount > 0;
            }
            else if (roleTypeId == RoleTypeId.Scientist)
            {
                return DRCount > 0;
            }
            else if (roleTypeId == RoleTypeId.FacilityGuard)
            {
                return FGCount > 0;
            }
            return true;
        }
        private static void GetCounts(int n)
        {
            switch (n)
            {
                case 1: DDCount = 1; break;
                case 2: SCPCount = 1; DDCount = 1; break;
                case 3: SCPCount = 1; DDCount = 1; FGCount = 1; break;
                case 4: SCPCount = 1; DDCount = 2; FGCount = 1; break;
                case 5: SCPCount = 1; DDCount = 2; FGCount = 1; DRCount = 1; break;
                case 6: SCPCount = 1; DDCount = 3; FGCount = 1; DRCount = 1; break;
                case 7: SCPCount = 2; DDCount = 3; FGCount = 1; DRCount = 1; break;
                case 8: SCPCount = 2; DDCount = 3; FGCount = 1; DRCount = 2; break;
                case 9: SCPCount = 2; DDCount = 4; FGCount = 1; DRCount = 2; break;
                case 10: SCPCount = 3; DDCount = 4; FGCount = 2; DRCount = 1; break;
                case 11: SCPCount = 3; DDCount = 5; FGCount = 2; DRCount = 1; break;
                case 12: SCPCount = 3; DDCount = 5; FGCount = 2; DRCount = 2; break;
                case 13: SCPCount = 3; DDCount = 6; FGCount = 2; DRCount = 2; break;
                case 14: SCPCount = 3; DDCount = 6; FGCount = 2; DRCount = 3; break;
                case 15: SCPCount = 3; DDCount = 6; FGCount = 3; DRCount = 3; break;
                case 16: SCPCount = 4; DDCount = 6; FGCount = 3; DRCount = 3; break;
                case 17: SCPCount = 4; DDCount = 7; FGCount = 3; DRCount = 3; break;
                case 18: SCPCount = 4; DDCount = 8; FGCount = 3; DRCount = 3; break;
                case 19: SCPCount = 4; DDCount = 9; FGCount = 3; DRCount = 3; break;
                case 20: SCPCount = 5; DDCount = 8; FGCount = 4; DRCount = 3; break;
                case 21: SCPCount = 5; DDCount = 9; FGCount = 4; DRCount = 3; break;
                case 22: SCPCount = 5; DDCount = 9; FGCount = 4; DRCount = 4; break;
                case 23: SCPCount = 5; DDCount = 10; FGCount = 4; DRCount = 4; break;
                case 24: SCPCount = 5; DDCount = 11; FGCount = 4; DRCount = 4; break;
                case 25: SCPCount = 6; DDCount = 11; FGCount = 4; DRCount = 5; break;
                case 26: SCPCount = 6; DDCount = 11; FGCount = 5; DRCount = 5; break;
                case 27: SCPCount = 6; DDCount = 11; FGCount = 5; DRCount = 5; break;
                case 28: SCPCount = 6; DDCount = 12; FGCount = 5; DRCount = 5; break;
                case 29: SCPCount = 6; DDCount = 12; FGCount = 6; DRCount = 5; break;
                case 30: SCPCount = 6; DDCount = 12; FGCount = 6; DRCount = 6; break;
                case 31: SCPCount = 6; DDCount = 13; FGCount = 6; DRCount = 6; break;
                case 32: SCPCount = 6; DDCount = 13; FGCount = 6; DRCount = 7; break;
                case 33: SCPCount = 6; DDCount = 14; FGCount = 6; DRCount = 7; break;
                case 34: SCPCount = 6; DDCount = 15; FGCount = 6; DRCount = 7; break;
                case 35: SCPCount = 6; DDCount = 16; FGCount = 6; DRCount = 7; break;
                case 36: SCPCount = 6; DDCount = 16; FGCount = 6; DRCount = 8; break;
                case 37: SCPCount = 7; DDCount = 16; FGCount = 6; DRCount = 8; break;
                case 38: SCPCount = 7; DDCount = 16; FGCount = 7; DRCount = 8; break;
                case 39: SCPCount = 7; DDCount = 17; FGCount = 7; DRCount = 8; break;
                case 40: SCPCount = 7; DDCount = 18; FGCount = 7; DRCount = 8; break;
                case 41: SCPCount = 8; DDCount = 18; FGCount = 7; DRCount = 8; break;
                case 42: SCPCount = 8; DDCount = 18; FGCount = 8; DRCount = 8; break;
                case 43: SCPCount = 8; DDCount = 19; FGCount = 8; DRCount = 8; break;
                case 44: SCPCount = 8; DDCount = 20; FGCount = 8; DRCount = 8; break;
                case 45: SCPCount = 8; DDCount = 20; FGCount = 8; DRCount = 9; break;
                case 46: SCPCount = 8; DDCount = 20; FGCount = 8; DRCount = 10; break;
                case 47: SCPCount = 8; DDCount = 20; FGCount = 9; DRCount = 10; break;
                case 48: SCPCount = 8; DDCount = 21; FGCount = 9; DRCount = 10; break;
                case 49: SCPCount = 8; DDCount = 22; FGCount = 9; DRCount = 10; break;
                case 50: SCPCount = 9; DDCount = 20; FGCount = 10; DRCount = 11; break;

            }
        }
        public static void Deal(Player player, RoleTypeId role)
        {
            if (role.GetSide() == Side.Scp)
            {
                SCPCount--;
            }
            else if (role == RoleTypeId.ClassD)
            {
                DDCount--;
            }
            else if (role == RoleTypeId.Scientist)
            {
                DRCount--;
            }
            else if (role == RoleTypeId.FacilityGuard)
            {
                FGCount--;
            }
            player.Role.Set(role,SpawnReason.RoundStart);

        }
        public static void Start()
        {
            GetCounts(Player.List.Count(x => !x.IsOverwatchEnabled && x.Role.Type == RoleTypeId.Tutorial));
            Log.Info($"当前待分配人数:{Player.List.Count(x => !x.IsOverwatchEnabled && x.Role.Type == RoleTypeId.Tutorial)}");
            Log.Info($"DD:{DDCount},DR:{DRCount},FG:{FGCount},SCP:{SCPCount}");
            Reset();
            foreach (var item in Player.List.Where(x => !x.IsOverwatchEnabled && x.IsTutorial))
            {
                item.Role.Set(RoleTypeId.Spectator);
                //添加
                foreach(var roles in queues)
                {
                    UpdateRequest(item.UserId, roles,false,true);
                }
                UpdateRequest(item.UserId, RoleTypeId.FacilityGuard, false, true);
                UpdateRequest(item.UserId, RoleTypeId.Scientist, false, true);
                UpdateRequest(item.UserId, RoleTypeId.ClassD, false, true);
            }
            while (queues.Count > 0)
            {
                var model = queues.Dequeue();
                Log.Info($"现在分配:{model.ToString()}");
                if (!Check(model))
                {
                    Log.Info("跳过了一个待分配的角色");
                    continue;
                }
                var target = Main.list.Where(x => x.role == model && Player.TryGet(x.userid,out var player) && !player.IsOverwatchEnabled && player.Role.Type == RoleTypeId.Spectator).OrderByDescending(x => x.luck).FirstOrDefault().userid;
                if (Player.TryGet(target, out var boy))
                {
                    Deal(boy, model);
                    Log.Info($"分配成功:{boy.Nickname}成为了{model},他的申请成功");
                }
            }
            Log.Info("SCP分配完毕");
            var relist = Main.list.Where(x => x.role == RoleTypeId.FacilityGuard);
            Log.Info($"保安:{relist.Count()}");
            var r = new List<Request>();

            foreach(var item in relist)
            {
                if(Player.TryGet(item.userid,out var player))
                {
                    if(!player.IsOverwatchEnabled && player.Role.Type == RoleTypeId.Spectator)
                    {
                        Log.Info($"待分配角色:{item.userid}");
                        r.Add(item);
                    }
                    else
                    {
                        Log.Info($"不满足分配角色条件/已分配:{item.userid}");
                    }
                }
            }
            r.OrderByDescending(x => x.luck);
            var deallist = r.Take(FGCount);
           
            Log.Info("list  :" + deallist.Count());
            if (deallist.Any())
            {
                foreach (var item in deallist)
                {
                    var pl = Player.Get(item.userid);
                    pl.Role.Set(RoleTypeId.FacilityGuard, SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }


            relist = Main.list.Where(x=>x.role == RoleTypeId.ClassD);
            Log.Info($"DD:{relist.Count()}");
            r = new List<Request>();
            foreach (var item in relist)
            {
                if (Player.TryGet(item.userid, out var player))
                {
                    if (!player.IsOverwatchEnabled && player.Role.Type == RoleTypeId.Spectator)
                    {
                        Log.Info($"待分配角色:{item.userid}");
                        r.Add(item);
                    }
                    else
                    {
                        Log.Info($"不满足分配角色条件/已分配:{item.userid}");
                    }
                }
            }
            r.OrderByDescending(x => x.luck);
            deallist = r.Take(DDCount);

            Log.Info("list  :" + deallist.Count());
            if (deallist.Any())
            {
                foreach (var item in deallist)
                {
                    var pl = Player.Get(item.userid);
                    pl.Role.Set(RoleTypeId.ClassD, SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }


            relist = Main.list.Where(x => x.role == RoleTypeId.Scientist);
            r = new List<Request>();
            foreach (var item in relist)
            {
                if (Player.TryGet(item.userid, out var player))
                {
                    if (!player.IsOverwatchEnabled && player.Role.Type == RoleTypeId.Spectator)
                    {
                        r.Add(item);
                    }
                }
            }
            r.OrderByDescending(x => x.luck);
            deallist = r.Take(DRCount);

            Log.Info("list  :" + deallist.Count());
            if (deallist.Any())
            {
                foreach (var item in deallist)
                {
                    var pl = Player.Get(item.userid);
                    pl.Role.Set(RoleTypeId.Scientist, SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }
            var list = Player.List.Where(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Spectator);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Role.Set(RoleTypeId.ClassD, SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }
            Log.Info("申请信息已被清理");
            Round.IsLocked = false;
            Log.Info("回合锁已被解除");

            Timing.CallDelayed(5f, () =>
            {
                var l = Player.List.Where(x => x.Role == RoleTypeId.Tutorial);
                if (l != null && l.Any())
                {
                    foreach (var item in l)
                    {
                        item.Role.Set(RoleTypeId.ClassD);
                        Log.Info($"补充角色{item.Nickname}已分配完成");
                    }
                }
            });
        }
        private static List<RoleTypeId> roles = new List<RoleTypeId>()
        {
            RoleTypeId.Scp079,
            RoleTypeId.Scp049,
            RoleTypeId.Scp106,
            RoleTypeId.Scp096,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
            RoleTypeId.Scp939
        };
        private static Queue<RoleTypeId> queues = new Queue<RoleTypeId>();
        public static void Reset()
        {
            #region 刷新queue
            queues.Clear();

            roles.ShuffleList();
            var newroles = new List<RoleTypeId>();
            foreach(var item in list)
            {
                if(item.ischoice && item.role.GetSide() == Side.Scp)
                {
                    newroles.Add(item.role);
                }
            }
            var restful = new List<RoleTypeId>();
            foreach(var restrole in roles)
            {
                if(!newroles.Contains(restrole) && restrole != RoleTypeId.Scp939)
                {
                    restful.Add(restrole);
                }
                else
                {
                    if(!newroles.Contains(restrole))
                    {
                        if(restrole == RoleTypeId.Scp939)
                        {
                            if(newroles.Count(x=>x == RoleTypeId.Scp939) + restful.Count(x => x == RoleTypeId.Scp939) + 1 <= 2 )
                            {
                                restful.Add(restrole);
                            }
                        }
                    }
                    else
                    {
                        //
                    }
                }
            }

            restful.ShuffleList();

            foreach(var it in restful.Take(SCPCount - newroles.Count))
            {
                newroles.Add(it);
            }


            foreach (var item in newroles)
            {
                if (SCPCount < 5)
                {
                    if (SCPCount < 2)
                    {
                        if (item != RoleTypeId.Scp079)
                        {
                            queues.Enqueue(item);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ((queues.Contains(RoleTypeId.Scp096) && item == RoleTypeId.Scp079) || (queues.Contains(RoleTypeId.Scp079) && item == RoleTypeId.Scp096))
                        {
                            //
                            continue;
                        }
                        else
                        {
                            queues.Enqueue(item);
                        }
                    }
                }
                else
                {
                    queues.Enqueue(item);
                }
            }
            #endregion
        }
        public static List<Request> list = new List<Request>();
        public static void UpdateRequest(string userid, RoleTypeId roleType,bool isforced = false,bool auto = true)
        {
            var request = new Request();
            if (list.Count(x => x.userid == userid && x.role == roleType) >= 1)
            {
                return;
            }
            else
            {
                request.userid = userid;
                request.role = roleType;
                if(!auto)
                {
                    list.RemoveAll(x => x.userid == userid);
                    request.ischoice = true;
                }
                request.luck = 5;
                request.luck += UnityEngine.Random.Range(0, 30);
                request.luck += (Player.Get(userid).GetScpPreference(roleType)) * 4;

                if(isforced)
                {
                    var model = guaranteeds.Find(x => x.Userid == Player.Get(userid).UserId);
                    request.luck += model.Rounds * 20;//五局一次保底 任何角色
                }

                if(request.luck >= 100)
                {
                    request.luck = 100;
                }

                Log.Info($"{request.userid}添加了一条记录:auto={auto},luck={request.luck},角色={request.role.ToString()}");

                list.Add(request);
            }
        }
        private static List<Guaranteed> guaranteeds = new List<Guaranteed>();
        public static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if(ev != null)
            {
              /*  var res = await Methods.Getguaranteed(ev.Player.UserId);
                if(res != null)
                {
                    if(res.Success)
                    {
                        guaranteeds.Add(res.Data);
                    }
                    else
                    {
                        if(res.Code == 404)
                        {
                            var model = new Guaranteed();
                            model.Userid = ev.Player.UserId;
                            guaranteeds.Add(model);
                        }
                        else
                        {
                            //无视 访问不了

                        }
                    }
                }*/
            }
        }
        public static void Onspawned(SpawnedEventArgs ev)
        {
            if(ev.Reason == SpawnReason.RoundStart)
            {
                /*if(list.Count(x=>x.ischoice && x.role == ev.Player.Role.Type && x.userid == ev.Player.UserId) >= 1)
                {
                    //申请成功了
                    var model = guaranteeds.Find(x => x.Userid == ev.Player.UserId);
                    model.Rounds = 0;
                    Ui.PlayerMain.Send(ev.Player, "<color=#32CD32>您的申请成功，您已成为对应角色</color>",5,Pos.正中偏下);
                    await Methods.Updateguaranteed(model);
                }
                else if(list.Count(x=>x.ischoice && x.role != ev.Player.Role.Type && x.userid == ev.Player.UserId)>=1)
                {
                    var model = guaranteeds.Find(x => x.Userid == ev.Player.UserId);
                    model.Rounds++;
                    Ui.PlayerMain.Send(ev.Player, $"<color=#32CD32>您的申请失败，您的下一次申请的概率提升{model.Rounds * 20}%</color>", 5, Pos.正中偏下);
                    await Methods.Updateguaranteed(model);
                }*/
            }
        }
        public static void ResetData()
        {
            guaranteeds.Clear();
            queues.Clear();
            list.Clear();
            coins.Clear();
            SpawnCoin(RoleTypeId.ClassD, new Vector3(24, 980.387f, -34.531f));
            SpawnCoin(RoleTypeId.FacilityGuard, new Vector3(21.949f, 980.387f, -37.371f));
            SpawnCoin(RoleTypeId.Scientist, new Vector3(22.011f, 980.387f, -41.954f));
            SpawnCoin(RoleTypeId.Scp173, new Vector3(31.012f, 980.387f, -34.629f));
            SpawnCoin(RoleTypeId.Scp939, new Vector3(33.055f, 980.387f, -37.410f));
            SpawnCoin(RoleTypeId.Scp049, new Vector3(30.113f, 980.387f, -38.105f));
            SpawnCoin(RoleTypeId.Scp079, new Vector3(30.160f, 980.387f, -41.465f));
            SpawnCoin(RoleTypeId.Scp096, new Vector3(33f, 980.387f, -41.898f));
            SpawnCoin(RoleTypeId.Scp106, new Vector3(31.070f, 980.387f, -44.574f));
        }
        private static Dictionary<ushort, RoleTypeId> coins = new Dictionary<ushort, RoleTypeId>();
        private static void SpawnCoin(RoleTypeId role,Vector3 postion)
        {
            var coin = Exiled.API.Features.Items.Item.Create(ItemType.Coin);
            coin.Scale = Vector3.one * 6f;
            var pickup = coin.CreatePickup(postion, default, true);
            coins.Add(pickup.Serial, role);
        }
        public static void OnPickup(PickingUpItemEventArgs ev)
        {
            if(coins.TryGetValue(ev.Pickup.Serial,out var roleType))
            {
                ev.IsAllowed = false;
                UpdateRequest(ev.Player.UserId,roleType,true,false);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Verified += OnJoined;
            Exiled.Events.Handlers.Player.Spawned += Onspawned;
            Exiled.Events.Handlers.Server.WaitingForPlayers += ResetData;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickup;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Verified -= OnJoined;
            Exiled.Events.Handlers.Player.Spawned -= Onspawned;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= ResetData;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickup;
        }
    }
}

