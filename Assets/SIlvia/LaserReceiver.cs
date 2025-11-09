using UnityEngine;
using UnityEngine.Events;

public class LaserReceiver : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onLaserReceived;    
    public UnityEvent onLaserLost;      

    [Header("Settings")]
    public float laserTimeout = 0.2f; 

    private bool isReceiving = false;
    private float lastHitTime = 0f;

    void Update()
    {
        if (isReceiving && Time.time - lastHitTime > laserTimeout)
        {
            isReceiving = false;
            onLaserLost.Invoke();
        }
    }

    public void ActivateReceiver()
    {
        if (!isReceiving)
        {
            isReceiving = true;
            onLaserReceived.Invoke();
        }

        lastHitTime = Time.time;
    }
}

