using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Analytics;

public class Portal : MonoBehaviour
{
    private Camera playerCamera;
    public Portal otherPortal;
    [SerializeField] private Camera reflectionCamera;
    [SerializeField] private Transform reflectionTransform;
    public float effectNearPlane=-0.5f;
    public GameObject wall;
    private List<Collider> ignoredColliders = new List<Collider>();

    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        foreach (Collider collider in ignoredColliders)
        {
            Physics.IgnoreCollision(wall.GetComponent<Collider>(), collider, false);

        }
    }


    void LateUpdate()
    {
        if (otherPortal != null && wall != null)
        {
            Vector3 worldPosition = playerCamera.transform.position;
            Vector3 localPosition = reflectionTransform.InverseTransformPoint(worldPosition);
            localPosition.z = -localPosition.z;
            localPosition.x = -localPosition.x;
            otherPortal.reflectionCamera.transform.position = otherPortal.transform.TransformPoint(localPosition);

            Vector3 worldDirection = playerCamera.transform.forward;
            Vector3 localDirection = reflectionTransform.InverseTransformDirection(worldDirection);
            localDirection.z = -localDirection.z;
            localDirection.x = -localDirection.x;
            otherPortal.reflectionCamera.transform.forward = otherPortal.transform.TransformDirection(localDirection);

            float distance = Vector3.Distance(otherPortal.reflectionCamera.transform.position, otherPortal.transform.position);
            otherPortal.reflectionCamera.nearClipPlane = Mathf.Max(0.0f, distance) + effectNearPlane;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (otherPortal == null || wall == null) return;

        Collider wallCollider = wall.GetComponent<Collider>();
        Collider targetCollider = other.GetComponent<Collider>();

        if (wallCollider == null || targetCollider == null) return;

        if (other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            Physics.IgnoreCollision(wallCollider, targetCollider, true);
            ignoredColliders.Add(targetCollider);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (otherPortal == null || wall == null) return;

        Collider wallCollider = wall.GetComponent<Collider>();
        Collider targetCollider = other.GetComponent<Collider>();

        if (wallCollider == null || targetCollider == null) return;

        if (other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            Physics.IgnoreCollision(wallCollider, targetCollider, false);
            ignoredColliders.Remove(targetCollider);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (otherPortal==null) return;


        if (other.CompareTag("Player")) {
            Transform playerTransform = other.transform;

            Plane portalPlane = new Plane(transform.forward, transform.position);

            float distance = portalPlane.GetDistanceToPoint(other.transform.position);


            if (distance < 0f)
            {

                PortalEvents.RaisePlayerTeleported(this, otherPortal, other.gameObject);
                Physics.IgnoreCollision(wall.GetComponent<Collider>(), other, false);
                ignoredColliders.Remove(other);
            }


        }
        else if (other.CompareTag("Cube"))
        {            
            Transform playerTransform = other.transform;

            Plane portalPlane = new Plane(transform.forward, transform.position);

            float distance = portalPlane.GetDistanceToPoint(other.transform.position); 


            if (distance < 0f)
            {
                PortalEvents.RaiseCubeTeleported(this, otherPortal, other.gameObject);
                Physics.IgnoreCollision(wall.GetComponent<Collider>(), other, false);
                ignoredColliders.Remove(other);
            }
        }

    }
    public void setWall(GameObject newWall) { wall = newWall; } 
    public void setOtherPortal(GameObject newOtherPortal) { 
        otherPortal = newOtherPortal.GetComponent<Portal>();
    }
}
