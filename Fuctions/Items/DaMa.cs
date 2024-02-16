using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Items
{
    public class DaMa
    {
        public static List<ushort> DaMaList = new List<ushort>();
        public static Dictionary<int,int> DaMaUseList = new Dictionary<int,int>();
        private static void OnRoundStarted()
        {
            Timing.RunCoroutine(CheckTiming());
        }
        private static void OnUsingItem(UsingItemEventArgs ev)
        {
            if(DaMaList.Contains(ev.Item.Serial))
            {
                ev.IsAllowed = false;
                ev.Player.RemoveItem(ev.Item);
                ev.Player.Heal(5);
                if(!DaMaUseList.ContainsKey(ev.Player.Id))
                {
                    DaMaUseList.Add(ev.Player.Id ,1);
                    ev.Player.Broadcast(3, "你使用了一个SCP420-J,当前你以使用" + DaMaUseList[ev.Player.Id]);
                    ev.Player.EnableEffect<Concussed>(5f);
                    Timing.CallDelayed(3f, () =>
                    {
                        ev.Player.DisableEffect<Concussed>();
                    });
                }
                else
                {
                    DaMaUseList[ev.Player.Id]++;
                    ev.Player.Broadcast(3, "你使用了一个SCP420-J,当前你以使用" + DaMaUseList[ev.Player.Id]);
                    if (DaMaUseList[ev.Player.Id] % 8 == 0)
                    {
                        ev.Player.EnableEffect<Scp207>();
                        ev.Player.ReferenceHub.playerEffectsController.ChangeState<Scp207>((byte)(DaMaUseList[ev.Player.Id] / 8));
                    }
                    ev.Player.EnableEffect<Concussed>(5f);
                    Timing.CallDelayed(3f, () =>
                    {
                        ev.Player.DisableEffect<Concussed>();
                    });
                }
            }
        }
        private static void OnPlayerHurting(HurtingEventArgs ev)
        {


        }
        private static void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if(DaMaUseList.ContainsKey(ev.Player.Id))
            {
                DaMaUseList.Remove(ev.Player.Id);
            }
        }
        private static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            DaMaList.Clear();
        }
        private static IEnumerator<float> CheckTiming()
        {
            yield return Timing.WaitForSeconds(1f);
            int time = new System.Random().Next(15,30);
            var room = Room.Get(Exiled.API.Enums.RoomType.LczPlants);
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                time--;
                if(time < 0)
                {
                    time = new System.Random().Next(15, 30);
                    var pickup = Pickup.CreateAndSpawn(ItemType.Painkillers, room.Position + room.transform.forward * 4.5f + room.transform.right*4.5f+ Vector3.up, Quaternion.identity);
                    pickup.Scale = new Vector3(1, 1, 3);
                    DaMaList.Add(pickup.Serial);
                    pickup = Pickup.CreateAndSpawn(ItemType.Painkillers, room.Position + room.transform.forward * 4.5f + Vector3.up, Quaternion.identity);
                    pickup.Scale = new Vector3(1, 1, 3);
                    DaMaList.Add(pickup.Serial);
                    pickup = Pickup.CreateAndSpawn(ItemType.Painkillers, room.Position + room.transform.forward * 4.5f - room.transform.right * 4.5f + Vector3.up, Quaternion.identity);
                    pickup.Scale = new Vector3(1, 1, 3);
                    DaMaList.Add(pickup.Serial);
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.ChangingRole += OnPlayerChangingRole;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurting;

        }
        public static void UnRegister()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.ChangingRole -= OnPlayerChangingRole;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurting;
        }
    }
}