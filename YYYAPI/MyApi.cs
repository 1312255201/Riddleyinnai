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
    public static Dictionary<Team, string> TeamTranslation = new()
    {
        { Team.Dead ,"阵亡"},
        { Team.Scientists ,"科学家"},
        { Team.FoundationForces ,"九尾狐"},
        { Team.SCPs ,"SCP"},
        { Team.OtherAlive ,"其他存活"},
        { Team.ChaosInsurgency ,"混沌分裂者"},
        { Team.ClassD ,"D级人员"},
    };
        
    public static Dictionary<RoleTypeId, string> TranslateOfRoleType = new()
    {
        {RoleTypeId.NtfPrivate,"九尾狐新兵" },
        {RoleTypeId.NtfCaptain,"九尾狐指挥官" },
        {RoleTypeId.NtfSergeant,"九尾狐中士" },
        {RoleTypeId.NtfSpecialist,"九尾狐收容专家" },
        {RoleTypeId.FacilityGuard,"设施保安" },
        {RoleTypeId.ChaosConscript,"混沌征召兵" },
        {RoleTypeId.ChaosMarauder,"混沌掠夺者" },
        {RoleTypeId.ChaosRepressor,"混沌镇压者" },
        {RoleTypeId.ChaosRifleman,"混沌抢手" },
        {RoleTypeId.Scp096,"SCP-096" },
        {RoleTypeId.Scp049,"SCP-049" },
        {RoleTypeId.Scp173,"SCP-173" },
        {RoleTypeId.Scp939,"SCP-939" },
        {RoleTypeId.Scp106,"SCP-106" },
        {RoleTypeId.Scp0492,"SCP-049-2" },
        {RoleTypeId.Scp079,"SCP-079" },
        {RoleTypeId.ClassD,"D级人员" },
        {RoleTypeId.Scientist,"科学家" },
        {RoleTypeId.Tutorial,"训练人员" },
        {RoleTypeId.Overwatch,"观察者" },
        {RoleTypeId.CustomRole,"本地角色？" },
        {RoleTypeId.Spectator,"观察者" },
        {RoleTypeId.Filmmaker,"导演模式" },
        {RoleTypeId.None,"空" },
        { RoleTypeId.Scp3114, "SCP-3114" },
    };
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