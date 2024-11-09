using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace Riddleyinnai.Fuctions
{
    internal class SCPMaxHP
    {
        public static Dictionary<RoleTypeId, int> MaxHP = new Dictionary<RoleTypeId, int>()
        {
            {RoleTypeId.Scp173,-1},
            {RoleTypeId.Scp939,-1},
            {RoleTypeId.Scp049,-1 },
            {RoleTypeId.Scp096,-1},
            {RoleTypeId.Scp106,-1},
            {RoleTypeId.Scp0492,-1}
        };
        public static void Reset()
        {
            var Config = Main.Singleton.Config;
            MaxHP[RoleTypeId.Scp049] = Config.HP049;
            MaxHP[RoleTypeId.Scp0492] = Config.HP0492;
            MaxHP[RoleTypeId.Scp096] = Config.HP096;
            MaxHP[RoleTypeId.Scp106] = Config.HP106;
            MaxHP[RoleTypeId.Scp173] = Config.HP173;
            MaxHP[RoleTypeId.Scp939] = Config.HP939;
        }
        private static float GetHP(Player player)
        {
            return MaxHP[player.Role.Type];
            float hp_default = player.Health;
            float add = MaxHP[player.Role.Type] - hp_default;
            if (add > 0)
            {
                var pieces = (Server.MaxPlayerCount - 20)>=1? (Server.MaxPlayerCount - 20):float.MaxValue;

                var div = add / pieces;

                return hp_default += div * (Player.List.Count - 20);
            }
            return hp_default;
        }
        public static void Spawned(Exiled.Events.EventArgs.Player.SpawnedEventArgs ev)
        {
            if (ev.Player != null)
            {
                if (MaxHP.ContainsKey(ev.Player.Role.Type))
                {
                    if (MaxHP[ev.Player.Role.Type] != -1)
                    {
                        var result = GetHP(ev.Player);
                        ev.Player.MaxHealth = result;
                        ev.Player.Health = result;
                    }
                }
                if(ev.Player.Role.Type == RoleTypeId.Scp3114)
                {
                    ev.Player.MaxHealth = 1250;
                    ev.Player.Health = 1250;
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Spawned += Spawned;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Spawned -= Spawned;
        }
    }
}
