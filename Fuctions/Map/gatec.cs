using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using MEC;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Map
{
    public class gatec
    {
        private static List<int> gatccd = new List<int>();
        private static void OnRoundStarted()
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
                    if (gatccd.Contains(player.Id)) continue;
                    if (!(Vector3.Distance(player.Position, new Vector3(-40.5f, 991.5f, -36f)) <= 1)) continue;
                    if (player.CurrentItem == null) continue;
                    if (!player.CurrentItem.IsKeycard) continue;
                    if (!(player.CurrentItem is Keycard keycard)) continue;
                    if ((keycard.Permissions & KeycardPermissions.ExitGates) != 0)
                    {
                        player.Position = Room.Get(RoomType.EzIntercom).Position +Vector3.down *4.7f -Door.Get(DoorType.Intercom).Transform.forward * 4.5f - Door.Get(DoorType.Intercom).Transform.right*0.9f;
                        gatccd.Add(player.Id);
                        Timing.CallDelayed(4f, () =>
                        {
                            gatccd.Remove(player.Id);
                        });
                    }
                }
                foreach (var player in Player.List)
                {
                    if (gatccd.Contains(player.Id)) continue;
                    if (!(Vector3.Distance(player.Position, Room.Get(RoomType.EzIntercom).Position + Vector3.down * 4.7f - Door.Get(DoorType.Intercom).Transform.forward * 5.7f - Door.Get(DoorType.Intercom).Transform.right * 0.9f) <= 1.2f)) continue;
                    if (player.CurrentItem == null) continue;
                    if (!player.CurrentItem.IsKeycard) continue;
                    if (!(player.CurrentItem is Keycard keycard)) continue;
                    if ((keycard.Permissions & KeycardPermissions.ExitGates) != 0)
                    {
                        player.Position = new Vector3(-37.5f, 991.5f, -36f);
                        gatccd.Add(player.Id);
                        Timing.CallDelayed(4f, () =>
                        {
                            gatccd.Remove(player.Id);
                        });
                    }
                }
                if(Warhead.IsDetonated)
                {
                    foreach(var pl in Player.List)
                    {
                        if(pl.IsAlive)
                        {
                            try
                            {
                                if (pl.Zone != ZoneType.Surface && pl.CurrentRoom.Type != RoomType.Pocket)
                                {
                                    pl.Kill("核弹爆炸位置错误处死");
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Log.Debug("GateC启动");
        }

        public static void UnRegister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }
    }
}