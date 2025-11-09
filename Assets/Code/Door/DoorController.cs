using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Settings")]
    public float openDistance = 0.75f;
    public float openSpeed = 2f;     

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isOpen = false;

    void Start()
    {
        leftClosedPos = leftDoor.localPosition;
        rightClosedPos = rightDoor.localPosition;

        leftOpenPos = leftClosedPos + Vector3.right * openDistance;
        rightOpenPos = rightClosedPos + Vector3.left * openDistance;
    }

    void Update()
    {
        if (leftDoor == null || rightDoor == null) return;

        leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition,
            isOpen ? leftOpenPos : leftClosedPos,
            Time.deltaTime * openSpeed);

        rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition,
            isOpen ? rightOpenPos : rightClosedPos,
            Time.deltaTime * openSpeed);
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}



