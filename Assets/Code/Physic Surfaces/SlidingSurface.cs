using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlidingSurface : MonoBehaviour
{
    [Header("Configuration Sliding")]
    public float slideMultiplier = 1.5f;
    public float dragWhileSliding = 0.1f; 
    public float minVelocityToSlide = 0.2f;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        if (collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Turret"))
        {
            Vector3 velocity = rb.linearVelocity;
            if (velocity.magnitude < minVelocityToSlide) return;
            Vector3 horizontalDir = new Vector3(velocity.x, 0, velocity.z).normalized;
            rb.AddForce(horizontalDir * slideMultiplier, ForceMode.Acceleration);
            rb.linearDamping = dragWhileSliding;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;
        rb.linearDamping = 0f;
    }
}


