using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*   This class controls the UI for the player's health bar
*/
public class HealthManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image healthBar = null!;
    [SerializeField] private PlayerStats playerStats = null!;

    private float maxHealth, currentHealth;

    // Set UI bar to player's base health
    void Start()
    {
        currentHealth = playerStats.Health;
        maxHealth = playerStats.Health.Max;
    }

    // Update UI when player's health changes
    void Update()
    {
        currentHealth = playerStats.Health;
        maxHealth = playerStats.Health.Max;

        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
