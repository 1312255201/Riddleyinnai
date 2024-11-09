using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using Scp914;

namespace Riddleyinnai.Misc
{
    internal class Info914
    {
        private static Scp914KnobSetting Scp914KnobSetting = Scp914KnobSetting.Rough;
        public static void Changing914(Exiled.Events.EventArgs.Scp914.ChangingKnobSettingEventArgs ev)
        {
            if(ev.Player != null)
            {
                Scp914KnobSetting = ev.KnobSetting;
                player = ev.Player;
            }
        }
        private static Player player = Server.Host;
        public static void OnStart914(ActivatingEventArgs ev)
        {
            if(ev.Player != null)
            {
                var display = "";
                switch(Scp914KnobSetting)
                {
                    case Scp914KnobSetting.Rough:
                        display = "<color=#228b22>超粗加工</color>"; break;
                    case Scp914KnobSetting.Coarse:
                        display = "<color=#32cd32>粗加工</color>"; break;
                    case Scp914KnobSetting.OneToOne:
                        display = "<color=#faff86>1:1加工</color>"; break;
                    case Scp914KnobSetting.Fine:
                        display = "<color=#00b7eb>精加工</color>"; break;
                    case Scp914KnobSetting.VeryFine:
                        display = "<color=#960018>超精加工</color>"; break;
                }
                foreach(var item in ev.Player.CurrentRoom.Players)
                {
                    Ui.PlayerMain.Send(item, $"<b><size=25>运行模式:{display}</b></size>",8,Ui.Pos.正中偏下,5);
                    if(player != Server.Host)
                    {
                        Ui.PlayerMain.Send(item, $"<b><size=25>调整者:{player.Nickname}</b></size>", 8, Ui.Pos.正中偏下, 5);
                    }
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Scp914.Activating += OnStart914;
            Exiled.Events.Handlers.Scp914.ChangingKnobSetting += Changing914;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Scp914.Activating -= OnStart914;
            Exiled.Events.Handlers.Scp914.ChangingKnobSetting -= Changing914;
        }
    }
}
