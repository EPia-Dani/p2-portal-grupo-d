using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Analytics;

public class Portal : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private Portal otherPortal;
    [SerializeField] private Camera reflectionCamera;
    [SerializeField] private Transform reflectionTransform;
    public float effectNearPlane=-0.5f;
    public GameObject wall;
    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }


    void LateUpdate()
    {

        if (otherPortal != null&&wall!=null)
        {
            Vector3 worldPosition = playerCamera.transform.position;
            Vector3 localPosition = reflectionTransform.InverseTransformPoint(worldPosition);
            localPosition.z = -localPosition.z;
            otherPortal.reflectionCamera.transform.position = otherPortal.transform.TransformPoint(localPosition);


            Vector3 worldDirection = playerCamera.transform.forward;
            Vector3 localDirection = reflectionTransform.InverseTransformDirection(worldDirection);
            localDirection.z = -localDirection.z;
            otherPortal.reflectionCamera.transform.forward = otherPortal.transform.TransformDirection(localDirection);

            float distance = Vector3.Distance(otherPortal.reflectionCamera.transform.position, otherPortal.transform.position);
            otherPortal.reflectionCamera.nearClipPlane = Mathf.Max(0.0f, distance) + effectNearPlane;
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
        Vector3 l_Position = reflectionTransform.transform.InverseTransformPoint(player.transform.position);
        l_Position.z -= 0.1f;
        l_Position.z= -l_Position.z;
        
        Vector3 l_Direction =reflectionTransform.transform.InverseTransformDirection(-player.transform.forward);
        l_Direction.x=-l_Direction.x;

        Vector3 targetPosition= l_Position+player.transform.position;

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position= otherPortal.transform.TransformPoint(l_Position);
        player.transform.forward = otherPortal.transform.TransformDirection(l_Direction);
        player.GetComponent<CharacterController>().enabled = true;

    }

    public void setWall(GameObject newWall)
    {
        wall=newWall;
    }

    public void setOtherPortal(GameObject newOtherPortal)
    {
        otherPortal = newOtherPortal.GetComponent<Portal>();
    }
}
