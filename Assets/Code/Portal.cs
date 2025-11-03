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
    public float effectNearPlane=-0.5f;
    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }
    void LateUpdate(){

        Vector3 worldPosition = playerCamera.transform.position;
        Vector3 localPosition =  reflectionTransform.InverseTransformPoint(worldPosition);
        localPosition.z = -localPosition.z;
        otherPortal.reflectionCamera.transform.position = otherPortal.transform.TransformPoint(localPosition);
        

        Vector3 worldDirection = playerCamera.transform.forward;
        Vector3 localDirection = reflectionTransform.InverseTransformDirection(worldDirection);
        localDirection.z = -localDirection.z;
        otherPortal.reflectionCamera.transform.forward= otherPortal.transform.TransformDirection(localDirection);
     
        otherPortal.reflectionCamera.nearClipPlane= Mathf.Max(0.0f,distance)+effectNearPlane;


    }
}
