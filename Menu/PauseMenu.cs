using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *   This class controls the pause menu when player presses "esc" key
 */
public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject menuUI = null!;

    private AudioManager audioManager = null!;

    // Disable the pause menu on start
    private void Start()
    {
        menuUI.SetActive(false);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Enable and disable the pause menu when player presses "esc" key
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (GamePaused)
        {
            Resume();

        }
        else
        {
            Pause();
        }
    }

    // Resume the game
    public void Resume()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        // Cursor.visible = false;
    }

    // Return to the main menu
    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Quit the game
    public void Quit()
    {
        Debug.Log("Stopped game");
        // Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif
    }

    // Pause the game
    void Pause()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        Cursor.visible = true;
    }
}
