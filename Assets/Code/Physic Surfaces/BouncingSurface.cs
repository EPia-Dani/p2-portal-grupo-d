using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncingSurface : MonoBehaviour
{
    [Header("Configuraci� del rebot")]
    public float bounceForce = 10f;
    public bool reflectDirection = true; 

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return; 

        if (collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Turret"))
        {
            Vector3 bounceDir;

            if (reflectDirection)
                bounceDir = Vector3.Reflect(collision.relativeVelocity.normalized, collision.contacts[0].normal);
            else
                bounceDir = collision.contacts[0].normal; 

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(bounceDir * bounceForce, ForceMode.VelocityChange);
        }
    }
}
