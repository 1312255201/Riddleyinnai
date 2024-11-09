using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Riddleyinnai.Fuctions.SpRoleManage;
using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;

namespace Riddleyinnai.Fuctions.Alloction;

public class RemoteKeycard
{
    private static void OnDoorInteract(InteractingDoorEventArgs ev)
    {
        try
        {
            if (ev.Door.Type == DoorType.Scp914Gate)
            {
                if (SpRoleManage.RoleManger.IsRole(ev.Player.Id, RoleManger.RoleName.SCP035))
                {
                    return;
                }
            }
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions) &&
                !ev.Door.IsLocked)
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    public static void Reg()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnDoorInteract;
    }

    public static void UnReg()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnDoorInteract;
    }
}

public static class RemoteKeyAPI
{
    public static bool HasKeycardPermission(
        this Player player,
        KeycardPermissions permissions,
        bool requiresAllPermissions = false)
    {
        if (true&& player.IsEffectActive<AmnesiaVision>())
            return false;

        return requiresAllPermissions
            ? player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlag(permissions))
            : player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & permissions) != 0);
    }
}