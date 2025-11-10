using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    [Header("Configuration Button")]
    public Transform buttonTop;           
    public float pressDepth = 0.2f;        
    public float pressSpeed = 4f;         
    public string[] activatorTags = { "Player", "Cube" }; 

    [Header("Events")]
    public UnityEvent onPressed;            
    public UnityEvent onReleased;         

    private Vector3 initialPos;
    private Vector3 pressedPos;
    private bool isPressed = false;
    private int pressCount = 0; 

    void Start()
    {
        if (buttonTop == null)
            buttonTop = transform; 

        initialPos = buttonTop.localPosition;
        pressedPos = initialPos - new Vector3(0, pressDepth, 0);
    }

    void Update()
    {
        
        Vector3 targetPos = isPressed ? pressedPos : initialPos;
        buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * pressSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActivator(other))
        {
            pressCount++;
            if (!isPressed)
            {
                isPressed = true;
                onPressed?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsActivator(other))
        {
            pressCount--;
            if (pressCount <= 0)
            {
                isPressed = false;
                onReleased?.Invoke();
            }
        }
    }

    private bool IsActivator(Collider other)
    {
        foreach (string tag in activatorTags)
        {
            if (other.CompareTag(tag))
                return true;
        }
        return false;
    }
}
