using Exiled.API.Features;
using System.Linq;
using UnityEngine;

namespace Riddleyinnai.User.Achieve.Details
{
    internal class Doorbreaker
    {
        public static void DoorDamaged(Exiled.Events.EventArgs.Player.DamagingDoorEventArgs ev)
        {
            if(ev.DamageType == Interactables.Interobjects.DoorUtils.DoorDamageType.Scp096)
            {
                var scp106 = Player.Get(PlayerRoles.RoleTypeId.Scp106).FirstOrDefault();
                var scp096 = Player.Get(PlayerRoles.RoleTypeId.Scp096).FirstOrDefault();
                if(scp106 != null && scp096 != null) { 
                    if(Vector3.Distance(scp106.Position,scp096.Position) <= 10)
                    {
                        if(ev.Door.Type == Exiled.API.Enums.DoorType.Scp106Primary || ev.Door.Type == Exiled.API.Enums.DoorType.Scp106Secondary)
                        {
                            var ach = Main.achieves.Find(x => x.id == 16);
                            ach.status = true;
                            Main.UpdateAchievement(scp096.UserId, ach);
                            Main.UpdateAchievement(scp106.UserId, ach);
                        }
                    }
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.DamagingDoor += DoorDamaged;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.DamagingDoor -= DoorDamaged;
        }
    }
}
