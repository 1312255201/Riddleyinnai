using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Riddleyinnai.Fuctions.Alloction
{
    public class Roundwaiting
    {
        public static bool IsLobby => !Round.IsStarted && !RoundSummary.singleton._roundEnded;


        private static CoroutineHandle lobbyTimer;

        public static string CustomMessage { get; set; } = "<size=35><color=red>输入.sq + 申请代码即可申请角色</color></size>\n";

        public static string TopMessage { get; set; } = "<size=40><color=yellow><b>回合即将开始,剩余等待时间：{seconds}</b></color></size>";

        public static string BottomMessage { get; set; } = "<size=30><i>{players}</i></size>";

        public static string ServerIsPaused { get; set; } = "回合已暂停";

        public static string RoundIsBeingStarted { get; set; } = "请稍等 系统正在为您分配角色";

        public static string OneSecondRemain { get; set; } = "秒";

        public static string XSecondsRemains { get; set; } = "秒";

        public static string OnePlayerConnected { get; set; } = "个玩家已连接到服务器";

        public static string XPlayersConnected { get; set; } = "个玩家已连接到服务器";
        public static int HintVertPos { get; set; } = 0;
        private static SchematicObject obj = null;
        public static void NoHurt(HurtingEventArgs ev)
        {
            if (lobbyTimer.IsRunning)
            {
                ev.Amount = 0;
            }
        }
        public static void WaitingPlayers()
        {
            //obj = MapEditorReborn.API.Features.ObjectSpawner.SpawnSchematic("ClassLobby", new Vector3(0, 1000, 0));
            CustomMessage = $"<size=35><color=red>捡起角色模型前的硬币即可申请角色</color></size>\n";
            try
            {
                GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            }
            catch
            {
            }

            if (lobbyTimer.IsRunning)
            {

            }

            //lobbyTimer = Timing.RunCoroutine(LobbyTimer());
        }
        public static void OnJoin(VerifiedEventArgs ev)
        {
            if (IsLobby && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.Role.Set(RoleTypeId.Tutorial);
                    ev.Player.AddItem(ItemType.Coin);
                });
            }
        }

        public static void OnDied(DiedEventArgs ev)
        {
            if (IsLobby)
            {
                ev.Player.ClearInventory();
            }
        }
        public static void RoundStart()
        {
            if (lobbyTimer.IsRunning)
            {
                Timing.KillCoroutines(lobbyTimer);
            }
        }
        public static void Droping(DroppingItemEventArgs ev)
        {
            if (Round.IsLobby)
            {
                if (ev.Item.Type == ItemType.Coin && ev.Player.Role.Type == RoleTypeId.Tutorial)
                {
                    if (Vector3.Distance(ev.Player.Position, new Vector3(-17.2f, 1001.4f, 98.6f)) <= 3)
                    {
                        ev.IsAllowed = false;
                        ev.Player.RemoveItem(ev.Item);
                        ev.Player.AddItem(ItemType.SCP207);
                    }
                }
            }
        }
        public static void Register()
        {
            Exiled.Events.Handlers.Player.DroppingItem += Droping;
            Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += RoundStart;
            Exiled.Events.Handlers.Player.Verified += OnJoin;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += NoHurt;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= Droping;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStart;
            Exiled.Events.Handlers.Player.Verified -= OnJoin;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= NoHurt;
        }
    }
}
