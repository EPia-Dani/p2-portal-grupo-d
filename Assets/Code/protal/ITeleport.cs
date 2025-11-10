using UnityEngine;

public interface ITeleport 
{
     void HandleTeleportEvent(Portal fromPortal, Portal toPortal, GameObject Object);
}
