using UnityEngine;

public class CubeTeleportCode : MonoBehaviour
{

    void OnEnable()
    {
        PortalEvents.OnCubeTeleported += HandleTeleportEvent;

    }
    void OnDisable()
    {
        PortalEvents.OnCubeTeleported -= HandleTeleportEvent;


    }
    public void HandleTeleportEvent(Portal fromPortal, Portal toPortal, GameObject Object)
    {
        
        if(Object.GetComponent<Collider>()!=this.GetComponent<Collider>()) return;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        
        if (rb.isKinematic) return;

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

        //to maintain the momentum of the cube;
        Vector3 localVelocity = portalA.InverseTransformDirection(rb.linearVelocity);
        localVelocity.z = -localVelocity.z;
        localVelocity.x = -localVelocity.x;

        transform.SetPositionAndRotation(finalPos, Quaternion.LookRotation(finalDir, Vector3.up));

        Vector3 finalVelocity = portalB.TransformDirection(localVelocity); //correct the velocity
        rb.linearVelocity = finalVelocity;

    }
}
