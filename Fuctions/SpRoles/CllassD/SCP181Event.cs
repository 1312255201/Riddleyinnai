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
        YYYApi.MyApi.SetNickName("SCP-181","pink",player);
        Ui.PlayerMain.Send(player,"你是[SCP-181]有百分之一几率强制开启需要卡才能开启的门.\n免疫低于5的伤害，大于5的伤害，免疫1次，之后67%免疫\n不会触发电网，即使附近有其他玩家\n减少被SCP-939看到的时间，2.5秒降低到1秒]",5,Pos.正中偏下,5);
        player.AddItem((ItemType)new System.Random().Next(0, 12));
    }

    private static void OnPlayerOpenDoor(InteractingDoorEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP181))
        {
            if (new System.Random().Next(1, 100) >= 99)
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