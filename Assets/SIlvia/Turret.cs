using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Turret : MonoBehaviour
{
    [Header("Laser Settings")]
    public LineRenderer m_LineRenderer;       
    public float m_MaxDistance = 20f;         
    public LayerMask m_CollisionLayerMask;   
    public float m_MaxAngle = 45f;       

    private bool m_LaserActive = true;

    private void Start()
    {
        m_LineRenderer = GetComponentInChildren<LineRenderer>();
        m_LineRenderer.enabled = true;
        m_LineRenderer.positionCount = 2;
    }

    private void Update()
    {
        CheckTurretAngle();

        if (m_LaserActive)
            UpdateLaser();
        else
            m_LineRenderer.enabled = false;
    }

    private void CheckTurretAngle()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        m_LaserActive = angle <= m_MaxAngle;
    }

    private void UpdateLaser()
    {
        m_LineRenderer.enabled = true;

        Vector3 startPos = m_LineRenderer.transform.position;
        Vector3 forward = m_LineRenderer.transform.forward;
        RaycastHit hit;
        Vector3 endPos;

        if (Physics.Raycast(new Ray(startPos, forward), out hit, m_MaxDistance, m_CollisionLayerMask))
            endPos = startPos + forward * hit.distance;
        else
            endPos = startPos + forward * m_MaxDistance;

        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos);
    }

    private void OnDrawGizmos()
    {
        if (m_LineRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                m_LineRenderer.transform.position,
                m_LineRenderer.transform.position + m_LineRenderer.transform.forward * m_MaxDistance
            );
        }
    }
}

