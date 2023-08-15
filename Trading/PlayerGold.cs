using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 *  This class controls the player's gold
 */

public class PlayerGold : MonoBehaviour
{
    // Singleton for accessing player's gold for trading
    #region Singleton

    public static PlayerGold Instance { get; private set; } = null!;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    #endregion

    [SerializeField] private GameObject goldUI = null!;

    public int gold = 0;
    private bool addedGold = false;

    // Increase player's gold over time
    private void Update()
    {
        showGold();

        while (!addedGold)
            StartCoroutine(addPassiveGold());
    }

    public void addGold(int amount)
    {
        gold += amount;
    }

    private IEnumerator addPassiveGold()
    {
        addedGold = true;
        yield return new WaitForSeconds(2);
        gold += 1;
        addedGold = false;
    }

    private void showGold()
    {
        Text goldText = GameObject.Find("Gold Text").GetComponent<Text>();
        goldText.text = gold.ToString() + "g";
    }
}
