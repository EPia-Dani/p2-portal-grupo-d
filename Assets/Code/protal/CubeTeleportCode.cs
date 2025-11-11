using UnityEngine;

public class CubeTeleportCode : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        PortalEvents.OnCubeTeleported += HandleTeleportEvent;

    }
    void OndDisable()
    {
        PortalEvents.OnCubeTeleported -= HandleTeleportEvent;


    }
    public void HandleTeleportEvent(Portal fromPortal, Portal toPortal, GameObject Object)
    {

        Transform portalA = fromPortal.transform;
        Transform portalB = toPortal.transform;

        Vector3 localPos = portalA.InverseTransformPoint(transform.position);
        localPos.z = -localPos.z;
        localPos.x = -localPos.x;
        Vector3 finalPos = portalB.TransformPoint(localPos);

        Vector3 localDir = portalA.InverseTransformDirection(transform.forward);
        localDir.z = -localDir.z;
        localDir.x = -localDir.x;
        Vector3 finalDir = portalB.TransformDirection(localDir);

        transform.SetPositionAndRotation(finalPos, Quaternion.LookRotation(finalDir, Vector3.up));



    }
}
