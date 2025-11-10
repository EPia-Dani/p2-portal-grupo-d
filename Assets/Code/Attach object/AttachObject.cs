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

    //detect both layers

   [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private LayerMask turretLayer;
    private LayerMask combinedMask; 

    private FPS_Controller playerController;

    void Start()
    {
            playerController = GetComponentInParent<FPS_Controller>();

    }
    public void OnCatch(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        if (!holding)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCatchDistance))
            {
                Debug.Log(hit.collider.gameObject.tag);
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
            Debug.Log("Trying to pick up something");
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

        if (holding||attachObject!=null)
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
            }

            attachObject= null;
            holding = false;
            

        }

    }
}
