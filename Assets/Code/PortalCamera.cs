using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Analytics;

public class teleportCamera : MonoBehaviour
{

    
    GameObject playerCamera;
    teleportCamera virtual_portal;
    public Camera reflectionCamera;
    Transform reflectionTransform;
    float effectNearPlane;
    void Start(){
        playerCamera = GameObject.FindGameObjectsWithTag("playerCamera");
    }
    void FixedUpdate(){

        Vector3 worldPosition = playerCamera.transform.position;
        Vector3 localPosition =  reflectionTransform.InverseTransformPoint(worldPosition);
        virtual_portal.reflectionCamera.transform.position = virtual_portal.transform.TransformPoint(localPosition);

        Vector3 worldDirection = playerCamera.transform.forward;
        Vector3 localDirection = reflectionTransform.InverseTransformPoint(worldDirection);
        virtual_portal.reflectionTransform.transform.forward= virtual_portal.transform.TransformPoint(localDirection);

        float distance = Vector3.Distance( virtual_portal.reflectionTransform.transform.position, virtual_portal.reflectionTransform.position );
        virtual_portal.reflectionCamera.nearClipPlane= Mathf.Max(0.0f,distance)+effectNearPlane;


    }
}
