using Exiled.API.Extensions;
using Exiled.API.Features;
using MapEditorReborn.API.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Misc
{
    public class Moveboost
    {
        public static void SCPMove()
        {
            foreach (var item in scps)
            {
                if (Player.TryGet(item, out var scp))
                {
                    if (scp.Zone == Exiled.API.Enums.ZoneType.Surface)
                    {
                        scp.EnableEffect<CustomPlayerEffects.MovementBoost>(byte.MaxValue, true);
                        scp.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>(10, byte.MaxValue);
                        scp.Heal(1f);
                    }
                    else
                    {
                        scp.DisableEffect<CustomPlayerEffects.MovementBoost>();
                    }
                }
            }
        }
        private static List<string> scps = new List<string>();
        public static void Onrolechanged(Exiled.Events.EventArgs.Player.ChangingRoleEventArgs ev)
        {
            if(scps.Contains(ev.Player.UserId))
            {
                if(ev.NewRole.GetSide() != Exiled.API.Enums.Side.Scp)
                {
                    scps.Remove(ev.Player.UserId);
                }
            }
            else
            {
                if (ev.NewRole.GetSide() == Exiled.API.Enums.Side.Scp)
                {
                    scps.Add(ev.Player.UserId);
                }
            }
        }
        public static void Reset()
        {
            scps.Clear();
        }

        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.ChangingRole += Onrolechanged;
        }
        public static void Unregister() 
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.ChangingRole -= Onrolechanged;
        }
    }
}
