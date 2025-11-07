using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    public LayerMask m_CollisionMask;
    public Color m_LaserColor = Color.red;

    private LineRenderer m_LineRenderer;

    private void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
        m_LineRenderer.useWorldSpace = true;
        m_LineRenderer.startWidth = 0.05f;
        m_LineRenderer.endWidth = 0.05f;
        m_LineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        m_LineRenderer.material.color = m_LaserColor;
    }

    private void Update()
    {
        Vector3 start = transform.position;
        Vector3 dir = transform.forward;
        Vector3 end = start + dir;

        RaycastHit hit;

        m_LineRenderer.SetPosition(0, start);
        m_LineRenderer.SetPosition(1, end);


        if (Physics.Raycast(start, dir, out hit, m_CollisionMask.value))
        {
            end = hit.point;

            if (hit.collider.CompareTag("RefractionCube"))
            {
                RefractionCube cube = hit.collider.GetComponent<RefractionCube>();
                if (cube != null)
                {

                    Vector3 reflectDir = Vector3.Reflect(dir, hit.normal);
                    cube.CreateRefraction(reflectDir);
                }
            }

            else if (hit.collider.CompareTag("SwitchTarget"))
            {
                hit.collider.GetComponent<SwitchTarget>()?.ActivateSwitch();
            }
        }
    }
}
