using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("UI del mensaje")]
    public TMP_Text mensajeUI;
    public float duracionMensaje = 2f;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.CompareTag("Player"))
        {
            activado = true;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetCheckpoint(transform.position);
            }

            StartCoroutine(MostrarMensaje());
        }
    }

    private System.Collections.IEnumerator MostrarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = "¡Punto de partida guardado!";
            mensajeUI.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duracionMensaje);

        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }
    }
}


