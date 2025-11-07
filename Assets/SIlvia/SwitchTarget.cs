using UnityEngine;

public class SwitchTarget : MonoBehaviour
{
    public GameObject door;

    public void ActivateSwitch()
    {
        Debug.Log("Interruptor activado!");
        if (door != null)
            door.GetComponent<Animator>()?.SetTrigger("Open");
    }
}
