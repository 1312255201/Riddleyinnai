using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class MulHid
    {
        public class Kill
        {
            public DateTime Date { get; set; }
            public int Mul { get; set; }
        }
        private static Dictionary<string, Kill> Kills = new Dictionary<string, Kill>();
        public static void OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (ev.TargetOldRole.GetTeam() == PlayerRoles.Team.SCPs && ev.TargetOldRole != PlayerRoles.RoleTypeId.Scp0492)
                {
                    if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.MicroHid)
                    {
                        if (Kills.TryGetValue(ev.Attacker.UserId, out var kill))
                        {
                            if (DateTime.Now - kill.Date > TimeSpan.FromSeconds(5))
                            {
                                kill.Mul = 1;
                                kill.Date = DateTime.Now;
                                Kills[ev.Attacker.UserId] = kill;
                            }
                            else
                            {
                                kill.Mul++;
                                kill.Date = DateTime.Now;
                                Kills[ev.Attacker.UserId] = kill;
                            }
                        }
                        else
                        {
                            var kills = new Kill();
                            kills.Mul = 1;
                            kills.Date = DateTime.Now;
                            Kills.Add(ev.Attacker.UserId, kills);
                        }

                        if (Kills[ev.Attacker.UserId].Mul >= 3)
                        {
                            var ach = Main.achieves.Find(x => x.id == 13);
                            ach.status = true;
                            Main.UpdateAchievement(ev.Attacker.UserId, ach);
                        }
                    }
                }
            }
        }
        public static void Reset()
        {
            Kills.Clear();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= OnDied;


        }
    }
}
