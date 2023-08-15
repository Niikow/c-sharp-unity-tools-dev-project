using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoManager : MonoBehaviour
{
    // Singleton for accessing item info from other scripts
    #region Singleton

    public static ItemInfoManager Instance { get; set; } = null!;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    #endregion

    public GameObject equipmentInfoPanel = null!, potionInfoPanel = null!;

    // Set item and potion info panels to inactive
    private void Start()
    {
        equipmentInfoPanel.SetActive(false);
        potionInfoPanel.SetActive(false);
    }

    // Show item info panel
    public void showEquipmentInfo()
    {
        equipmentInfoPanel.SetActive(true);
    }

    // Show potion info panel
    public void showPotionInfo()
    {
        potionInfoPanel.SetActive(true);
    }

    // Close item info panel
    public void closeEquipmentInfo()
    {
        equipmentInfoPanel.SetActive(false);
    }

    // Close potion info panel
    public void closePotionInfo()
    {
        potionInfoPanel.SetActive(false);
    }
}
