using UnityEngine;

public class CubeTeleportCode : MonoBehaviour, ITeleport
{
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void HandleTeleportEvent(Portal fromPortal, Portal toPortal, GameObject Object)
    {
        //rigidbody.enabled = false;

        Transform portalA = fromPortal.transform;
        Transform portalB = toPortal.transform;

        //calculate position
        Vector3 localPos = portalA.InverseTransformPoint(transform.position);
        localPos.z = -localPos.z;
        localPos.x = -localPos.x;
        Vector3 finalPos = portalB.TransformPoint(localPos);

        //calculate dir
        Vector3 localDir = portalA.InverseTransformDirection(transform.forward);
        localDir.z = -localDir.z;
        localDir.x = -localDir.x;
        Vector3 finalDir = portalB.TransformDirection(localDir);

        //apply 
        transform.SetPositionAndRotation(finalPos, Quaternion.LookRotation(finalDir, Vector3.up));


        //rigidbody.enabled = true;

    }
}
