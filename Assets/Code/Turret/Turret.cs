using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
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
    private AudioSource audioSource;

    private bool hasFallen = false;
    private bool m_LaserActive = true;
    public float detectionRange = 15f;

    public LayerMask visionMask;
    public float fireRange = 20f;
    public bool canSeePlayer = false;
    private bool hasPlayedDetectionSound = false;

    [Header("Sounds")]
    public AudioClip fallSound;
    public AudioClip detectSound;

    [Header("Particles")]
    public ParticleSystem sparks;
    public ParticleSystem smoke;

    public float laserDamage = 10f;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; 

        if (m_LineRenderer == null)
            m_LineRenderer = GetComponentInChildren<LineRenderer>();

        if (m_LineRenderer == null)
        {
            enabled = false;
            return;
        }

        m_LineRenderer.enabled = true;
        m_LineRenderer.positionCount = 2;

        if (sparks != null) sparks.Stop();
        if (smoke != null) smoke.Stop();

        UpdateLaser(); 
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
            DisableTurret();
        }
    }

    private void UpdateLaser()
    {
        if (m_LineRenderer == null) return;

        m_LineRenderer.enabled = true;

        Vector3 startPos = m_LineRenderer.transform.position;
        Vector3 forward = m_LineRenderer.transform.forward;
        RaycastHit hit;
        Vector3 endPos;

        if (!Physics.Raycast(new Ray(startPos, forward), out hit, m_MaxDistance, m_CollisionLayerMask))
        {
            endPos = startPos + forward * m_MaxDistance;
        }
        else
        {
            endPos = startPos + forward * hit.distance;

            if (hit.collider.CompareTag("Player"))
            {
                PlayerHealth ph = hit.collider.GetComponentInParent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(ph.maxHealth);
            }
            else if (hit.collider.CompareTag("Turret"))
            {
                Turret otherTurret = hit.collider.GetComponentInParent<Turret>();
                if (otherTurret != null)
                    otherTurret.DisableTurret();
            }
        }

        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos);
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

        if (collision.collider.CompareTag("Player") ||
            collision.collider.CompareTag("RefractionCube") ||
            collision.collider.CompareTag("Turret"))
        {
            DisableTurret();
        }
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

                
                if (!hasPlayedDetectionSound && detectSound != null)
                {
                    audioSource.clip = detectSound;
                    audioSource.Play();
                    hasPlayedDetectionSound = true;
                }

                return true;
            }
        }

        if (hasPlayedDetectionSound && !canSeePlayer)
            hasPlayedDetectionSound = false;

        canSeePlayer = false;
        return false;
    }

    public void DisableTurret()
    {
        if (hasFallen) return;

        if (fallSound != null)
        {
            audioSource.Stop();
            audioSource.clip = fallSound;
            audioSource.Play();
        }

        hasFallen = true;
        rb.isKinematic = false;
        m_LaserActive = false;
        m_LineRenderer.enabled = false;

        if (sparks != null) sparks.Play();
        if (smoke != null) smoke.Play();
    }
}

