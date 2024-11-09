using CommandSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Riddleyinnai.Database.Model;
using System;
using System.Collections.Generic;

namespace Riddleyinnai.User
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StatTrakCommand : ICommand
    {    public bool SanitizeResponse { get; }

        public string Command => "stattrak";

        public string[] Aliases => new string[] { };

        public string Description => "查询您的各种武器击杀记录";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var target = Player.Get(sender);
            if(StatTrak.Statraks.TryGetValue(target.UserId,out var model))
            {
                var res = "";
                res += $"\nAK:{model.AK}";
                res += $"\nE11:{model.E11SR}";
                res += $"\nCrossvec:{model.Crossvec}";
                res += $"\nCOM15:{model.COM15}";
                res += $"\nCOM45:{model.COM45}";
                res += $"\nCOM18:{model.COM18}";
                res += $"\nFRMG0:{model.FRMG0}";
                res += $"\nFSP9:{model.FS9}";
                res += $"\nLogicer:{model.Logicer}";
                res += $"\n3X:{model.ParticleDisruptor}";
                res += $"\n左轮:{model.Revolver}";
                res += $"\n喷子:{model.Shotgun}";
                res += $"\n电磁炮:{model.MicroHID}";
                res += $"\n手雷:{model.Explosion}";
                response = res; return true;
            }
            else
            {
                response = "暂无击杀记录"; return false;
            }
        }
    }

    public class StatTrak
    {
        public static Dictionary<string,Weapons> Statraks = new Dictionary<string,Weapons>();
        public static void Reset()
        {
            Statraks.Clear();
        }
        public static void OnJoined(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            if(ev.Player != null)
            {
             /*   if(!Statraks.ContainsKey(ev.Player.UserId))
                {
                    new Task(() =>
                    {
                        var res = Methods.Getweapons(ev.Player.UserId);
                        if (res != null)
                        {
                            if (res.Success)
                            {
                                Statraks.Add(ev.Player.UserId, res.Data);
                            }
                            else
                            {
                                if (res.Code == 404)
                                {
                                    Statraks.Add(ev.Player.UserId, new Weapons() { Userid = ev.Player.UserId });
                                }
                            }
                        }

                    }).Start();
                }*/
            }
        }
        public static void Ondied(DiedEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if(Statraks.TryGetValue(ev.Attacker.UserId,out var model))
                {
                    switch(ev.DamageHandler.Type)
                    {
                        case Exiled.API.Enums.DamageType.Com15:
                            model.COM15++;
                            break;
                        case Exiled.API.Enums.DamageType.Com18:
                            model.COM18++; break;
                        case Exiled.API.Enums.DamageType.Com45: model.COM45++; break;
                        case Exiled.API.Enums.DamageType.AK: model.AK++; break;
                        case Exiled.API.Enums.DamageType.Fsp9:model.FS9++; break;
                        case Exiled.API.Enums.DamageType.A7: model.AK++; break;
                        case Exiled.API.Enums.DamageType.MicroHid:model.MicroHID++; break;
                        case Exiled.API.Enums.DamageType.Crossvec:model.Crossvec++; break;
                        case Exiled.API.Enums.DamageType.E11Sr:model.E11SR++; break;
                        case Exiled.API.Enums.DamageType.Explosion:model.Explosion++; break;
                        case Exiled.API.Enums.DamageType.Frmg0:model.FRMG0++; break;
                        case Exiled.API.Enums.DamageType.Logicer:model.Logicer++; break;
                        case Exiled.API.Enums.DamageType.Revolver:model.Revolver++; break;
                        case Exiled.API.Enums.DamageType.Shotgun:model.Shotgun++; break;
                        case Exiled.API.Enums.DamageType.ParticleDisruptor:model.ParticleDisruptor++; break;
                    }
                }
            }
        }
        public static void OnLeft(LeftEventArgs ev)
        {
            if(ev.Player != null)
            {
                if (ev.Player.UserId != null)
                {
                    if(Statraks.TryGetValue(ev.Player.UserId,out var weapons))
                    {
                        //new Task(() => Methods.Updateweapons(weapons)).Start();
                        Statraks.Remove(ev.Player.UserId);
                    }    
                }
                
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Verified += OnJoined;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Died += Ondied;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Verified -= OnJoined;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Died -= Ondied;
        }
    }
}
