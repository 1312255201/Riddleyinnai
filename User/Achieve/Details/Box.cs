using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Box
    {
        private static Dictionary<string,DateTime> Spawnedtime = new Dictionary<string,DateTime>();
        public static void OnDied(DiedEventArgs ev)
        {
            if(ev.TargetOldRole.GetTeam() == PlayerRoles.Team.ChaosInsurgency || ev.TargetOldRole.GetTeam() == PlayerRoles.Team.FoundationForces)
            {
                var datetime = Spawnedtime[ev.Player.UserId];
                if(DateTime.Now - datetime <= TimeSpan.FromSeconds(30))
                {
                    Main.UpdateAchievement(ev.Player.UserId, new Database.Model.Achieve() { id = 1, name = "落地成盒", description = "重生为MTF或者CI后30秒阵亡", status = true, count = 0, hide = false, progress = 0 });
                }
            }
        }
        public static void Onrolechanged(ChangingRoleEventArgs ev)
        {
            if(ev.NewRole.GetTeam() == PlayerRoles.Team.ChaosInsurgency || ev.NewRole.GetTeam() == PlayerRoles.Team.FoundationForces)
            {
                if(Spawnedtime.TryGetValue(ev.Player.UserId,out var dateTime))
                {
                    Spawnedtime[ev.Player.UserId] = DateTime.Now;
                }
                else
                {
                    Spawnedtime.Add(ev.Player.UserId , DateTime.Now);
                }
            }
        }
        public static void Reset()
        {
            Spawnedtime.Clear();
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.ChangingRole += Onrolechanged;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.ChangingRole -= Onrolechanged;
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }
    }
}
