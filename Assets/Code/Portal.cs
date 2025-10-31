using Unity.Cinemachine;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Analytics;

public class Portal : MonoBehaviour
{
    public Camera playerCamera;
    public Portal otherPortal;
    public Camera reflectionCamera;
    [SerializeField] private Transform reflectionTransform;
    float effectNearPlane;
    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }
    void LateUpdate(){

        Vector3 worldPosition = playerCamera.transform.position;
        Vector3 localPosition =  reflectionTransform.InverseTransformPoint(worldPosition);
        otherPortal.reflectionCamera.transform.position = otherPortal.transform.TransformPoint(localPosition);

        Vector3 worldDirection = playerCamera.transform.forward;
        Vector3 localDirection = reflectionTransform.InverseTransformDirection(worldDirection);
        otherPortal.reflectionCamera.transform.forward= otherPortal.transform.TransformDirection(localDirection);
     /*   // Convert player direction to this portal's local space
        Vector3 localDir = transform.InverseTransformDirection(playerCamera.transform.forward);
        localDir.z = -localDir.z;

        // Same for up vector to preserve roll
        Vector3 localUp = transform.InverseTransformDirection(playerCamera.transform.up);
        localUp.z = -localUp.z;

        // Apply rotation
        otherPortal.reflectionCamera.transform.rotation = Quaternion.LookRotation(
            otherPortal.transform.TransformDirection(localDir),
            otherPortal.transform.TransformDirection(localUp)
        );*/

        //float distance = Vector3.Distance( otherPortal.reflectionTransform.transform.position, otherPortal.reflectionTransform.position );
        //otherPortal.portalCamera.nearClipPlane= Mathf.Max(0.0f,distance)+effectNearPlane;


    }
}
