using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Ui;
using UnityEngine;

namespace Riddleyinnai.Fuctions.SpRoles.MTFRoles;

public class SCP1143
{
    public static int scp1143num;
    public static bool scp1143cd;
    public static void SpawnAScp1143(Player player)
    {
        if (!SpRoleManage.RoleManger.GetRole(RoleManger.RoleName.SCP1143).Any())
        {
            SpRoleManage.RoleManger.AddRole(player.Id,RoleManger.RoleName.SCP1143,"",Side.Mtf,false);
            YYYApi.MyApi.SetNickName("SCP-1143","cyan",player);
            Ui.PlayerMain.Send(player,"你是<color=#0F0>[SCP-1143]</color>每隔一段30s你的背包内会刷新手雷/闪光弹 通过炸死其他人来获得升级",5,Pos.正中偏下,5);
            scp1143num = 0;
            scp1143cd = false;
            Timing.RunCoroutine(CheckTiming(player));
        }
    }

    private static IEnumerator<float> CheckTiming(Player player)
    {
        yield return Timing.WaitForSeconds(5f);
        int time = 0;
        while (RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP1143))
        {
            yield return Timing.WaitForSeconds(1f);
            time++;
            if (time >= 30)
            {
                time = 0;
                if (scp1143num == 0)
                {
                    if (new System.Random().Next(1, 100) >= 40)
                    {
                        player.AddItem(ItemType.GrenadeHE);
                    }
                    else
                    {
                        player.AddItem(ItemType.GrenadeFlash);
                    }
                }

                if (scp1143num == 1)
                {
                    int awa = new System.Random().Next(1, 100);
                    if (awa <= 10)
                    {
                        player.AddItem(ItemType.SCP018);
                    }
                    else if(awa <= 70)
                    {
                        player.AddItem(ItemType.GrenadeHE);
                    }
                    else
                    {
                        player.AddItem(ItemType.GrenadeFlash);
                    }
                }

            }
            if(scp1143num >= 4)
            {
                foreach (Player tPlayer in Player.List)
                {
                    if (player.Role.Team == Team.SCPs)
                    {
                        if (Vector3.Distance(tPlayer.Position, player.Position) <= 3)
                        {
                            if (!scp1143cd)
                            {
                                scp1143cd = true;
                                var grenade1 = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                grenade1.FuseTime = 0.3f;
                                grenade1.SpawnActive(player.Position, player);
                                Timing.CallDelayed(10f, () =>
                                {
                                    scp1143cd = false;
                                });
                            }
                        }
                    }
                }

            }
        }
    }

    public static void OnPlayerDead(DiedEventArgs ev)
    {
        if (ev.Attacker != null)
        {
            if (RoleManger.IsRole(ev.Attacker.Id,RoleManger.RoleName.SCP1143))
            {
                scp1143num++;
                if (scp1143num == 1)
                {
                    Ui.PlayerMain.Send(ev.Attacker,"你已升级到<color=#0F0>[2级]</color>有概率获得弹力球",5,Pos.正中偏下,5);
                }
                if (scp1143num == 4)
                {
                    Ui.PlayerMain.Send(ev.Attacker,"你已升级到<color=#0F0>[3级]</color>当SCP靠近你的时候会产生一个手榴弹在脚下",5,Pos.正中偏下,5);
                }
            }
        }
        string killname = ev.Attacker != null ? ev.Attacker.Nickname : "被服务器日了";
        if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP1143))
        {
            YYYApi.MyApi.ScpDeath("SCP 1 1 4 3",ev.DamageHandler.Type,killname);
            YYYApi.MyApi.SetNickName("","",ev.Player);
        }
    }

    public static void OnRoundEnded(RoundEndedEventArgs ev)
    {
        scp1143cd = false;
        scp1143num = 0;
    }
    public static void Register()
    {
        Exiled.Events.Handlers.Player.Died += OnPlayerDead;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
    }
    public static void UnRegister()
    {
        Exiled.Events.Handlers.Player.Died -= OnPlayerDead;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
    }
}