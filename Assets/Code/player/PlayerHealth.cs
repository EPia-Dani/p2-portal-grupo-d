using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public System.Action PlayerDied;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.GetComponent<FPS_Controller>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        GameManager.instance.GameOver();
        PlayerDied?.Invoke();
    }
}

