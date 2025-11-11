using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("UI Message")]
    public TMP_Text messageUI;
    public float messageDuration = 2f;

    [Header("Sound")]
    public AudioClip saveSound;
    public float volume = 1f;

    private bool activated = false;
    private bool showingMessage = false;
    private float remainingMessageTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetCheckpoint(transform.position);
            }

            ShowMessage();

            if (saveSound != null)
                AudioSource.PlayClipAtPoint(saveSound, transform.position, volume);
        }
    }

    private void Update()
    {
        if (showingMessage)
        {
            remainingMessageTime -= Time.deltaTime;

            if (remainingMessageTime <= 0f)
            {
                HideMessage();
            }
        }
    }

    private void ShowMessage()
    {
        if (messageUI != null)
        {
            messageUI.text = "Checkpoint saved!";
            messageUI.gameObject.SetActive(true);
            showingMessage = true;
            remainingMessageTime = messageDuration;
        }
    }

    private void HideMessage()
    {
        if (messageUI != null)
        {
            messageUI.gameObject.SetActive(false);
        }
        showingMessage = false;
    }
}
