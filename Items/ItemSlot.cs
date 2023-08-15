using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

/*
 * This class adds functionality to each item frame by displaying the items in inventory
 */
public class ItemSlot : MonoBehaviour
{
    Item? item; // a slot could be empty
    public Image icon = null!;
    public Button removeButton = null!;

    [SerializeField] private GameObject tradingUI;

    private void Awake()
    {
        tradingUI = GameObject.Find("Trading");
    }

    public void addItem(Item newItem)
    {
        item = newItem;
        // Set the sprite of the item in inventory slot
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void clearSlot()
    {
        item = null;
        // Remove the sprite of the item in inventory slot
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    // When player presses "X" UI button to remove item
    public void onRemoveButton()
    {
        if (item == null) return;
        Debug.Log("Removing " + item.name);
        // Inventory.instance.Remove(item);
        Inventory.Instance.DropItem(item);
    }

    // When player removes item from equipment UI
    public void onUnequipItem()
    {
        if (item == null) return;
        if (item.EquipmentSlot == null) return;
        EquipmentManager.Instance.Unequip((int)item.EquipmentSlot);
    }

    // When player hover over item in inventory
    public void displayItemInfo()
    {

        UnityEngine.UI.Text
            nameText,
            healthText,
            damageText,
            staminaText,
            potionNameText,
            healText,
            worthText;

        // Display item info
        if (item is Equipment equipment)
        {
            ItemInfoManager.Instance.showEquipmentInfo();

            nameText = GameObject.Find("Item Info Name Text").GetComponent<Text>();
            healthText = GameObject.Find("Info Health Text").GetComponent<Text>();
            damageText = GameObject.Find("Info Damage Text").GetComponent<Text>();
            staminaText = GameObject.Find("Info Stamina Text").GetComponent<Text>();
            worthText = GameObject.Find("Info Gold Equipment Text").GetComponent<Text>();

            Debug.Log($"{this.item.name} is equipment");


            nameText.text = equipment.name;
            healthText.text = equipment.healthModifier.ToString();
            damageText.text = equipment.damageModifier.ToString();
            staminaText.text = equipment.staminaModifier.ToString();
            worthText.text = equipment.itemValue.ToString();
        }
        // Display potion info
        else if (item is Potion potion)
        {
            ItemInfoManager.Instance.showPotionInfo();

            potionNameText = GameObject.Find("Potion Info Name Text").GetComponent<Text>();
            healText = GameObject.Find("Info Heal Text").GetComponent<Text>();
            worthText = GameObject.Find("Info Gold Potion Text").GetComponent<Text>();

            Debug.Log($"{this.item.name} is potion");

            potionNameText.text = potion.name;
            healText.text = potion.healModifier.ToString();
            worthText.text = potion.itemValue.ToString();
        }

    }

    // When player stops hovering over item in inventory
    public void closeDisplayItemInfo()
    {

        ItemInfoManager.Instance.closeEquipmentInfo();
        ItemInfoManager.Instance.closePotionInfo();
    }

    // When player clicks on item in inventory to use it
    // Calls Use() method in Item.cs to determine what to do
    public void useItem()
    {
        if (tradingUI.activeSelf)
        {
            if (SelectTradeType.Instance.isBuy) return;

            // Selling selected item
            SelectedItemSlot.Instance.addItem(item);
            Inventory.Instance.Remove(item);
            return;
        }
        item?.Use();
    }
}
