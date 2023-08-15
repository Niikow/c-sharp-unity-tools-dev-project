using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class controls the player's respawn behaviour
 */

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform
        player = null!,
        respawnPoint = null!;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            playerStats.Die();
        }
    }
}
