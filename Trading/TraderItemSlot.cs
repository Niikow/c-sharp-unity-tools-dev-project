using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  This class manages the traders inventory UI slots
 */


public class TraderItemSlot : MonoBehaviour
{
    Item? item;
    public UnityEngine.UI.Image icon = null!;

    [SerializeField] private bool isItemSlot = true;

    // Display item in slot
    public void addItem(Item newItem)
    {
        if (!isItemSlot) return;
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    // Remove item from slot
    public void clearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // Select item in slot to buy and remove from inventory
    public void selectItem()
    {
        if (!SelectTradeType.Instance.isBuy) return;

        // Buying selected item
        SelectedItemSlot.Instance.addItem(item);
        TraderInventory.Instance.Remove(item);
    }

    // Display item info on mouse hover
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

    public void closeDisplayItemInfo()
    {

        ItemInfoManager.Instance.closeEquipmentInfo();
        ItemInfoManager.Instance.closePotionInfo();
    }
}
