using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Header("Configuration")]
    public CubeSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            if (spawner != null)
                spawner.Unregister(other.gameObject);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Turret"))
        {

            Turret turret = other.GetComponent<Turret>();
            if (turret != null)
            {
                if (turret.GetComponent<LineRenderer>())
                    turret.GetComponent<LineRenderer>().enabled = false;

                if (turret.smoke != null)
                    turret.smoke.Stop();

                if (turret.sparks != null)
                    turret.sparks.Stop();
            }

            Destroy(other.gameObject);
        }
    }
}

