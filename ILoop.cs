using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions;
using Riddleyinnai.Misc;
using Riddleyinnai.User;
using System;
using System.Collections.Generic;

namespace Riddleyinnai
{
    internal class ILoop
    {
        public static bool RoundStarted = false;

        public static bool AllRoundStarted = false;

        public static CoroutineHandle loop1;
        private static CoroutineHandle loop2;
        private static CoroutineHandle loop3;
        public static void Start()
        {
            RoundStarted = true;
            Timing.CallDelayed(0.1f, () =>
            {
                Loop();
            });
        }
        public static void Reset()
        {
            AllRoundStarted = true;
            Timing.CallDelayed(0.5f, () =>
            {
                _Loop();
            });
        }

        /// <summary>
        /// 0.5s一次
        /// </summary>
        public static void Loop()
        {
            if (RoundStarted)
            {
                try
                {
                    Timing.CallDelayed(0.5f, () =>
                    {
                        Loop();
                    });
                }
                catch (Exception ex)
                {
                    Log.Info(ex);
                }
            }
        }
        /// <summary>
        /// 0.5s一次 回合未开始时也可以
        /// </summary>
        public static void _Loop()
        {
            try
            {
                if (AllRoundStarted)
                {
                    //ExpSystem.CheckEXPQueue();
                    //ExpSystem.CheckJoinData();
                    Timing.CallDelayed(0.5f, () =>
                    {
                        _Loop();
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Info(ex);
            }
        }
        public static void WaitingPlayers()
        {
            loop1 = Timing.RunCoroutine(Loop1());
            loop3 = Timing.RunCoroutine(Loop3());
        }
        private static int time = 0;
        public static void RoundStart()
        {
            time = 0;
            loop2 = Timing.RunCoroutine(Loop2());
        }
        public static void EndLoop(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(loop1);
            Timing.KillCoroutines(loop2);
            Timing.KillCoroutines(loop3);
        }
        /// <summary>
        /// 0.5s一次 未开始时就循环
        /// </summary>
        /// <returns></returns>
        public static IEnumerator<float> Loop1()
        {
            while (!Round.IsEnded)
            {
                try { Ui.PlayerMain.Msg(); } catch { }
                //try { User.Badge.BadgeCheck(); } catch { }

                yield return Timing.WaitForSeconds(1f);
            }
            yield break;
        }
        public static IEnumerator<float> Loop3()
        {
            while (!Round.IsEnded)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Log.Info(ex.ToString());
                }
                yield return Timing.WaitForSeconds(1f);
            }
            yield break;
        }
        public static IEnumerator<float> Loop2()
        {
            while (!Round.IsEnded && Round.IsStarted)
            {
                time++;
                try { Playerlistinfo.Editlist(); } catch { }
                try { Fuctions.Broadcast.Sys_Broadcast(time); } catch { }
                /*try { Misc.Playerinfo.Info(); } catch { }*/
                //try { Moveboost.SCPMove(); } catch { }
                try { SCPRecovery.CheckSCP(); } catch { }
                try { Exp.Onescaping(); } catch { }
                yield return Timing.WaitForSeconds(1);
            }
            yield break;
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundEnded += EndLoop;
            Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += RoundStart;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= EndLoop;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStart;
        }
    }
}
