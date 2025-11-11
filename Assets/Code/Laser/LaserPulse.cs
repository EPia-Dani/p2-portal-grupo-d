using UnityEngine;

public class LaserPulse : MonoBehaviour
{
    public LineRenderer line;
    public float pulseSpeed = 4f;
    public float minWidth = 0.02f;
    public float maxWidth = 0.05f;

    void Update()
    {
        if (line == null) return;
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        line.startWidth = Mathf.Lerp(minWidth, maxWidth, pulse);
        line.endWidth = line.startWidth;
    }
}

