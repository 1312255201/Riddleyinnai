using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.Pickups;
using MEC;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Riddleyinnai.Fuctions
{
    internal class SCP_524
    {
        public static int Delay = 180;

        public static bool BeginFlag = false;

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
                    Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast("<color=#dc143c>[Scp-524]</color>地上有好多好吃的~我不客气啦~", 5));
                    Timing.CallDelayed(10f, () =>
                    {
                        foreach(var pickup in Pickup.List)
                        {
                            if(pickup.Type.IsScp() || pickup.Type.IsThrowable() || pickup.Type == ItemType.ArmorHeavy || pickup.Type == ItemType.GunFRMG0 || pickup.Type == ItemType.GunLogicer
                            || pickup.Type == ItemType.Medkit || pickup.Type == ItemType.KeycardO5 || pickup.Type == ItemType.KeycardFacilityManager)
                            {
                                continue;
                            }
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
                Exiled.API.Features.Map.Broadcast(new Exiled.API.Features.Broadcast("<color=#dc143c>[Scp-524]</color>我要开始清理这个设施咯", 5));
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
        public static void Register()
        {
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += LczLocked;
            //Exiled.Events.Handlers.Server.RoundStarted += ClearObjects;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Server.RespawningTeam += ClearRagDoll;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Map.AnnouncingDecontamination -= LczLocked;
            //Exiled.Events.Handlers.Server.RoundStarted -= ClearObjects;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Server.RespawningTeam -= ClearRagDoll;
        }
    }
}
