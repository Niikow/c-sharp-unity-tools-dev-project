using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Class for spawning platforms
 */

public class SpawnPlatform : MonoBehaviour
{
    public GameObject[] platforms = Array.Empty<GameObject>();

    // Disable platforms on start
    void Start()
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
    }

    // Spawn platforms when player touches the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            Debug.Log("touched player");
            foreach (GameObject platform in platforms)
            {
                platform.SetActive(true);
            }

            StartCoroutine(DespawnPlatform());
        }
    }

    // Despawn platforms after 10 seconds
    private IEnumerator DespawnPlatform()
    {
        yield return new WaitForSeconds(10f);

        foreach (var platform in platforms)
        {
            platform.SetActive(false);
        }
    }
}
