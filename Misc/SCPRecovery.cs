using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Riddleyinnai.Misc
{
    internal class SCPRecovery
    {
        public class RecoveryEntry
        {
            public string player { get; set; }
            public Vector3 pos { get; set; }
            public int sec { get; set; } = 0;
        }
        public static bool IsRecovery = true;

        public static int RecoveryHP = 2;

        public static int WaitSecs = 3;

        public static List<RecoveryEntry> recoverys = new List<RecoveryEntry>();
        public static void CheckSCP()
        {
            var list = scps;

            if (list != null && list.Count() > 0)
            {
                foreach (var p in list)
                {
                    if (recoverys.Count(s => s.player == p) == 0)
                    {
                        if (Player.TryGet(p, out var pl))
                        {
                            recoverys.Add(new RecoveryEntry() { player = p, pos = pl.Position, sec = 0 });
                        }
                    }
                    var rec = recoverys.FirstOrDefault(s => s.player == p);
                    if (rec != null && rec.player != null)
                    {
                        if (Player.TryGet(p, out var pl))
                        {
                            if (rec.pos == pl.Position)
                            {
                                rec.sec += 1;
                                if (rec.sec >= WaitSecs)
                                {
                                    Player.Get(rec.player).Heal(RecoveryHP);
                                }
                            }
                            else
                            {
                                //坐标不一致重新计时
                                rec.sec = 0;
                                rec.pos = pl.Position;
                            }
                        }
                    }
                }
            }
        }
        private static List<string> scps = new List<string>();
        public static void Onrolechanged(Exiled.Events.EventArgs.Player.ChangingRoleEventArgs ev)
        {
            if (scps.Contains(ev.Player.UserId))
            {
                if (ev.NewRole.GetSide() != Exiled.API.Enums.Side.Scp)
                {
                    scps.Remove(ev.Player.UserId);
                }
            }
            else
            {
                if (ev.NewRole.GetSide() == Exiled.API.Enums.Side.Scp)
                {
                    scps.Add(ev.Player.UserId);
                }
            }
        }
        public static void Reset()
        {
            scps.Clear();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.ChangingRole += Onrolechanged;
        }
        public static void Unregister()
        {

            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.ChangingRole -= Onrolechanged;
        }
    }
}
