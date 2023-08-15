using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class controls the UI slot for the selected item in trade menu
 */

public class SelectedItemSlot : MonoBehaviour
{
    // Singleton for accessing selected item from other scripts
    #region Singleton

    public static SelectedItemSlot Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    #endregion

    public Item? item;
    public UnityEngine.UI.Image icon = null!;

    public TradeType tradeType, lastTradeType;

    // Add item to sell/buy to item slot
    public void addItem(Item newItem)
    {
        // 0 = buying from | 1 = selling to
        if ((int)tradeType == 0)
        {
            if (item != null)
                TraderInventory.Instance.items.Add(item);

        }
        else if ((int)tradeType == 1)
        {
            if (item != null)
                Inventory.Instance.items.Add(item);
        }

        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    // Remove item from item slot
    public void clearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // Return item to trader inventory or player inventory
    // 0 = buying from | 1 = selling to (enum)
    public void returnItem()
    {
        if ((int)tradeType == 0)
        {
            if (item != null)
            {
                TraderInventory.Instance.items.Add(item);
                TraderInventory.Instance.OnItemChanged.Invoke();
                Debug.Log($"{item.name} added to trader inventory");
            }
        }
        else if ((int)tradeType == 1)
        {
            if (item != null)
            {
                Inventory.Instance.items.Add(item);
                Inventory.Instance.OnItemChanged.Invoke();
                Debug.Log($"{item.name} added to player inventory");
            }
        }

        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // Display selected item's info when hovered over
    public void displayItemInfo()
    {

        Text
            nameText,
            healthText,
            damageText,
            staminaText,
            potionNameText,
            healText,
            worthText;

        if (item is Equipment equipment)
        {
            ItemInfoManager.Instance.showEquipmentInfo();

            nameText = GameObject.Find("Item Info Name Text").GetComponent<Text>();
            healthText = GameObject.Find("Info Health Text").GetComponent<Text>();
            damageText = GameObject.Find("Info Damage Text").GetComponent<Text>();
            staminaText = GameObject.Find("Info Stamina Text").GetComponent<Text>();
            worthText = GameObject.Find("Info Gold Equipment Text").GetComponent<Text>();

            nameText.text = equipment.name;
            healthText.text = equipment.healthModifier.ToString();
            damageText.text = equipment.damageModifier.ToString();
            staminaText.text = equipment.staminaModifier.ToString();
            worthText.text = equipment.itemValue.ToString();
        }
        else if (item is Potion potion)
        {
            ItemInfoManager.Instance.showPotionInfo();

            potionNameText = GameObject.Find("Potion Info Name Text").GetComponent<Text>();
            healText = GameObject.Find("Info Heal Text").GetComponent<Text>();
            worthText = GameObject.Find("Info Gold Potion Text").GetComponent<Text>();

            potionNameText.text = potion.name;
            healText.text = potion.healModifier.ToString();
            worthText.text = potion.itemValue.ToString();
        }

    }

    // Stop displaying item info when stop hovering over item slot
    public void closeDisplayItemInfo()
    {

        ItemInfoManager.Instance.closeEquipmentInfo();
        ItemInfoManager.Instance.closePotionInfo();
    }
}

// Enum for trade type
public enum TradeType { buy, sell }
