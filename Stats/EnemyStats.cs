using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [SerializeField] private bool isBoss;

    private Vector3 respawnPosition;
    public int respawnHealth = 50, goldDropped;

    // Added ? to the EquipmentManager because it may be null
    // notall enemies will have an inventory
    public EquipmentManager? inventory;


    private void Start()
    {
        if (inventory != null) Register(inventory); // this made the compiler give a warning here which forced me to add a null check
        respawnPosition = gameObject.transform.position;
        Health.Value = Health.Base;
    }

    // Called when enemy health reaches 0
    public override void Die()
    {
        base.Die();
        if (isBoss) UnityEditor.EditorApplication.ExitPlaymode();
        gameObject.SetActive(false);
        PlayerGold.Instance.addGold(goldDropped);
        Invoke("Respawn", 10f);
    }

    // Reset the enemy
    public void Respawn()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = respawnPosition;
        Health.Value = respawnHealth;
    }
}
