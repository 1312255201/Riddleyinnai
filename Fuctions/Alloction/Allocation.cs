using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions.Alloction
{
    public class Request
    {
        public string Userid { get; set; } = string.Empty;
        public RoleTypeId Roles { get; set; } = RoleTypeId.None;

        public int luck = 5;
    }
    public class Allocation
    {
        private static int SCPCount = 0;
        private static int DDCount = 0;
        private static int DRCount = 0;
        private static int FGCount = 0;

        public static Dictionary<RoleTypeId, List<Request>> Requests = new Dictionary<RoleTypeId, List<Request>>();

        private static Queue<RoleTypeId> queues = new Queue<RoleTypeId>();

        public static void Deal(Player player, RoleTypeId role)
        {
            if (Exiled.API.Extensions.RoleExtensions.GetTeam(role) == Team.SCPs)
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
            player.Role.Set(role,Exiled.API.Enums.SpawnReason.RoundStart);

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
                case 10: SCPCount = 2; DDCount = 4; FGCount = 2; DRCount = 2; break;
                case 11: SCPCount = 2; DDCount = 5; FGCount = 2; DRCount = 2; break;
                case 12: SCPCount = 2; DDCount = 5; FGCount = 2; DRCount = 3; break;
                case 13: SCPCount = 2; DDCount = 6; FGCount = 2; DRCount = 3; break;
                case 14: SCPCount = 3; DDCount = 6; FGCount = 2; DRCount = 3; break;
                case 15: SCPCount = 3; DDCount = 6; FGCount = 3; DRCount = 3; break;
                case 16: SCPCount = 3; DDCount = 7; FGCount = 3; DRCount = 3; break;
                case 17: SCPCount = 3; DDCount = 7; FGCount = 3; DRCount = 4; break;
                case 18: SCPCount = 3; DDCount = 8; FGCount = 3; DRCount = 4; break;
                case 19: SCPCount = 3; DDCount = 9; FGCount = 3; DRCount = 4; break;
                case 20: SCPCount = 4; DDCount = 8; FGCount = 4; DRCount = 4; break;
                case 21: SCPCount = 4; DDCount = 9; FGCount = 4; DRCount = 4; break;
                case 22: SCPCount = 4; DDCount = 9; FGCount = 4; DRCount = 5; break;
                case 23: SCPCount = 4; DDCount = 10; FGCount = 4; DRCount = 5; break;
                case 24: SCPCount = 4; DDCount = 11; FGCount = 4; DRCount = 5; break;
                case 25: SCPCount = 4; DDCount = 12; FGCount = 4; DRCount = 5; break;
                case 26: SCPCount = 4; DDCount = 12; FGCount = 5; DRCount = 5; break;
                case 27: SCPCount = 4; DDCount = 12; FGCount = 5; DRCount = 6; break;
                case 28: SCPCount = 4; DDCount = 13; FGCount = 5; DRCount = 6; break;
                case 29: SCPCount = 4; DDCount = 13; FGCount = 6; DRCount = 6; break;
                case 30: SCPCount = 5; DDCount = 13; FGCount = 6; DRCount = 6; break;
                case 31: SCPCount = 5; DDCount = 13; FGCount = 6; DRCount = 7; break;
                case 32: SCPCount = 5; DDCount = 14; FGCount = 6; DRCount = 7; break;
                case 33: SCPCount = 5; DDCount = 15; FGCount = 6; DRCount = 7; break;
                case 34: SCPCount = 5; DDCount = 15; FGCount = 7; DRCount = 7; break;
                case 35: SCPCount = 5; DDCount = 16; FGCount = 7; DRCount = 7; break;
                case 36: SCPCount = 5; DDCount = 16; FGCount = 7; DRCount = 8; break;
                case 37: SCPCount = 5; DDCount = 16; FGCount = 8; DRCount = 8; break;
                case 38: SCPCount = 5; DDCount = 16; FGCount = 9; DRCount = 8; break;
                case 39: SCPCount = 6; DDCount = 17; FGCount = 8; DRCount = 8; break;
                case 40: SCPCount = 6; DDCount = 18; FGCount = 8; DRCount = 8; break;
                case 41: SCPCount = 6; DDCount = 18; FGCount = 8; DRCount = 9; break;
                case 42: SCPCount = 6; DDCount = 18; FGCount = 9; DRCount = 9; break;
                case 43: SCPCount = 7; DDCount = 18; FGCount = 9; DRCount = 9; break;
                case 44: SCPCount = 7; DDCount = 19; FGCount = 9; DRCount = 9; break;
                case 45: SCPCount = 7; DDCount = 19; FGCount = 9; DRCount = 10; break;
                case 46: SCPCount = 7; DDCount = 20; FGCount = 9; DRCount = 10; break;
            }
        }
        public static bool Check(RoleTypeId role)
        {
            if (Exiled.API.Extensions.RoleExtensions.GetTeam(role) == Team.SCPs)
            {
                return SCPCount > 0;
            }
            else if (role == RoleTypeId.ClassD)
            {
                return DDCount > 0;
            }
            else if (role == RoleTypeId.Scientist)
            {
                return DRCount > 0;
            }
            else if (role == RoleTypeId.FacilityGuard)
            {
                return FGCount > 0;
            }
            return true;
        }
        public static void Start()
        {
            GetCounts(Player.List.Count(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Tutorial));
            Log.Info($"当前待分配人数:{Player.List.Count(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Tutorial)}");
            Reset();
            foreach (var item in Player.List.Where(x => !x.IsOverwatchEnabled))
            {
                item.Role.Set(RoleTypeId.Spectator);
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
                if (Requests.TryGetValue(model, out List<Request> requests))
                {
                    var target = requests.OrderByDescending(x => x.luck).FirstOrDefault().Userid;
                    if (Player.TryGet(target, out var boy))
                    {
                        Deal(boy, model);
                        Log.Info($"分配成功:{boy.Nickname}成为了{model},他的申请成功");
                    }
                }
                else
                {
                    Player tar = null;
                    try
                    {
                        var listhere = Player.List.Where(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Spectator);
                        if (listhere.Any())
                        {
                            tar = listhere.ToList().RandomItem();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info(ex.StackTrace);
                    }
                    if (tar != null)
                    {
                        Deal(tar, model);
                        Log.Info($"分配成功:{tar.Nickname}成为了{model},他由系统随机");
                    }
                }
            }
            Log.Info("特殊角色分配完毕");
            var list = Player.List.Where(x => x.Role != RoleTypeId.Overwatch);
            try
            {
                list = Player.List.Where(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Spectator).Take(FGCount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Role.Set(RoleTypeId.FacilityGuard, reason: Exiled.API.Enums.SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }
            list = Player.List.Where(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Spectator).Take(DRCount);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Role.Set(RoleTypeId.Scientist, reason: Exiled.API.Enums.SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }
            list = Player.List.Where(x => !x.IsOverwatchEnabled && x.Role == RoleTypeId.Spectator);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Role.Set(RoleTypeId.ClassD, reason: Exiled.API.Enums.SpawnReason.RoundStart);
                }
            }
            else
            {
                Log.Info("没有可待分配的角色了");
            }
            Requests.Clear();
            Log.Info("申请信息已被清理");
            var classds = Player.List.Where(x=>x.Role.Type == RoleTypeId.Scp3114);
            if(classds.Any())
            {
                var target = classds.GetRandomValue();
                target.Role.Set(RoleTypeId.Scp3114);
            }
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

        public static void Reset()
        {
            #region 刷新queue
            queues.Clear();
            roles.ShuffleList();
            foreach (var item in roles)
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
    }
}
