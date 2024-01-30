using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Fuctions
{
    internal class Playerlistinfo
    {
        private static string Listmsg = Main.Singleton.Config.Playerlistmsg;
        public static void Editlist()
        {
            PlayerList.Title.Value = Listmsg + "<size=30><color=#ffce64>回合已进行：" + Round.ElapsedTime.ToString(@"hh\:mm\:ss") + "</color></size>";
        }
    }
}
