using UnityEngine;

public class AttachObject : MonoBehaviour
{
    GameObject attachObject;

    string boxTag = "Cube";
    string turretTag = "Turret";
    public Transform holdPosition;
    private float maxCatchDistance = 20f;
    private bool holding = false;
    private Transform originalParent;

    public void OnCatch()
    {

        if (!holding)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCatchDistance))
            {
                if (hit.collider.CompareTag(boxTag) || hit.collider.CompareTag(turretTag)) {

                    PickUp(hit.collider.gameObject);

                }
            }

        }
        else
        {
            Drop();
        }
    }
    private void PickUp(GameObject o)
    {
        Rigidbody rb = o.GetComponent<Rigidbody>();
        if(rb != null)
        {
            originalParent = rb.transform.parent;
            holding = true; //save original parent

            //configure rb
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

            attachObject.transform.SetParent(originalParent, true);
            attachObject.GetComponent<Rigidbody>().isKinematic = false;
            attachObject= null;
            holding= false;

        }

    }
}
