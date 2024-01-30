using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using HarmonyLib;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using static HarmonyLib.AccessTools;
using Events = Exiled.Events.Events;

namespace Riddleyinnai.Fuctions
{

    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    internal class CommandLogging
    {
        /// <summary>
        /// Logs a command to the RA log file.
        /// </summary>
        /// <param name="query">The command being logged.</param>
        /// <param name="sender">The sender of the command.</param>
        public static void LogCommand(string query, CommandSender sender)
        {
            try
            {
                if (query.StartsWith("$", StringComparison.Ordinal))
                    return;

                Player player = sender is PlayerCommandSender playerCommandSender && sender != Server.Host.Sender
                    ? Player.Get(playerCommandSender)
                    : Server.Host;

                string logMessage = string.Empty;

                try
                {
                    logMessage =
                        $"[{DateTime.Now}] {(player == Server.Host ? "Server Console" : $"{player?.Nickname} ({player?.UserId}) {player?.IPAddress}")}" +
                        $" has run the command {query}.\n";
                    AdminLog.LogCommand(player, query);
                }
                catch (Exception exception)
                {
                    Log.Error($"{nameof(CommandLogging)}: Failed to log command; unable to parse log message.\n{player is null}\n{exception}");
                }

                if (string.IsNullOrEmpty(logMessage))
                    return;

                string directory = Path.Combine(Paths.Exiled, "Logs");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string filePath = Path.Combine(directory, $"{Server.Port}-RAlog.txt");

                if (!File.Exists(filePath))
                    File.Create(filePath).Close();

                File.AppendAllText(filePath, logMessage);
            }
            catch (Exception exception)
            {
                Log.Error($"{nameof(CommandLogging)}: Unable to log a command.\n{string.IsNullOrEmpty(query)} - {sender is null}\n{exception}");
            }
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            const int index = 0;

            Label continueLabel = generator.DefineLabel();

            newInstructions.InsertRange(
                index,
                new[]
                {
                    // if (!Events.Instance.Config.LogRaCommands)
                    //   goto continueLabel;
                    new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Events), nameof(Events.Instance))),
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Events), nameof(Events.Config))),
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Exiled.Events.Config), nameof(Exiled.Events.Config.LogRaCommands))),
                    new CodeInstruction(OpCodes.Brfalse, continueLabel),

                    // q
                    new CodeInstruction(OpCodes.Ldarg_0),

                    // sender
                    new CodeInstruction(OpCodes.Ldarg_1),

                    // LogCommand(q, sender)
                    new CodeInstruction(OpCodes.Call, Method(typeof(CommandLogging), nameof(LogCommand))),

                    // continueLabel:
                    new CodeInstruction(OpCodes.Nop).WithLabels(continueLabel),
                });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
    public class AdminLog
    {
        public static List<string> CommandLogs = new List<string>();
        public static void LogCommand(Player commander, string query)
        {
            new Thread(() =>
            {
                try
                {
                    if (query == "$1" || query == "$0 1") return;
                    if (commander != null && commander != Server.Host)
                    {
                        var cmd = query.Replace(".", "");
                        AdminLog.CommandLogs.Add($"[{commander.GroupName.ToLower()}][{DateTime.Now}]{commander.Nickname}使用了指令:{cmd}\n");
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn(ex.ToString());
                    Log.Warn(ex.StackTrace);
                }
            }).Start();
        }
        public static void Reset() => CommandLogs.Clear();
        public static void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += Reset;
        }
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Reset;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AdminLogCommand : ParentCommand
    {
        public override string Command => "adminlog";

        public override string[] Aliases => new string[] { "al", "log" };

        public override string Description => "查询当局管理员情况使用情况(无汉化)";

        public override void LoadGeneratedCommands()
        {
            //
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var commander = Player.Get(sender);
            if (commander == null)
            {
                response = "";
                return false;
            }
            if (!Publicmethod.Checkpermission(commander, new List<string>() { "ADMIN", "OWNER", "high00", "high01", "high02", "high03", "high04" }))
            {
                response = "You don't have permissions to access to do this command!";
                return false;
            }
            if (AdminLog.CommandLogs.Any())
            {
                var _ = $"本局共使用了{AdminLog.CommandLogs.Count}次权限,下列是权限使用记录:\n";
                foreach (var text in AdminLog.CommandLogs)
                {
                    _ += text;
                }
                response = _;
                return true;
            }
            response = "暂无使用记录,请稍后再查看";
            return false;
        }
    }
}
