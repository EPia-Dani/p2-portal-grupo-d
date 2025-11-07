using NUnit.Framework.Constraints;
using Unity.Cinemachine;
//using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Analytics;

public class Portal : MonoBehaviour
{
    public Camera playerCamera;
    public Portal otherPortal;
    public Camera reflectionCamera;
    [SerializeField] private Transform reflectionTransform;
    public float effectNearPlane=-0.5f;
    public GameObject wall;
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
     
        float distance = Vector3.Distance( otherPortal.reflectionCamera.transform.position, otherPortal.transform.position );
        otherPortal.reflectionCamera.nearClipPlane= Mathf.Max(0.0f,distance)+effectNearPlane;
    }

    public void OnNewPortalPosition(GameObject newWall)    // TODO: attempts to create a new portal in a wall
    {
        if (newWall != null)    //TODO: call a method to check if it's possible to create a portal
        {
            wall=newWall;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Disable the collider of the wall when the player enters the portal trigger
        if (wall != null)
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            if (wallCollider != null)
                wallCollider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (wall != null)
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            if (wallCollider != null)
                wallCollider.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        

        Transform playerTransform = other.transform;

        Plane portalPlane = new Plane(transform.forward, transform.position);

        float distance = portalPlane.GetDistanceToPoint(playerTransform.position);

        if (distance < 0.0f)
        {
            
            TeleportPlayer(GameObject.FindGameObjectWithTag("Player"));
        }
        
    }
    void TeleportPlayer(GameObject player)
    {
        /*
        // convert player position to local space of this portal
        Vector3 localPos = transform.InverseTransformPoint(player.transform.position);
        localPos.z = -localPos.z;

        // transform to world space of the other portal
        Vector3 newWorldPos = otherPortal.transform.TransformPoint(localPos);

        // apply +0.5f offset forward from the other portal
        //newWorldPos += otherPortal.transform.forward * 0.5f;

        player.GetComponent<CharacterController>().Move(newWorldPos);
        //player.position = newWorldPos;

        // mirror rotation
        Vector3 localDir = transform.InverseTransformDirection(player.transform.forward);
        localDir.z = -localDir.z;
        player.transform.forward = otherPortal.transform.TransformDirection(localDir);
        */

        Vector3 l_Position = reflectionTransform.transform.InverseTransformPoint(player.transform.position);
        l_Position.z += 0.5f;
        l_Position.z= -l_Position.z;
        Vector3 l_Direction =reflectionTransform.transform.InverseTransformDirection(-player.transform.forward);

        Vector3 targetPosition= l_Position+player.transform.position;
        player.GetComponent<CharacterController>().Move(otherPortal.transform.TransformPoint(targetPosition));
        //player.transform.position=(otherPortal.transform.TransformPoint(l_Position));

    }
}
