using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    [Header("Referències")]
    public Transform core;            
    public LineRenderer lineRenderer;
    public LayerMask collisionMask;

    [Header("Configuració")]
    public float maxDistance = 100f;

    private bool createRefraction = false;

    void Update()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = createRefraction;

        createRefraction = false;
    }

    public void CreateRefraction(RaycastHit hitInfo)
    {

        createRefraction = true;

        Vector3 startPos = core.position;
        Vector3 dir = core.forward;

        lineRenderer.SetPosition(0, startPos);

        RaycastHit nextHit;
        if (Physics.Raycast(startPos, dir, out nextHit, maxDistance, collisionMask))
        {
            lineRenderer.SetPosition(1, nextHit.point);

            if (nextHit.collider.CompareTag("RefractionCube"))
            {
                nextHit.collider.GetComponent<RefractionCube>()?.CreateRefraction(nextHit);
            }
            else if (nextHit.collider.CompareTag("LaserReceiver"))
            {
                nextHit.collider.GetComponent<LaserReceiver>()?.ActivateReceiver();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPos + dir * maxDistance);
        }
    }
}

