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
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using PlayerStatsSystem;
using Riddleyinnai.Fuctions.Items;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Fuctions.SpRoles;
using Riddleyinnai.Fuctions.SpRoles.ChaosRoles;
using Riddleyinnai.Fuctions.SpRoles.CllassD;
using Riddleyinnai.Fuctions.SpRoles.MTFRoles;
using Riddleyinnai.Fuctions.SpRoles.Scientists;
using Riddleyinnai.Fuctions.SpRoles.SCPRoles;
using Riddleyinnai.Fuctions.SpRoles.Tutroles;
using UnityEngine;
using FirearmDamageHandler = PlayerStatsSystem.FirearmDamageHandler;

namespace Riddleyinnai
{
    internal class ETC
    {
        public static Riddleyinnai.YinNaiConfig AwaYinNaiConfig;
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

                if (ev.Attacker.CurrentItem != null && LieSheDan.items.Contains(ev.Attacker.CurrentItem.Serial))
                {
                    if (ev.Amount != 0)
                    {
                        ev.Amount *= 0.85f * LieSheDan.lastuse;
                    }
                }
                if (ev.Attacker.CurrentItem != null && JuJiQiang.items.Contains(ev.Attacker.CurrentItem.Serial))
                {
                    if (ev.Amount != 0)
                    {
                        ev.Amount *= 10;
                    }
                }
                if (ev.Attacker.CurrentItem != null)
                {
                    if (ev.Amount != 0)
                    {
                        if (SCP2818.scp2818id.Contains(ev.Attacker.CurrentItem.Serial))
                        {
                            isscp2818 = true;
                            bool flag = false;
                            Item item2 = null;
                            foreach (var item in ev.Attacker.Items)
                            {
                                if (SCP2818.scp2818aid.Contains(item.Serial))
                                {
                                    flag = true;
                                    item2 = item;
                                }
                            }

                            if (item2 != null)
                            {
                                ev.Attacker.RemoveItem(item2);
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
                            if (flag)
                            {
                                var grenade1 = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                grenade1.FuseTime = 0.3f;
                                grenade1.SpawnActive(ev.Attacker.Position, ev.Attacker);
                            }
                            if (!flag)
                            {
                                var pickup = Pickup.CreateAndSpawn(ItemType.Medkit, ev.Attacker.Position, Quaternion.identity);
                                pickup.Scale = Vector3.one / 2;
                                SCP2818.scp2818aid.Add(pickup.Serial);
                            }
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
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP550))
                {
                    ev.Amount *= 0.5f;
                }
            }

            if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP999))
            {
                if (ev.Attacker != null)
                {
                    if (ev.Amount < -0.5f || ev.Amount >= 400)
                    {
                        ev.Amount = 400;
                    }
                }
            }
            if (ev.Attacker != null)
            {
                if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP035))
                {
                    ev.Amount *= 0.33f;
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
            //免伤处理
            if (ev.Player != null)
            {
                if (ev.Attacker != null)
                {
                    if (RoleManger.GetSide(ev.Player.Id) == Side.Scp)
                    {
                        if (RoleManger.GetSide(ev.Attacker.Id) == Side.Scp)
                        {
                            ev.Amount = 0;
                        }
                    }    
                }
                
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP2490))
                {
                    if (ev.DamageHandler.Type == DamageType.Scp207)
                    {
                        ev.Amount = 0;
                    }
                }
                if (RoleManger.IsRole(ev.Player.Id,RoleManger.RoleName.SCP181))
                {
                    if (ev.DamageHandler.Type != DamageType.Scp207)
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
                                if (new System.Random().Next(1, 100) >= 75)
                                {
                                    ev.Amount = 0;
                                }
                            }
                        }
                        else
                        {
                            if (ev.Attacker != null)
                            {
                                if (new System.Random().Next(1, 100) >= 75)
                                {
                                    ev.Amount = 0;
                                }
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
            //特殊技能
            if (ev.Attacker != null)
            {
                if (RoleManger.IsRole(ev.Attacker.Id, RoleManger.RoleName.SCP550))
                {
                    if (ev.Amount > 0)
                    {
                        ev.Attacker.Health +=
                            ((ev.Amount * 0.25f) * (1f - ((ev.Attacker.Health) / (ev.Attacker.Health + 80))));
                        ev.Attacker.ArtificialHealth += (ev.Amount * 0.15f);
                    }
                }
            }
            if (ev.Attacker != null)
            {
                if (RoleManger.IsRole(ev.Attacker.Id, RoleManger.RoleName.SCP999))
                {
                    ev.Amount = 0;
                    ev.Player.Heal(new System.Random().Next(1,5));
                }
            }
            if (ev.Amount <= -0.5f)
            {
                ev.Amount = -1;
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
            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
            {
                Timing.RunCoroutine(SetNineFox2(ev.Players));
            }

            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
            {
                
            }
        }
        
        private static IEnumerator<float> SetNineFox2(List<Player> players)
        {
            yield return Timing.WaitForSeconds(0.1f);
            List<Player> players2 = new List<Player>();
            int tiems = 0;
            foreach (Player player1 in players)
            {
                players2.Add(player1);
            }
            List<Player> players1 = RandomSort(players2);
            foreach (var variPlayer in players1)
            {
                if (variPlayer.Role.Type == RoleTypeId.NtfPrivate)
                {
                    tiems++;
                    if (tiems == 1)
                    {
                        SCP1143.SpawnAScp1143(variPlayer);
                    }

                    if (tiems == 2)
                    {
                        NTFHealth.SpawnAHealther(variPlayer);
                    }

                    if (tiems == 3)
                    {
                        NTFsniper.SpawnASniper(variPlayer);
                    }
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
            Timing.RunCoroutine(CheckTiming());
        }
                private static IEnumerator<float> CheckTiming()
        {
            yield return Timing.WaitForSeconds(5f);
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                foreach (var player in Player.List)
                {
                    if (!player.IsAlive) continue;
                    if ((!(player.Position.x <= 150)) || (!(player.Position.x >= 115)) ||
                        ((!(player.Position.y <= 1000)) || (!(player.Position.y >= 980))) ||
                        (!(player.Position.z <= 32)) ||
                        (!(player.Position.z >= 10))) continue;

                    if (!SCP035Event.scp035runaway)
                    {
                        if (RoleManger.IsRole(player.Id,RoleManger.RoleName.SCP035))
                        {
                            SCP035Event.scp035runaway = true;
                            if (SCP035Event.checktiming.IsRunning)
                            {
                                Timing.KillCoroutines(SCP035Event.checktiming);
                            }
                            SCP035Event.checktiming = Timing.RunCoroutine(SCP035Event.CheckTiming(player, RoleTypeId.NtfSergeant));
                            player.ChangeAppearance(RoleTypeId.NtfSergeant);
                            player.Position = RoleTypeId.NtfCaptain.GetRandomSpawnLocation().Position + Vector3.up;
                        }
                    }
                }
            }
        }
        public static IEnumerator<float> RenWuFenPei()
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
                            if (Player.List.Count() >= AwaYinNaiConfig.scp069minnum)
                            {
                                SCP069Event.SpawnAScp069(player);
                            }
                            break;
                        case 5:
                            SCP035Event.SpawnAScp035(player);
                            break;
                        case 3:
                            if (Player.List.Count() >= AwaYinNaiConfig.scp493minnum)
                            {
                                SCP493Event.SpawnASCP493(player);
                            }
                            break;
                        case 1:
                            if (Player.List.Count() >= AwaYinNaiConfig.d9341minnum)
                            {
                                D9341Events.SpanwAD9341(player);
                            }
                            break;
                        case 2:
                            if (Player.List.Count() >= AwaYinNaiConfig.scp181minnum)
                            {
                                SCP181Event.SpawnAScp181(player);
                            }
                            break;
                    }
                }

                if (player.Role.Type == RoleTypeId.Scientist)
                {
                    scientist++;
                    switch (scientist)
                    {
                        case 1:
                            if (Player.List.Count() >= AwaYinNaiConfig.scpcn08minnum)
                            {
                                scpcn08.SpawnACn08(player);
                            }
                            break;
                    }
                }

                if (player.Role.Type == RoleTypeId.FacilityGuard)
                {
                    g++;
                    switch (g)
                    {
                        case 1:
                            if (Player.List.Count() >= AwaYinNaiConfig.scp550minnum)
                            {
                                SCP550Event.SpawnAScp550(player);
                            }
                            break;
                        case 2:
                            if (Player.List.Count() >= 30)
                            {
                                SCP999Event.SpawnAScp999(player);
                            }
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

            if (Player.List.Count() >= 30)
            {
                if (!SCP2490Event.scp2490spawnyes)
                {
                    SCP2490Event.scp2490spawnyes = true;
                    SCP2490Event.SpawnAScp2490(ppp);
                }
            }
        }

        private static void OnPLayerDying(DyingEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP181))
                {
                    YYYApi.MyApi.ScpDeath("SCP 1 8 1",ev.DamageHandler.Type ,ev.Attacker.Nickname);
                }
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP035))
                {
                    YYYApi.MyApi.ScpDeath("SCP 0 3 5",ev.DamageHandler.Type ,ev.Attacker.Nickname);
                }
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP550))
                {
                    YYYApi.MyApi.ScpDeath("SCP 5 5 0",ev.DamageHandler.Type ,ev.Attacker.Nickname);
                }
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP2490))
                {
                    YYYApi.MyApi.ScpDeath("SCP 2 4 9 0",ev.DamageHandler.Type ,ev.Attacker.Nickname);
                }
                if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCPCN08))
                {
                    ev.Player.Scale = Vector3.one;
                }
            }
        }

        private static void OnPlayerEnteringPocket(EnteringPocketDimensionEventArgs ev)
        {
            if (RoleManger.GetSide(ev.Player.Id) == Side.Scp)
            {
                ev.IsAllowed = false;
            }

            if (RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP999))
            {
                ev.IsAllowed = false;
            }
        }

        private static void OnRoundEnding(EndingRoundEventArgs ev)
        {
            
        }
        public static void Register()
        {          
            AwaYinNaiConfig = Main.Singleton.Config;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting += Onhurting;
            Exiled.Events.Handlers.Player.Dying += OnPLayerDying;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnPlayerEnteringPocket;
            Exiled.Events.Handlers.Server.RespawningTeam += OnTeamspawn;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound += OnRoundEnding;
        }
        public static void Unregister() {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting -= Onhurting;
            Exiled.Events.Handlers.Player.Dying -= OnPLayerDying;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnPlayerEnteringPocket;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnTeamspawn;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound -= OnRoundEnding;
        }
    }
}
