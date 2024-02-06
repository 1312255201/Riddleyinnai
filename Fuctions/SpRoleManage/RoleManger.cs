using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions.SpRoleManage
{
    public class RoleManger
    {
        public static List<RoleEntry> RolePlayers = new List<RoleEntry>();
        private static object obj = new object();
        private static object flag = new object();

        public static void Reset() => RolePlayers.Clear();

        public static int RoleCount() => RolePlayers.Count(x => !Player.Get(x.player).IsDead);

        public static int RoleCount(RoleName roleName) => RolePlayers.Count(x => x.RoleName == roleName && !Player.Get(x.player).IsDead);

        public static List<RoleEntry> GetRole(RoleName roleName) => RolePlayers.Where(s => s.RoleName == roleName).ToList();

        public static RoleName GetRole(int player) => RolePlayers.FirstOrDefault(s => s.player == player)!.RoleName;

        public static Side GetSide(int player) => IsRole(player) ? RolePlayers.FirstOrDefault(s => s.player == player)!.Side : Player.Get(player).Role.Side;

        public static void ChangeSide(int player, Side newside) => RolePlayers.FirstOrDefault(s => s.player == player)!.Side = newside;

        public static string GetRoleTips(int player) => RolePlayers.FirstOrDefault(s => s.player == player)?.Tips;

        public static void AddRole(
          int player,
          RoleName name,
          string tips,
          Side Side,
          bool IsTips = true)
        {
            if (IsRole(player, name))
                return;
            try
            {
                Player.Get(player).BadgeHidden = false;
            }
            catch
            {
            }
            RolePlayers.Add(new RoleEntry
            {
                player = Player.Get(player).Id,
                RoleName = name,
                Tips = tips,
                IsTips = IsTips,
                Side = Side
            });
        }
        public static void AddRole(
          Player player,
          RoleName name,
          string tips,
          Side Side,
          bool IsTips = true)
        {
            if (IsRole(player.Id, name))
                return;
            try
            {
                player.BadgeHidden = false;
            }
            catch
            {
            }
            RolePlayers.Add(new RoleEntry
            {
                player = player.Id,
                RoleName = name,
                Tips = tips,
                IsTips = IsTips,
                Side = Side
            });
        }
        public static void DelRolePlayer(int UserID)
        {
            if (!IsRole(UserID))
                return;
            List<RoleEntry> list = RolePlayers.Where(s => s.player == UserID).ToList();
            if (list != null && list.Any())
            {
                foreach (RoleEntry roleEntry in list)
                    RolePlayers.Remove(roleEntry);
                Player.Get(UserID).RankName = "";
            }
        }

        public static void DelAllRolePlayers(RoleName roleName)
        {
            if (!IsRole(roleName))
                return;
            IEnumerable<RoleEntry> source = RolePlayers.Where(s => s.RoleName == roleName);
            if (source != null && source.Any())
            {
                foreach (RoleEntry roleEntry in source)
                    RolePlayers.RemoveAt(RolePlayers.IndexOf(roleEntry));
            }
        }

        public static bool IsRoleTips(int UserId, bool IsTips = true) => RolePlayers.Count(p => p.player == UserId && p.IsTips == IsTips) > 0;

        public static bool IsRole(int UserId)
        {
            bool lockTaken = false;
            Monitor.TryEnter(flag, 500, ref lockTaken);
            if (!lockTaken)
                return false;
            try
            {
                return RolePlayers.Count(p => p.player == UserId) > 0;
            }
            finally
            {
                Monitor.Exit(flag);
            }
        }

        public static bool IsRole(int UserId, RoleName name)
        {
            try
            {
                return IsRole(UserId, new List<RoleName>
        {
          name
        });
            }
            catch
            {
                return false;
            }
        }

        public static bool IsRole(int UserId, List<RoleName> list)
        {
            try
            {
                return RolePlayers.Count(p => list.Contains(p.RoleName) && p.player == UserId) > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsRole(RoleName name) => RolePlayers.Count(p => p.RoleName == name) > 0;

        public static void DiesClear(DiedEventArgs ev)
        {
            Timing.CallDelayed(0.03f, () =>
            {
                try
                {
                    if (ev.Player.IsConnected)
                    {
                        if (!IsRole(ev.Player.Id) || ev.Player == null)
                            return;
                        try
                        {
                            DelRolePlayer(ev.Player.Id);
                            Scp173Role.TurnedPlayers.Remove(ev.Player);
                            Scp096Role.TurnedPlayers.Remove(ev.Player);
                            ev.Player.Scale = UnityEngine.Vector3.one;
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {

                }
            });

        }

        public static void DisConnecting(DestroyingEventArgs ev)
        {
            Log.Info("我被调用了");
            if (!IsRole(ev.Player.Id))
                return;
            DelRolePlayer(ev.Player.Id);
        }

        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += DiesClear;
            Exiled.Events.Handlers.Player.Destroying += DisConnecting;
        }

        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= DiesClear;
            Exiled.Events.Handlers.Player.Destroying -= DisConnecting;
        }

        public enum RoleName
        {
D9341,SCP181,SCPCN08,SCP069,SCP493,SCP2490,SCP1143,SCP550,九尾狐医疗兵,九尾狐狙击手,SCP035,SCP999

        }

        public class RoleEntry
        {
            public int player { get; set; }

            public RoleName RoleName { get; set; }

            public string Tips { get; set; }

            public bool IsTips { get; set; } = true;

            public Side Side { get; set; } = Side.None;
        }
    }
}