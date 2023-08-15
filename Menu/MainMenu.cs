using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *   This class controls the main menu
 */
public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager = null!;
    public void Start()
    {
        Cursor.visible = true;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Switch to the tutorial scene
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit the game
    public void Quit()
    {
        Debug.Log("Stopped game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif
    }
}
