﻿using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using System.Collections.Generic;

namespace Riddleyinnai.Fuctions
{
    internal class SysWarhead
    {
        private static int Warheadtime = 20 * 60;//秒

        private static CoroutineHandle cor;
        public static IEnumerator<float> Sys_Warhead()
        {
            yield return Timing.WaitForSeconds(Warheadtime);

            Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30><color=red>系统</color>核弹已启动 无法关闭，请迅速移动到地表区域!</size>", 5), true);
            Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30><color=red>系统</color>核弹已启动 无法关闭，请迅速移动到地表区域!</size>", 5), true);
            Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast($"<size=30><color=red>系统</color>核弹已启动 无法关闭，请迅速移动到地表区域!</size>", 5), true);
            Warhead.IsKeycardActivated = true;

            Warhead.DetonationTimer = 90f;
            Warhead.Start();
            Warhead.IsLocked = true;

            yield break;
        }
        public static void Roundstart()
        {
           // cor = Timing.RunCoroutine(Sys_Warhead());
        }
        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (cor.IsRunning)
            {
                Timing.KillCoroutines(cor);
            }   
            Timing.KillCoroutines(cor);
        }
        public static void Register()
        {
            Warheadtime = Main.Singleton.Config.Syswarhead;

            Exiled.Events.Handlers.Server.RoundStarted += Roundstart;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= Roundstart;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }
    }
}
