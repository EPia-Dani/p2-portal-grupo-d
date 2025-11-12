using UnityEngine;
using UnityEngine.InputSystem;

public class AttachObject : MonoBehaviour
{
    GameObject attachObject;

    string boxTag = "Cube";
    string turretTag = "Turret";
    public Transform holdPosition;
    private float maxCatchDistance = 20f;
    private bool holding = false;
    private Transform originalParent;

    private float forceMulti = 3f;
    
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private LayerMask turretLayer;
    private LayerMask combinedMask;

    [SerializeField] private Camera playerCamera;




    private FPS_Controller playerController;

    void Start()
    {
        playerController = GetComponentInParent<FPS_Controller>();
        combinedMask = cubeLayer | turretLayer;
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();

    }
    public void OnCatch(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        if (!holding)
        {
            //Debug.LogWarning("Action Call E");
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * maxCatchDistance, Color.red, 1f);

            RaycastHit hit;
            if (Physics.SphereCast(ray,0.3f, out hit, maxCatchDistance,combinedMask))
            {
                //Debug.Log(hit.collider.gameObject.tag);
                //Debug.Log("Raycast throw");
                if (hit.collider.CompareTag(boxTag) || hit.collider.CompareTag(turretTag))
                {

                    PickUp(hit.collider.gameObject);

                }
            }

        }
        else if (attachObject != null)
        {
            Drop();
        }

    }
    private void PickUp(GameObject o)
    {
        Rigidbody rb = o.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Debug.Log("picking up something");
            originalParent = rb.transform.parent;
            holding = true; //save original parent

            //configure rb
            attachObject = o;
            rb.linearVelocity  = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.transform.SetParent(holdPosition, worldPositionStays: false); // add to holdPosition
            rb.transform.localPosition = Vector3.zero;
            rb.transform.localRotation = Quaternion.identity;

            rb.isKinematic = true; //don't move the objecet

        }


    }
    private void Drop()
    {

        HandleCubeExpulsion(false);

    }

    public void Throw(InputAction.CallbackContext context)
    {
        HandleCubeExpulsion(true);

    }
    private void HandleCubeExpulsion(bool throwObject)
    {
        
        if (holding || attachObject != null)
        {
            if (originalParent != null)
            {
                attachObject.transform.SetParent(originalParent, true);
            }
            else
            {
                attachObject.transform.SetParent(null);
            }
            Rigidbody rb = attachObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            Vector3 playerVelocity = Vector3.zero;

            if (playerController != null)
            {
                playerVelocity = playerController.GetVelocity();
                rb.linearVelocity = playerVelocity;
                if (throwObject)
                {
                    rb.AddForce(playerController.transform.forward*forceMulti, ForceMode.Impulse);
                }
            }

            attachObject = null;
            holding = false;


        }

    }

}
