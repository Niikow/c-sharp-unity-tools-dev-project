using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Class for restarting the game
 */

public class Restart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            Debug.Log("touched player");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
