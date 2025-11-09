using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 lastCheckpointPos;
    private GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastCheckpointPos = player.transform.position;
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPos = position;
    }

    public void RespawnPlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = lastCheckpointPos;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }
}


