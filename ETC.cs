using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using Riddleyinnai.Fuctions.Items;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Fuctions.SpRoles;
using Riddleyinnai.Fuctions.SpRoles.CllassD;
using Riddleyinnai.Fuctions.SpRoles.Scientists;
using UnityEngine;

namespace Riddleyinnai
{
    internal class ETC
    {
        public static void Onhurting(HurtingEventArgs ev)
        {
            bool isscp2818 = false;
            if (ev.Attacker != null)
            {
                if (ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.ClassD && ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scientist)
                {
                    ev.IsAllowed = false;
                }
                else if (ev.Attacker.Role.Type == PlayerRoles.RoleTypeId.Scientist && ev.Player.Role.Type == PlayerRoles.RoleTypeId.ClassD)
                {
                    ev.IsAllowed = false;
                }
            }
            //伤害量处理
            if (ev.Attacker != null)
            {
                if (ev.Attacker.CurrentItem != null)
                {
                    if (ev.Amount != 0)
                    {
                        if (SCP2818.scp2818id.Contains(ev.Attacker.CurrentItem.Serial))
                        {
                            isscp2818 = true;
                            bool flag = false;
                            foreach (var item in ev.Attacker.Items)
                            {
                                if (SCP2818.scp2818aid.Contains(item.Serial))
                                {
                                    flag = true;
                                }
                            }
                            if (ev.Player.Role.Team == Team.SCPs)
                            {
                                ev.Amount = 1000;
                                if (flag)
                                {
                                    ev.Amount *= 3;
                                }
                                if (ev.Player.Role.Type == RoleTypeId.Scp106)
                                {
                                    ev.Amount *= 0.3f;
                                }
                            }
                            else
                            {
                                ev.Amount = 3500;
                                if (flag)
                                {
                                    ev.Amount *= 3;
                                }
                            }
                            var pickup = Pickup.CreateAndSpawn(ItemType.Medkit, ev.Attacker.Position, Quaternion.identity);
                            if (flag)
                            {
                                var grenade1 = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                grenade1.FuseTime = 0.3f;
                                grenade1.SpawnActive(ev.Attacker.Position, ev.Attacker);
                            }
                            pickup.Scale = Vector3.one / 2;
                            SCP2818.scp2818aid.Add(pickup.Serial);
                            ev.Attacker.Kill("使用SCP-2818而死");
                        }
                    }
                    if (ev.Amount != 0)
                    {
                        if (ev.Attacker.CurrentItem.Serial == 24908)
                        {
                            ev.Amount = 800;
                        }
                    }
                }
            }
            //抗性处理
            if (ev.Amount > 0)
            {
                if (SuperMedik.suppermeddikuseid.Contains(ev.Player.Id) && !isscp2818)
                {
                    ev.Amount = 1;
                }
            }
            else if (ev.Amount < 0)
            {
                if (ev.Attacker != null)
                {
                    if (SuperMedik.suppermeddikuseid.Contains(ev.Player.Id) && !isscp2818)
                    {
                        ev.Amount = 1;
                    }
                }
            }
            //免伤处理
            if (ev.Player != null)
            {
                if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP181))
                {
                    if (ev.Amount > 0 && ev.Amount <= 5)
                    {
                        ev.Amount = 0;
                    }
                    else if (ev.Amount > 5)
                    {
                        if (!SCP181Event.scp181jineng)
                        {
                            SCP181Event.scp181jineng = true;
                            ev.Amount = 0;
                        }
                        else
                        {
                            if (new System.Random().Next(1, 100) >= 33)
                            {
                                ev.Amount = 0;
                            }
                        }
                    }
                    else
                    {
                        if (ev.Attacker != null)
                        {
                            if (new System.Random().Next(1, 100) >= 33)
                            {
                                ev.Amount = 0;
                            }
                        }
                    }
                }
                if (ev.Amount > 0)
                {
                    if (SuperMedik.suppermeddikuseid.Contains(ev.Player.Id) && !isscp2818)
                    {
                        ev.Amount = 1;
                    }
                }
                else if (ev.Amount < 0)
                {
                    if (ev.Attacker != null)
                    {
                        if (SuperMedik.suppermeddikuseid.Contains(ev.Player.Id) && !isscp2818)
                        {
                            ev.Amount = 1;
                        }
                    }
                }
            }
        }
        public static void OnTeamspawn(RespawningTeamEventArgs ev)
        {
            Timing.CallDelayed(1f, () => { Respawning.RespawnTokensManager.ResetTokens(); });
        }
        public static void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
            {
                Timing.CallDelayed(1f, () =>
                {
                    int amount = (Player.List.Count(p => p.IsCHI) / 100) * 80;

                    RoundSummary.singleton.ChaosTargetCount = amount;
                });
            }
        }
        public static void Receiveeffect(ReceivingEffectEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                if(ev.Effect is CustomPlayerEffects.Flashed && ev.Player.Zone == Exiled.API.Enums.ZoneType.Surface)
                {
                    ev.IsAllowed = false;
                }
            }
        }
        public static List<T> RandomSort<T>(List<T> list)
        {
            var random = new System.Random();
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }

        private static void OnRoundStart()
        {
            
        }
        private static IEnumerator<float> RenWuFenPei()
        {
            int classd = 0;
            int scientist = 0;
            int g = 0;
            List<Player> players111 = new List<Player>();
            List<Player> awa;
            yield return Timing.WaitForSeconds(1f);
            foreach (var player in Player.List)
            {
                if (player.IsVerified && player.IsAlive)
                {
                    players111.Add(player);
                }
            }
            awa = RandomSort(players111);
            foreach (var player in awa)
            {
                if (player.Role.Type == RoleTypeId.ClassD)
                {
                    classd++;
                    switch (classd)
                    {
                        case 4:
                            SCP069Event.SpawnAScp069(player);
                            break;
                        case 5:
                            SCP035Event.SpawnAScp035(player);
                            break;
                        case 3:
                            SCP493Event.SpawnASCP493(player);
                            break;
                        case 1:
                            D9341Events.SpanwAD9341(player);
                            break;
                        case 2:
                            SCP181Event.SpawnAScp181(player);
                            break;
                    }
                }

                if (player.Role.Type == RoleTypeId.Scientist)
                {
                    scientist++;
                    switch (scientist)
                    {
                        case 1:
                            scpcn08.SpawnACn08(player);
                            break;
                    }
                }

                if (player.Role.Type == RoleTypeId.FacilityGuard)
                {
                    g++;
                    switch (g)
                    {
                        case 1:
                            SCP550Event.SpawnAScp550(player);
                            break;
                    }
                }
            }
            yield return Timing.WaitForSeconds(240);
            var ppp = YYYApi.MyApi.GetRamdomDeadPlayer();
            while ( ppp == null)
            {
                yield return Timing.WaitForSeconds(1f);
                ppp = YYYApi.MyApi.GetRamdomDeadPlayer();
            }
            SCP2490Event.SpawnAScp2490(ppp);
        }

        private static void OnPLayerDying(DyingEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP181))
                {
                    YYYApi.MyApi.ScpDeath("SCP 1 8 1",ev.DamageHandler.Type ,ev.Attacker.Nickname);
                }

                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCPCN08))
                {
                    ev.Player.Scale = Vector3.one;
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting += Onhurting;
            Exiled.Events.Handlers.Player.Dying += OnPLayerDying;
            Exiled.Events.Handlers.Server.RespawningTeam += OnTeamspawn;
            Exiled.Events.Handlers.Player.ReceivingEffect += Receiveeffect;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }
        public static void Unregister() {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting -= Onhurting;
            Exiled.Events.Handlers.Player.Dying -= OnPLayerDying;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnTeamspawn;
            Exiled.Events.Handlers.Player.ReceivingEffect -= Receiveeffect;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }
    }
}
