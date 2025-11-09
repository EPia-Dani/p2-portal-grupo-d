using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    [Header("Settings Laser")]
    public float maxDistance = 100f;
    public LayerMask collisionMask;
    public LineRenderer lineRenderer;

    void Update()
    {
        DispararLaser();
    }

    private void DispararLaser()
    {
        if (lineRenderer == null) return;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, collisionMask))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("RefractionCube"))
            {
                hit.collider.GetComponent<RefractionCube>()?.CreateRefraction(hit);
            }

            else if (hit.collider.CompareTag("LaserReceiver"))
            {
                hit.collider.GetComponent<LaserReceiver>()?.ActivateReceiver();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
        }
    }
}

