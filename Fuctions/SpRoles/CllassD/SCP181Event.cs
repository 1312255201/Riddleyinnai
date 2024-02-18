using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.CllassD;

public class SCP181Event
{
    public static bool scp181jineng;
    private static Vector3 teslagatepos;

    public static void SpawnAScp181(Player player)
    {
        SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP181,"",Side.ChaosInsurgency,false);
        YYYApi.MyApi.SetNickName("SCP-181","",player);
        Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#FFCC66>[SCP-181]</color>\n<color=#FFFFCC>1.你有百分之一的概率</color><color=#FF3333>强制</color><color=#FFFFCC>开启需要卡才能开启的门</color>\n<color=#FFFFCC>2.你可以免疫低于5的伤害</color><color=#FF3333>1次</color><color=#FFFFCC>，对于其他伤害也有免疫效果</color>\n<color=#FFFFCC>3.你不会</color><color=#FF3333>触发电网</color>\n<color=#FFFFCC>",10,Pos.正中偏下,5);
        int t = new System.Random().Next(0, 11);
        player.AddItem((ItemType)(t == 9 ? 8: t));
    }

    private static void OnPlayerOpenDoor(InteractingDoorEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP181))
        {
            if (new System.Random().Next(1, 100) >= 90)
            {
                if (!ev.Door.IsLocked)
                {
                    ev.IsAllowed = true;
                }
            }
        }
    }

    private static void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP181))
        {
            ev.IsAllowed = false;
            teslagatepos = ev.Tesla.Position;
        }

        if (RoleManger.GetRole(RoleManger.RoleName.SCP181).Any())
        {
            var scp181 = Player.Get(RoleManger.GetRole(RoleManger.RoleName.SCP181).First().player);
            if (scp181 != null)
            {
                if (Vector3.Distance(scp181.Position, teslagatepos) <= 10)
                {
                    if (Vector3.Distance(ev.Tesla.Position, teslagatepos) <= 2)
                    {
                        ev.IsAllowed = false;
                    }
                }
            }
        }
    }
    private static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        scp181jineng = false;
        teslagatepos = Vector3.zero;
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
        Log.Debug("SCP-181注册");
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnPlayerOpenDoor;
        Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
    }
}