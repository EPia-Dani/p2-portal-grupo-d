using UnityEngine;
using System;

public static class PortalEvents
{
    public static event System.Action<Portal, Portal, GameObject> OnPlayerTeleported;

    public static event System.Action<Portal, Portal, GameObject> OnCubeTeleported;

    public static event System.Action OnBluePortalActivated;
    public static event System.Action OnOrangePortalActivated;

    public static void RaisePlayerTeleported(Portal fromPortal, Portal toPortal, GameObject player)
    {
        OnPlayerTeleported?.Invoke(fromPortal, toPortal, player);

    }
    public static void RaiseCubeTeleported(Portal fromPortal, Portal toPortal, GameObject cube)
    {
        OnCubeTeleported?.Invoke(fromPortal, toPortal, cube);
    }

    public static void RaiseBluePortalActivated()
    {
        OnBluePortalActivated?.Invoke();
    }    public static void RaiseOrangePortalActivated()
    {
        OnOrangePortalActivated?.Invoke();
    }
}
