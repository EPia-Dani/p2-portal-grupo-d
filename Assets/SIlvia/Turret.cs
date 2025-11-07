using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody))]
public class Turret : MonoBehaviour
{
    [Header("Laser Settings")]
    public LineRenderer m_LineRenderer;
    public float m_MaxDistance = 20f;
    public LayerMask m_CollisionLayerMask;
    public float m_MaxAngle = 45f;

    [Header("Tracking")]
    public Transform player;
    public float rotationSpeed = 4f;

    private Rigidbody rb;
    private bool hasFallen = false;
    private bool m_LaserActive = true;
    public float detectionRange = 15f;

    public LayerMask visionMask;
    public float fireRange = 20f;
    public bool canSeePlayer = false;

    public AudioSource fallSound;

    public ParticleSystem sparks;
    public ParticleSystem smoke;

    public float laserDamage = 10f;

    private void Start()
    {
        if (m_LineRenderer == null)
            m_LineRenderer = GetComponentInChildren<LineRenderer>();

        if (m_LineRenderer == null)
        {
            enabled = false;
            return;
        }

        m_LineRenderer.enabled = true;
        m_LineRenderer.positionCount = 2;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = true;

        if (sparks != null) sparks.Stop();
        if (smoke != null) smoke.Stop();
    }

    private void Update()
    {
        if (!hasFallen && HasLineOfSight() && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            RotateToPlayer();
        }

        if (!hasFallen)           
            RotateToPlayer();

        CheckTurretAngle();

        if (m_LaserActive && !hasFallen)
            UpdateLaser();
        else
            m_LineRenderer.enabled = false;
    }

    private void CheckTurretAngle()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);

        if (!hasFallen && angle > m_MaxAngle)
        {
            hasFallen = true;
            rb.isKinematic = false;
            m_LaserActive = false;

            if (sparks != null && !sparks.isPlaying) sparks.Play();
            if (smoke != null && !smoke.isPlaying) smoke.Play();

            if (fallSound != null && !fallSound.isPlaying) fallSound.Play();
        }
    }


    private void UpdateLaser()
    {
        m_LineRenderer.enabled = true;

        Vector3 startPos = m_LineRenderer.transform.position;
        Vector3 forward = m_LineRenderer.transform.forward;
        RaycastHit hit;
        Vector3 endPos;

        if (!canSeePlayer && !Physics.Raycast(new Ray(startPos, forward), m_MaxDistance, m_CollisionLayerMask))
        {
            m_LineRenderer.enabled = false;
            return;
        }

        if (Physics.Raycast(new Ray(startPos, forward), out hit, m_MaxDistance, m_CollisionLayerMask))
        {
            endPos = startPos + forward * hit.distance;
        }

        else
        {
            endPos = startPos + forward * m_MaxDistance;
        }

        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos);

        if (hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(laserDamage * Time.deltaTime);
        }
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

    private void RotateToPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        float angleToPlayer = Vector3.Angle(transform.forward, direction);
        if (angleToPlayer > 90f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasFallen) return;

        if (collision.collider.CompareTag("Player"))
        {
            hasFallen = true;
            rb.isKinematic = false;
            m_LaserActive = false;
        }

        if (fallSound != null) fallSound.Play();
    }

    private bool HasLineOfSight()
    {
        Vector3 origin = m_LineRenderer.transform.position;
        Vector3 dir = (player.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, fireRange, visionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
                return true;
            }
        }

        canSeePlayer = false;
        return false;
    }

}


