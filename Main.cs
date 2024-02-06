using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using Riddleyinnai.Database;
using Riddleyinnai.Database.Model;
using Riddleyinnai.Fuctions;
using Riddleyinnai.Misc;
using System;
using System.Net.WebSockets;
using System.Security.Cryptography;
using Exiled.API.Features.Doors;
using Riddleyinnai.Fuctions.Items;
using Riddleyinnai.Fuctions.Map;
using Riddleyinnai.Fuctions.SpRoleManage;
using Riddleyinnai.Fuctions.SpRoles;
using Riddleyinnai.Fuctions.SpRoles.ChaosRoles;
using Riddleyinnai.Fuctions.SpRoles.CllassD;
using Riddleyinnai.Fuctions.SpRoles.MTFRoles;
using Riddleyinnai.Fuctions.SpRoles.SCPRoles;
using Riddleyinnai.Fuctions.SpRoles.Tutroles;

namespace Riddleyinnai
{
    public class Main : Plugin<YinNaiConfig>
    {
        public override string Name => "迷子音奈主插件";
        public override string Author => "Niruri";
        public override Version RequiredExiledVersion => new Version(7, 2, 0);
        public override Version Version => new Version(1, 0, 0);
        public override PluginPriority Priority => PluginPriority.Highest;
        public override string Prefix => "新纯净服配置文件";
        public static Main Singleton { get; set; }
        public Harmony Patches { get; private set; }
        public override void OnEnabled()
        {
            Singleton = this;
            Patches = new Harmony($"Patch{DateTime.Now.Millisecond.ToString()}");
            Patches.PatchAll();
            Methods.Init(Config);
            Exception _ = null;
            if (!EventManage(true, out _))
            {
                Log.Warn($"Oh,no...there's something wrong in \n{_.StackTrace}");

            }
            else
            {
                Log.Info("main plugin for this sever has been enabled now safely, all events registered!");
            }

            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exception _ = null;

            //Network.Websocket.Reset();
            if (!EventManage(false, out _))
            {
                Log.Warn($"Oh,no...there's something wrong in \n{_.StackTrace}");
            }
            else
            {
                Log.Info("main plugin for this sever has been disabled now safely.");
            }
            Patches.UnpatchAll();
            base.OnDisabled();
        }
        public static bool EventManage(bool reg, out Exception exception)
        {
            if (reg)
            {
                try
                {
                    //Fuctions.Allban.Register();
                    Fuctions.AdminLog.Register();
                    //Fuctions.Alloction.Main.Register();
                    Fuctions.Alloction.Roundwaiting.Register();
                    Fuctions.Omega.Register();
                    Fuctions.SCPMaxHP.Register();
                    Fuctions.SysWarhead.Register();
                    Fuctions.SCP_524.Register();
                    Fuctions.Effectkeeper.Register();
                    Fuctions.SupplyMap.Register();
                    DaMa.Register();
                    Misc.killer.Register();
                    Misc.Infammo.Register();
                    Misc.Info914.Register();
                    Misc.Warheadrating.Register();
                    Misc.Moveboost.Register();
                    Misc.SCPRecovery.Register();

                    Ui.PlayerMain.Register();
                    
                    
                    User.Achieve.Main.Register();
                    User.Achieve.Details.Box.Register();
                    User.Achieve.Details.Airlock.Register();
                    User.Achieve.Details.Box.Register();
                    User.Achieve.Details.Crazyer.Register();
                    User.Achieve.Details.Doorbreaker.Register();
                    User.Achieve.Details.Escapingpocket.Register(); 
                    User.Achieve.Details.Fallinggod.Register();
                    User.Achieve.Details.Healer.Register();
                    
                    User.Achieve.Details.Jumpgod.Register();
                    User.Achieve.Details.Killbest.Register();
                    User.Achieve.Details.MulHid.Register();
                    User.Achieve.Details.Necks.Register();
                    User.Achieve.Details.Peaceful.Register();
                    User.Achieve.Details.Reloadgod.Register();
                    User.Achieve.Details.Roundbreaker.Register();
                    User.Achieve.Details.SCP914god.Register();
                    User.Achieve.Details.ZombiesKiller.Register();
                    D9341Events.Register();
                    SCP181Event.Register();
                    SCP069Event.Register();
                    RoleManger.Register();
                    User.Exp.Register();
                    User.StatTrak.Register();
                    User.Badge.Register();
                    /*
                    
                    User.Permission.Register(); 
                    */
                    NTFHealth.Register();
                    SCP035Event.Register();
                    Fuctions.Hoilday.Killrank.Register();
                    Fuctions.Hoilday.Nospawn.Register();
                    gatec.Register();
                    SCP999Event.Reg();
                    LieSheDan.Reg();
                    SCP2818.Register();
                    SCP550Event.Register();
                    SuperMedik.Register();
                    SCP1143.Register();
                    ILoop.Register();
                    JuJiQiang.Reg();
                    ETC.Register();
                    SCP2490Event.Register();
                    exception = null;
                    return true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
            else
            {
                try
                {
                    //Fuctions.Allban.Unregister();
                    Fuctions.AdminLog.Unregister();
                    //Fuctions.Alloction.Main.Unregister();
                    Fuctions.Alloction.Roundwaiting.Unregister();
                    Fuctions.Omega.Unregister();
                    Fuctions.SCPMaxHP.Unregister();
                    Fuctions.SysWarhead.Unregister();
                    Fuctions.SCP_524.Unregister();
                    Fuctions.Effectkeeper.Unregister();
                    //Fuctions.SupplyMap.Unregister();

                    Misc.killer.Unregister();
                    Misc.Infammo.Unregister();
                    Misc.Info914.Unregister();
                    Misc.Warheadrating.Unregister();
                    Misc.Moveboost.Unregister();
                    Misc.SCPRecovery.Unregister();

                    Ui.PlayerMain.Unregister();
                    User.Achieve.Main.Unregister();
                    User.Achieve.Details.Airlock.Unregister();
                    User.Achieve.Details.Box.Unregister();
                    User.Achieve.Details.Crazyer.Unregister();
                    User.Achieve.Details.Doorbreaker.Unregister();
                    User.Achieve.Details.Escapingpocket.Unregister();
                    User.Achieve.Details.Fallinggod.Unregister();
                    User.Achieve.Details.Healer.Unregister();
                    User.Achieve.Details.Jumpgod.Unregister();
                    User.Achieve.Details.Killbest.Unregister();
                    User.Achieve.Details.MulHid.Unregister();
                    User.Achieve.Details.Necks.Unregister();
                    User.Achieve.Details.Peaceful.Unregister();
                    User.Achieve.Details.Reloadgod.Unregister();
                    User.Achieve.Details.Roundbreaker.Unregister();
                    User.Achieve.Details.SCP914god.Unregister();
                    User.Achieve.Details.ZombiesKiller.Unregister();
                    User.Badge.Unregister();
                    User.Exp.Unregister();
                    User.Permission.Unregister();
                    User.StatTrak.Unregister();
                    D9341Events.UnRegister();
                    SCP181Event.UnRegister();
                    LieSheDan.UnReg();
                    Fuctions.Hoilday.Killrank.Unregister();
                    Fuctions.Hoilday.Nospawn.Unregister();
                    DaMa.UnRegister();
                    gatec.UnRegister();
                    SCP2818.UnRegister();
                    SuperMedik.UnRegister();
                    SCP069Event.UnRegister();
                    ILoop.Unregister();
                    SCP035Event.UnRegister();
                    ETC.Unregister();
                    RoleManger.Unregister();             
                    SCP999Event.UnReg();
                    SCP2490Event.UnRegister();
                    SCP1143.UnRegister();
                    SCP550Event.UnRegister();
                    NTFHealth.UnRegister();
                    JuJiQiang.UnReg();
                    exception = null;
                    return true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
        }
    }
}
