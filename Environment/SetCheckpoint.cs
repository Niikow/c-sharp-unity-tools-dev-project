using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for setting the respawn point
 */

public class SetCheckpoint : MonoBehaviour
{
    // Set the respawn point when player touches the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("touching player");
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.SetRespawnPoint(transform);
            }
        }
    }
}
