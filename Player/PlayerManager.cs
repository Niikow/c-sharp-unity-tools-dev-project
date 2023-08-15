using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Class for keeping track of player for enemy AI
 */
public class PlayerManager : MonoBehaviour
{
    // Singleton for accessing player position from other scripts
    #region Singleton

    public static PlayerManager Instance { get; private set; } = null!;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    #endregion

    // Player object set manually in editor
    public GameObject player = null!;

    [SerializeField]
    private bool isInCombatRange = false;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool isInCombatRangeTemp = false;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

            if (distance <= 4f)
            {
                isInCombatRangeTemp = true;
            }
        }

        if (isInCombatRangeTemp)
        {
            isInCombatRange = true;
            AudioManager.Instance.StopSound("Background Music");
            AudioManager.Instance.PlaySound("Combat Music");
        }
        else
        {
            bool isPlayerOutOfCombatRange = true;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (distance <= 4f)
                {
                    isPlayerOutOfCombatRange = false;
                    break;
                }
            }

            if (isPlayerOutOfCombatRange)
            {
                isInCombatRange = false;
                AudioManager.Instance.StopSound("Combat Music");
                AudioManager.Instance.PlaySound("Background Music");
            }
        }
    }

    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
