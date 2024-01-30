using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features.DamageHandlers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddleyinnai.Misc
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Getkiller : ParentCommand
    {
        public override string Command => "getkiller";

        public override string[] Aliases => new string[] {"gk" };

        public override string Description => "查询本局全部的击杀记录";

        public override void LoadGeneratedCommands()
        {
            //
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "";
            for(int i = 1;i<=killer.killrecords.Count;i++)
            {
                var record = killer.killrecords[i-1];
                //var hitbox = (record.damageHandler.Base as UniversalDamageHandler).Hitbox;
                //var hitbox = (record.damageHandler.Base as UniversalDamageHandler).Hitbox;
                response += $"[{i}][{record.TargetRole}]{record.TargetName}({record.TargetId})被[{record.KillerRole}]{record.KillerName}({record.KillerId})使用{record.damageHandler.Type}杀死了\n";
            }
            return true;
        }
    }
    public class killer
    {
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Died += Onkill;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Died -= Onkill;
        }
        public static void Onkill(Exiled.Events.EventArgs.Player.DiedEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                var kill = new Killrecord();
                kill.time = DateTime.Now;
                kill.KillerName = ev.Attacker.Nickname;
                kill.KillerId = ev.Attacker.UserId;
                kill.KillerRole = ev.Attacker.Role.Type;
                kill.damageHandler = ev.DamageHandler;
                kill.TargetName = ev.Player.Nickname;
                kill.TargetId = ev.Player.UserId;
                kill.TargetRole = ev.TargetOldRole;  

                killrecords.Add(kill);
            }
        }
        public static void Reset()
        {
            killrecords.Clear();
        }
        public class Killrecord
        {
            public DateTime time { get; set; }
            public string KillerName { get; set; }
            public string KillerId { get; set;}
            public RoleTypeId KillerRole { get; set;}
            public DamageHandler damageHandler { get; set; }
            public string TargetName { get; set; }
            public string TargetId { get; set; }
            public RoleTypeId TargetRole { get; set; }
        }
        public static List<Killrecord> killrecords = new List<Killrecord>();
    }

}
