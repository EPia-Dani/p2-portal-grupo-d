using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    [Header("Refs")]
    public Transform core;
    public LineRenderer lineRenderer;
    public LayerMask collisionMask = 3;

    [Header("Configuration")]
    public float maxDistance = 100f;

    private bool createRefraction = false;
    private int lastFrameProcessed = -1;  

    void Update()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = createRefraction;

        createRefraction = false;
    }

    public void CreateRefraction(RaycastHit hitInfo)
    {

        if (lastFrameProcessed == Time.frameCount)
            return;

        lastFrameProcessed = Time.frameCount;

        createRefraction = true;

        Vector3 startPos = core.position;
        Vector3 dir = core.forward;

        lineRenderer.SetPosition(0, startPos);

        RaycastHit nextHit;
        if (Physics.Raycast(startPos, dir, out nextHit, maxDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            lineRenderer.SetPosition(1, nextHit.point);

            if (nextHit.collider.CompareTag("Cube"))
            {
                return;
            }

            if (nextHit.collider.CompareTag("RefractionCube"))
            {
                nextHit.collider.GetComponent<RefractionCube>()?.CreateRefraction(nextHit);
                return;
            }

            if (nextHit.collider.CompareTag("LaserReceiver"))
            {
                nextHit.collider.GetComponent<LaserReceiver>()?.ActivateReceiver();
                return;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPos + dir * maxDistance);
        }
    }
}



