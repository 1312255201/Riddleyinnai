using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using MEC;
using Riddleyinnai.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    internal class Omega
    {
        private static int Omegatime = 25 * 60;//秒
        private static bool Canescaping = false;
        private static CoroutineHandle cor;
        private static CoroutineHandle cor2;
        private static CoroutineHandle cor3;
        private static int CountDown = 30;
        public static IEnumerator<float> WarheadCountDown()
        {
            while (Round.InProgress && CountDown >= 0)
            {
                Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30><color=#f51000>Omega核弹</color>已经开启，引爆倒计时{CountDown}秒 请立刻搭乘飞机撤离！</size>"), true);
                CountDown--;
                
                if (CountDown == 15)
                {
                    Respawn.SummonNtfChopper();
                    Canescaping = true;
                }
                yield return Timing.WaitForSeconds(1f);
            }
            Canescaping = false;
            foreach (var item in Exiled.API.Features.Player.List)
            {
                item.Kill(Exiled.API.Enums.DamageType.Warhead);
            }
            Exiled.API.Features.Warhead.Detonate();
            yield break;
        }
        public static IEnumerator<float> OmegaWarhead()
        {
            Canescaping = false;
            CountDown = 30;
            yield return Timing.WaitForSeconds(Omegatime);

            cor = Timing.RunCoroutine(Checkescaping());

            cor3 = Timing.RunCoroutine(WarheadCountDown());

            yield break;
        }
        public static void Roundstart()
        {
          //  cor2 = Timing.RunCoroutine(OmegaWarhead());
        }
        public static IEnumerator<float> Checkescaping()
        {
            while(Round.InProgress)
            {
                if(Canescaping)
                {
                    foreach(var item in Exiled.API.Features.Player.List.Where(x=>x.IsNTF))
                    {
                        if(Vector3.Distance(item.Position,new Vector3(127.119f,995.456f,-42.959f)) <= 5)
                        {
                            item.Role.Set(PlayerRoles.RoleTypeId.Spectator,Exiled.API.Enums.SpawnReason.Escaped);
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
            yield break;
        }
        public static void Endinground(EndingRoundEventArgs ev)
        {
            if(ev.IsRoundEnded)
            {
                Timing.KillCoroutines(cor);
                Timing.KillCoroutines(cor2);
                Timing.KillCoroutines(cor3);
            }
        }
        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if(cor.IsRunning)
            {
                Timing.KillCoroutines(cor);
            }
            if(cor2.IsRunning)
            {
                Timing.KillCoroutines(cor2);
            }
            if(cor3.IsRunning)
            {
                Timing.KillCoroutines(cor3);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += Roundstart;
            Exiled.Events.Handlers.Server.EndingRound += Endinground;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= Roundstart;
            Exiled.Events.Handlers.Server.EndingRound -= Endinground;
        }
    }
}
