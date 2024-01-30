using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace Riddleyinnai.YYYApi;

public class MyApi
{
    public static Player GetRamdomDeadPlayer()
    {
        var players = Player.List.Where(player => player.Role.Type == RoleTypeId.Spectator && !player.IsOverwatchEnabled).ToList();
        return players.Any() ? players[new System.Random().Next(0, players.Count - 1)] : null;
    }
    public static void SetNickName(string text, string color, Player player)
    {
        if (player.IsConnected)
        {
            if (player.GlobalBadge == null)
            {
                player.RankName = text;
                player.RankColor = color;
            }
        }
    }
    private static IEnumerator<float> ScpDeath2(string scp, DamageType damageType, string killername)
    {
        var scp2 = scp.Replace(" ","");
        while(true)
        {
            if (Cassie.IsSpeaking)
            {
                yield return Timing.WaitForSeconds(3f);
            }
            else
            {
                switch (damageType)
                {
                    case DamageType.Tesla:
                        Cassie.MessageTranslated(scp + " successfully terminated by automatic security system.", scp2 + "收容成功，被电网电死了", true);
                        break;
                    case DamageType.Warhead:
                        Cassie.MessageTranslated(scp + " terminated by Alpha Warhead.", scp2 + "收容成功，死于核弹", true);
                        break;
                    case DamageType.Unknown:
                        Cassie.MessageTranslated(scp + " contained successfully.containment unit unknown.", scp2 + "收容成功，死因未知", true);
                        break;
                    default:
                        Cassie.MessageTranslated(scp + " contained successfully.", scp2 + "收容成功 收容者" + killername, true);
                        break;
                }

                break;
            }
        }
    }
    public static void ScpDeath(string scp, DamageType damageType,string killername)
    {
        Timing.RunCoroutine(ScpDeath2(scp,damageType, killername));
    }
}