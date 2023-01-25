using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject respawnPosition;

    private Image healthBar;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 150.0f;
    Health player;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        player = FindObjectOfType<Health>();
        
    }

    private void FixedUpdate()
    {
        ResetHealth();
        currentHealth = player.health;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Als de player de muur raakt, krijgt hij 10 damage
        if (other.tag == "Wall")
            currentHealth -= 10;
    }

    private void ResetHealth()
    {
        if (currentHealth < 1)
        {
            // Als de Health van de speler kleiner is dan 1, respawned hij op de respawnPosition gameObject
            gameObject.transform.position = respawnPosition.transform.position;
            // Na het respawnen krijgt hij weer maxHealth
            currentHealth = maxHealth;
        }
    }
}
