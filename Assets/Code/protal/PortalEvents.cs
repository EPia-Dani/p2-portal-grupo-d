using UnityEngine;
using System;

public static class PortalEvents
{
    public static event Action<Portal, Portal, GameObject> OnPlayerTeleported;
    public static void RaisePlayerTeleported(Portal fromPortal, Portal toPortal, GameObject player)
    {
        OnPlayerTeleported?.Invoke(fromPortal, toPortal, player);
    }
}
