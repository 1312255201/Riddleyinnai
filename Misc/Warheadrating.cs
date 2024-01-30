using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;

namespace Riddleyinnai.Misc
{
    internal class Warheadrating
    {
        private class Status
        {
            public string userid { get; set; }
            public float time { get; set; }
        }

        private static IEnumerator<float> Loop()
        {
            while (!Round.IsEnded)
            {
                foreach (var player in Player.List)
                {
                    if (player.IsAlive)
                    {
                        var st = new Status();
                        if (status.Count(x => x.userid == player.UserId) >= 1)
                        {
                            st = status.Find(x => x.userid == player.UserId);
                        }
                        else
                        {
                            st = new Status() { time = 0, userid = player.UserId };
                            status.Add(st);
                        }
                        if (player.CurrentRoom == Door.Get(Exiled.API.Enums.DoorType.NukeArmory).Room)
                        {
                            
                            status.Find(x => x.userid == player.UserId).time++;
                            if (status.Find(x => x.userid == player.UserId).time >= 20)
                            {
                                status.Find(x => x.userid == player.UserId).time = 20;
                            }
                            Log.Info($"{player.Nickname}在核弹室!时间:{status.Find(x => x.userid == player.UserId).time}");
                        }
                        else
                        {
                            status.Find(x => x.userid == player.UserId).time--;
                            if (status.Find(x => x.userid == player.UserId).time < 0)
                            {
                                status.Find(x => x.userid == player.UserId).time = 0;
                            }
                        }
                        if(status.Find(x => x.userid == player.UserId).time >= 20)
                        {
                            if (player.IsScp)
                            {
                                player.ReferenceHub.playerStats.DealDamage(new UniversalDamageHandler(5,DeathTranslations.Warhead));
                            }
                            else
                            {
                                player.ReferenceHub.playerStats.DealDamage(new UniversalDamageHandler(1, DeathTranslations.Warhead));
                            }

                        }
                    }
                    else
                    {
                        status.Find(x => x.userid == player.UserId).time--;
                        if (status.Find(x => x.userid == player.UserId).time < 0)
                        {
                            status.Find(x => x.userid == player.UserId).time = 0;
                        }
                    }
                }

                yield return Timing.WaitForSeconds(1);
            }
            yield break;
        }
        public static void Roundstart()
        {
            status.Clear();
            cor = Timing.RunCoroutine(Loop());
        }
        private static List<Status> status = new List<Status>();
        private static CoroutineHandle cor;

        public static void Roundend(EndingRoundEventArgs ev)
        {
            if (cor.IsRunning && ev.IsRoundEnded)
            { 
                Timing.KillCoroutines(cor);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += Roundstart;
            Exiled.Events.Handlers.Server.EndingRound += Roundend;

        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= Roundstart;
            Exiled.Events.Handlers.Server.EndingRound -= Roundend;
        }
    }
}
