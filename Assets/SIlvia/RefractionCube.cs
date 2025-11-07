using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RefractionCube : MonoBehaviour
{
    public LayerMask m_CollisionLayerMask;
    public bool m_CreateRefraction = false;

    private LineRenderer m_LineRenderer;

    private void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
        m_LineRenderer.useWorldSpace = true;
        m_LineRenderer.enabled = true;
    }

    private void Update()
    {
        m_LineRenderer.enabled = true;
        m_LineRenderer.enabled = m_CreateRefraction;
        m_CreateRefraction = false;
    }

    public void CreateRefraction(Vector3 incomingDir = default)
    {
        m_CreateRefraction = true;

        Vector3 startPos = transform.position;
        Vector3 direction = (incomingDir == Vector3.zero) ? transform.forward : incomingDir;
        Vector3 endPos = startPos + direction;

        RaycastHit hit;

        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos);


        if (Physics.Raycast(startPos, direction, out hit, m_CollisionLayerMask.value))
        {
            endPos = hit.point;

          
            if (hit.collider.CompareTag("RefractionCube"))
            {
                
                Vector3 reflectDir = Vector3.Reflect(direction, hit.normal);
                hit.collider.GetComponent<RefractionCube>()?.CreateRefraction(reflectDir);
            }
            
            else if (hit.collider.CompareTag("SwitchTarget"))
            {
                hit.collider.GetComponent<SwitchTarget>()?.ActivateSwitch();
            }
        }
    }
}


