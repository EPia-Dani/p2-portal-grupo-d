using UnityEngine;

public class LavaZone : MonoBehaviour
{
    public float killDelay = 0.1f;
    public AudioClip splashSound; 

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                if (splashSound != null)
                    AudioSource.PlayClipAtPoint(splashSound, other.transform.position);

                ph.TakeDamage(ph.maxHealth);
            }
        }

       
        if (other.CompareTag("Cube") || other.CompareTag("RefractionCube"))
        {
            Destroy(other.gameObject, killDelay);
        }

     
        if (other.CompareTag("Turret"))
        {
            Turret t = other.GetComponentInParent<Turret>();
            if (t != null)
                t.DisableTurret();
        }
    }
}

