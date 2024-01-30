using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Riddleyinnai.Misc
{
    public class Component : MonoBehaviour
    {
        public Player player;
        private float _counter;
        private void Update()
        {
            if(player.Role.Type == PlayerRoles.RoleTypeId.Spectator || player.IsOverwatchEnabled)
            {
                return;
            }
            _counter += Time.deltaTime;
            if (_counter < 3)
                return;
            _counter = 0;

            var custominfo = "";
            if (player != null)
            {
                if (player.Health <= player.MaxHealth * 0.2f)
                {
                    custominfo = $"<color=#C50000>{player.Health}/{player.MaxHealth}</color>";

                }
                else if(player.Health <= player.MaxHealth * 0.4f)
                {
                    custominfo = $"<color=#FF9966>{player.Health}/{player.MaxHealth}</color>";
                }
                else if (player.Health <= player.MaxHealth * 0.8f)
                {
                    custominfo = $"<color=#FAFF86>{player.Health}/{player.MaxHealth}</color>";
                }
                else
                {
                    custominfo = $"<color=#228B22>{player.Health}/{player.MaxHealth}</color>";
                }
            }

            player.CustomInfo = custominfo;
        }
    }
    
    internal class Playerinfo
    {
        public static void Info()
        {
            foreach (var player in Player.List)
            {
                if (player.Role.Type == PlayerRoles.RoleTypeId.Spectator || player.IsOverwatchEnabled || player.IsTutorial)
                {
                    continue;
                }
                else
                {
                    var custominfo = "";
                    if (player != null)
                    {
                        if (player.Health <= player.MaxHealth * 0.2f)
                        {
                            custominfo = $"<color=#C50000>{player.Health}/{player.MaxHealth}</color>";

                        }
                        else if (player.Health <= player.MaxHealth * 0.4f)
                        {
                            custominfo = $"<color=#FF9966>{player.Health}/{player.MaxHealth}</color>";
                        }
                        else if (player.Health <= player.MaxHealth * 0.8f)
                        {
                            custominfo = $"<color=#FAFF86>{player.Health}/{player.MaxHealth}</color>";
                        }
                        else
                        {
                            custominfo = $"<color=#228B22>{player.Health}/{player.MaxHealth}</color>";
                        }
                    }

                    player.CustomInfo = custominfo;
                }
            }
        }
    }
}
