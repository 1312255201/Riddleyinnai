﻿using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles.Ragdolls;
using Exiled.API.Features;
using Riddleyinnai.Ui;
using Riddleyinnai.YYYApi;

namespace Riddleyinnai.Fuctions
{
    internal class SCP_524
    {
        public static int Delay = 180;

        public static bool BeginFlag = false;

        public static List<ushort> roundstartItems = new();

        public static void Reset()
        {
            BeginFlag = false;
        }

        public static void ClearObjects()
        {
            if (BeginFlag)
            {
                try
                {
                    foreach (var variablPlayer in Player.List)
                    {
                        Ui.PlayerMain.Send(variablPlayer,"<color=#0F0>[Scp-524]</color>地上有好多好吃的~我不客气啦~",5,Pos.正中偏下,5);
                    }
                    Timing.CallDelayed(10f, () =>
                    {
                        foreach(var pickup in Pickup.List)
                        {
                            if(pickup.isSpesialItem() || pickup.Type.IsScp() || pickup.Type.IsThrowable() || pickup.Type == ItemType.ArmorHeavy || pickup.Type == ItemType.GunFRMG0 || pickup.Type == ItemType.GunLogicer
                            || pickup.Type == ItemType.Medkit || pickup.Type == ItemType.KeycardO5 || pickup.Type == ItemType.KeycardFacilityManager || roundstartItems.Contains(pickup.Serial) || pickup.Serial == 0)
                            {
                                continue;
                            }
                            pickup.Destroy();
                        }
                    });
                }
                catch { }
            }
            Timing.CallDelayed(Delay, () =>
            {
                ClearObjects();
            });
        }
        public static void LczLocked(AnnouncingDecontaminationEventArgs ev)
        {
            if (ev.Id == 3)
            {
                BeginFlag = true;
               // Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast("<color=#dc143c>[Scp-524]</color>我要开始清理这个设施咯", 5));
                ClearObjects();
            }
        }
        public static void ClearRagDoll(RespawningTeamEventArgs ev)
        {
            BasicRagdoll[] dolls = UnityEngine.Object.FindObjectsOfType<BasicRagdoll>();
            foreach (BasicRagdoll ragdoll in dolls)
            {
                if (ragdoll.NetworkInfo.RoleType.GetSide() == Exiled.API.Enums.Side.Scp && ragdoll.NetworkInfo.ExistenceTime >= 2 * 60)
                {
                    continue;
                }
                else
                {
                    Mirror.NetworkServer.Destroy(ragdoll.gameObject);
                }
            }
        }

        private static void OnRoundStart()
        {
            roundstartItems.Clear();
            foreach (var varpick in Pickup.List)
            {
                roundstartItems.Add(varpick.Serial);
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += LczLocked;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Server.RespawningTeam += ClearRagDoll;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Map.AnnouncingDecontamination -= LczLocked;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Server.RespawningTeam -= ClearRagDoll;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }
    }
}
