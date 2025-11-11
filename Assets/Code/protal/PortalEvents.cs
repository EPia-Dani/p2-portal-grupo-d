using UnityEngine;
using System;

public static class PortalEvents
{
    public static event Action<Portal, Portal, GameObject> OnPlayerTeleported;

    public static event Action<Portal, Portal, GameObject> OnCubeTeleported;

    public static void RaisePlayerTeleported(Portal fromPortal, Portal toPortal, GameObject player)
    {
        OnPlayerTeleported?.Invoke(fromPortal, toPortal, player);
    }
        public static void RaiseCubeTeleported(Portal fromPortal, Portal toPortal, GameObject cube)
    {
        OnCubeTeleported?.Invoke(fromPortal, toPortal, cube);
    }
}
