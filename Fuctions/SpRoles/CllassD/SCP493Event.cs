using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events;
using Exiled.Events.EventArgs.Server;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.CllassD;

public class SCP493Event
{
    public static void SpawnASCP493(Player player)
    {
        RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP493,"",Side.ChaosInsurgency,false);
        Ui.PlayerMain.Send(player,"<color=#FFFFCC>你是:</color><color=#FFCC66>[SCP-493]</color>\n<color=#FFFFCC>1.复活尸体：当你盯着</color><color=#FF3333>一具尸体后</color><color=#FFFFCC>10秒钟，他将复活</color>",15,Pos.正中偏下,5);
        YYYApi.MyApi.SetNickName("SCP-493","orange",player);
        Timing.RunCoroutine(CheckTiming(player));
    }
    private static IEnumerator<float> CheckTiming(Player player)
    {
        yield return Timing.WaitForSeconds(5f);
        int time = 0;
        while (RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP493))
        {
            yield return Timing.WaitForSeconds(1f);
            bool flag = false;
            BasicRagdoll[] array = UnityEngine.Object.FindObjectsOfType<BasicRagdoll>();
            foreach (BasicRagdoll ragdoll in array)
            {
                try
                {
                    if (Vector3.Distance(ragdoll.transform.position, player.Position) <= 3  && player.CurrentRoom.Type != RoomType.Pocket)
                    {
                        time++;
                        flag = true;
                        player.ClearBroadcasts();
                        player.Broadcast(1, time.ToString());
                        if (time >= 10)
                        {
                            time = 0;
                            try
                            {
                                var getupplayer = Player.Get(ragdoll.Info.OwnerHub.PlayerId);
                                if (getupplayer != null)
                                {
                                    if (!getupplayer.IsAlive)
                                    {
                                        getupplayer.Role.Set(ragdoll.Info.RoleType, SpawnReason.ForceClass, RoleSpawnFlags.None);
                                        if (ragdoll.Info.RoleType.GetTeam() == Team.SCPs)
                                        {
                                            getupplayer.Health = 800;
                                            Timing.CallDelayed(1f, () =>
                                            {
                                                if (getupplayer.Health >= 800)
                                                {
                                                    getupplayer.Health = 800;
                                                    getupplayer.MaxHealth = 800;
                                                }
                                            });
                                        }
                                        getupplayer.Position = ragdoll.transform.position + Vector3.up * 1.3f;
                                        Ui.PlayerMain.Send(player,"<color=#0F0>[复活成功你成功复活了]</color>"+getupplayer.Nickname,5,Pos.正中偏下,5);
                                        
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                
                            }

                            NetworkServer.Destroy(ragdoll.gameObject);
                            Ui.PlayerMain.Send(player,"<color=#0F0>[复活失败]</color>尸体主人可能退出服务器或者当前已经复活",5,Pos.正中偏下,5);
                        }
                        break;
                    }
                }
                catch
                {
                    if(!player.IsConnected)
                    {
                        break;
                    }
                }
            }
            if (!flag)
            {
                time = 0;
            }
        }
    }
}