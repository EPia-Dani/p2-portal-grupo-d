using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    [Header("Evento al activar el botón")]
    public UnityEvent m_Event; 

    [Header("Configuración del botón")]
    public string triggerTag = "Cube"; 
    public bool onlyOnce = false;      

    private bool activated = false;

    private void OnTriggerEnter(Collider _Collider)
    {
        if ((triggerTag == "" || _Collider.CompareTag(triggerTag)) && (!onlyOnce || !activated))
        {
            m_Event.Invoke();  
            activated = true;    
        }
    }
}

