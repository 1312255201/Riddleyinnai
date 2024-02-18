using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using Riddleyinnai.Fuctions.SpRoleManage;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.CllassD;

public class SCP069Event
{
    public static void SpawnAScp069(Player player)
    {
        RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP069,"",Side.ChaosInsurgency,false);
        YYYApi.MyApi.SetNickName("SCP-069","",player);
        Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#FFCC66>[SCP-069]</color>\n<color=#FFFFCC>1.你有在</color><color=#FF3333>死亡后</color><color=#FFFFCC>随机复活在附近的尸体上。</color>",10);
    }

    public static void OnPlayerDying(DyingEventArgs ev)
    {
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP069))
        {
            ev.Player.RankName = "";
            BasicRagdoll[] array = UnityEngine.Object.FindObjectsOfType<BasicRagdoll>();
            float distance = float.MaxValue-1;
            BasicRagdoll tmp = null;
            foreach (BasicRagdoll ragdoll in array)
            {
                try
                {
                    if (Vector3.Distance(ev.Player.Position, ragdoll.transform.position) < distance && Room.Get(ragdoll.transform.position).Type != RoomType.Pocket && ragdoll.Info.RoleType != RoleTypeId.Tutorial)
                    {
                        distance = Vector3.Distance(ev.Player.Position, ragdoll.transform.position);
                        tmp = ragdoll;
                    }
                }
                catch
                {
                    //ignore
                }
            }
            RoleTypeId ttt = ev.Player.Role.Type;
            Vector3 pos = ev.Player.Position;
            Timing.CallDelayed(1f, () =>
            {
                RoleManger.DelRolePlayer(ev.Player.Id);
                if (tmp != null)
                {
                    try
                    {
                        ev.Player.DropItems();
                    }
                    catch
                    {
                        //ignore
                    }
                    if (ev.Player.Role.Type != tmp.Info.RoleType)
                    {
                        ev.Player.Role.Set(tmp.Info.RoleType,SpawnReason.Respawn,RoleSpawnFlags.None);
                    }
                    ev.Player.Health = 100;
                    ev.Player.DisableAllEffects();
                    ev.Player.Position = tmp.transform.position +Vector3.up*1.3f;
                }
                else
                {
                    ev.Player.Role.Set(ttt);
                    if(ev.Player.CurrentRoom.Type != RoomType.Pocket)
                    {
                        ev.Player.Position = pos;
                        ev.Player.Health = 100;
                        ev.Player.DisableAllEffects();
                        ev.Player.DropItems();
                    }
                    else
                    {
                        ev.Player.Role.Set(ttt);
                        try
                        {
                            ev.Player.DropItems();
                        }
                        catch
                        {
                            //ignore
                        }
                        ev.Player.Health = 100;
                        ev.Player.DisableAllEffects();
                        ev.Player.Position = ev.Player.Role.Type.GetRandomSpawnLocation().Position + Vector3.up;
                    }
                }
            });

        }
    }

    public static void Register()
    {
        Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
        Log.Debug("SCP069注册");
    }

    public static void UnRegister()
    {
        Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
    }
}