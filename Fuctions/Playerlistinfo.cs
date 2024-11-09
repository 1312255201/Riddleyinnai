using Exiled.API.Features;

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
