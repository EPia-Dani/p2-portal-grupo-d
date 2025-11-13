using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Analytics;

public class Portal : MonoBehaviour
{
    private Camera playerCamera;
    public Portal otherPortal;
    [SerializeField] public Camera reflectionCamera;
    [SerializeField] private Transform reflectionTransform;
    public float effectNearPlane = -0.5f;
    
    private float scale = 1f;
    public GameObject wall;
    protected List<Collider> ignoredColliders = new List<Collider>();
    bool otherPortalCollisionsEnabled;
    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }

    void OnEnable()
    {
        PortalEvents.OnPortalTriggered += OnOtherPortalTriggered;
        PortalEvents.OnPortalUntriggered += OnOtherPortalUntriggered;
    }
    void OnDisable()
    {
        PortalEvents.OnPortalTriggered -= OnOtherPortalTriggered;
        PortalEvents.OnPortalUntriggered -= OnOtherPortalUntriggered;
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



        if (other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            Collider targetCollider = other.GetComponent<Collider>(); 
            if (wallCollider == null || targetCollider == null) return;

            Physics.IgnoreCollision(wallCollider, targetCollider, true);
            //PortalEvents.RaisePortalTriggered(this, other);
            //otherPortal.OnOtherPortalTriggered(this,other);
            ignoredColliders.Add(targetCollider);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (otherPortal == null || wall == null) return;



        if (other.CompareTag("Player") || other.CompareTag("Cube"))
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            Collider targetCollider = other.GetComponent<Collider>();
            if (wallCollider == null || targetCollider == null) return;

            Physics.IgnoreCollision(wallCollider, targetCollider, false);
            //PortalEvents.RaisePortalUntriggered(this, other);
            //otherPortal.OnOtherPortalUntriggered(this,other);
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


            if (distance <= 0.0f)
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

    public void OnOtherPortalTriggered(Portal portal, Collider other)
    {
        //if (portal != this) return;
        otherPortalCollisionsEnabled = true;
        if (otherPortalCollisionsEnabled) { 
                OnTriggerEnter(other);
        otherPortalCollisionsEnabled = false;
    }
    }
    
    public void OnOtherPortalUntriggered(Portal portal, Collider other)
    {
        //if (portal != this) return;
        otherPortalCollisionsEnabled = true;
        if (otherPortalCollisionsEnabled)
        {
            OnTriggerExit(other);
            otherPortalCollisionsEnabled = false;
        }
        }
    public void setWall(GameObject newWall) { wall = newWall; }
    public void setOtherPortal(GameObject newOtherPortal)
    {
        otherPortal = newOtherPortal.GetComponent<Portal>();
    }
    public void setScale(float s)
    {
        scale = s;
    }
    public float getScale()
    {
        return scale;
    }
}
