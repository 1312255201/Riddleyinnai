using System.Collections.Generic;

namespace Riddleyinnai.Fuctions
{
    internal class Broadcast
    {
        private static List<string> msgs = new List<string>()
        {
            "<size=30>欢迎来到<color=#00ffff>谜子音奈</color>服务器请仔细阅读规则<color=red>Server info</color></size>\n<size=30><color=#FF99CC>祝您游戏愉快~(,,・ω・,,)</color></size>"
        };

        //private static string msg = "<size=30>欢迎来到<color=#00ffff>谜子音奈</color>服务器请仔细阅读规则<color=red>Server info</color></size>\n<size=30><color=#FF99CC>祝您游戏愉快~(,,・ω・,,)</color></size>";

        public static void Sys_Broadcast(int time)
        {
            if(time % 180 == 0)
            {
                Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast(msgs.RandomItem(), 5), false);
            }
        }
    }
}
