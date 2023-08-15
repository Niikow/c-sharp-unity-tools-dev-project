/*
 * Child class of "Stats" for updating player stats depending on gear
 */

using System;
using UnityEngine;
using System.Linq;


public class PlayerStats : Stats
{
    #region Singleton

    public static PlayerStats Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    #endregion

    [SerializeField] private Transform player = null!; // same thing, could be null, we can silence it like this
    [SerializeField] private Transform respawnPoint = null!;
    [SerializeField] private UnityEngine.UI.Image healthBar = null!;

    private void Start()
    {
        Register(EquipmentManager.Instance);
        // EquipmentManager.instance.onEquipmentChanged += onEquipmentChanged;
    }

    void Update() //can ignore this Update code it was for testing purposes
    {
        if (Input.GetKeyDown(KeyCode.T)) DamageTaken(10);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Player Health: " + Health);
            Debug.Log("Player Max Health: " + Health.Max);

            Debug.Log("Player Armour: " + Armour);
            Debug.Log("Player Max Armour: " + Armour.Max);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(50);
        }
    }

    // Player death when health reaches 0
    public override void Die()
    {
        base.Die();

        FindObjectOfType<AudioManager>().PlaySound("Player Death");
        gameObject.SetActive(false);
        player.transform.position = respawnPoint.transform.position;
        Physics.SyncTransforms();
        Invoke("setActive", 5f);

        // PlayerManager.instance.restartScene();
    }

    // Reset the player
    public void setActive()
    {
        Health.Value = Health.Base;
        gameObject.SetActive(true);
    }

    // Update the spawn point
    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    // Healing potion used by the player to increase health
    public void Heal(int heal)
    {
        // Max health cap already defined in Stats.cs
        Health.Value += heal;
    }


}
