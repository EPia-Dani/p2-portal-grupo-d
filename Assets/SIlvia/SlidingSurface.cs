using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SlidingSurface : MonoBehaviour
{
    [Header("Configuraci� del lliscament")]
    [Tooltip("Redueix la fricci� de l'objecte mentre �s sobre aquesta superf�cie")]
    public float slideFriction = 0.05f;

    [Tooltip("Velocitat m�nima perqu� l'objecte segueixi lliscant")]
    public float minSlideSpeed = 0.2f;

    [Tooltip("Factor de manteniment de velocitat (1 = sense p�rdua)")]
    public float slidePreserveFactor = 0.98f;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        if (collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Turret"))
        {
            Vector3 velocity = rb.linearVelocity;

            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

            if (horizontalVelocity.magnitude > minSlideSpeed)
            {
                horizontalVelocity *= slidePreserveFactor;

                rb.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
            }

            rb.linearDamping = slideFriction;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;
        rb.linearDamping = 0f;
    }
}

