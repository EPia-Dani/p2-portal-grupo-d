using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("UI del mensaje")]
    public TMP_Text mensajeUI;
    public float duracionMensaje = 2f;

    private bool activado = false;
    private bool mostrandoMensaje = false;
    private float tiempoRestanteMensaje = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.CompareTag("Player"))
        {
            activado = true;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetCheckpoint(transform.position);
            }

            MostrarMensaje();
        }
    }

    private void Update()
    {
        if (mostrandoMensaje)
        {
            tiempoRestanteMensaje -= Time.deltaTime;

            if (tiempoRestanteMensaje <= 0f)
            {
                OcultarMensaje();
            }
        }
    }

    private void MostrarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = "¡Punto de partida guardado!";
            mensajeUI.gameObject.SetActive(true);
            mostrandoMensaje = true;
            tiempoRestanteMensaje = duracionMensaje;
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }
        mostrandoMensaje = false;
    }
}



