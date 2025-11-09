using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public float maxDistance = 100f;
    public LayerMask collisionMask = ~0;
    public LineRenderer lineRenderer;

    void Update()
    {
        if (!lineRenderer) return;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Cube"))
                return;

            if (hit.collider.CompareTag("RefractionCube"))
            {
                hit.collider.GetComponent<RefractionCube>()?.CreateRefraction(hit);
                return;
            }

            if (hit.collider.CompareTag("LaserReceiver"))
            {
                hit.collider.GetComponent<LaserReceiver>()?.ActivateReceiver();
                return;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
        }
    }
}


