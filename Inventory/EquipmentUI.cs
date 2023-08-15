using UnityEngine;
using System;

/*
 * Class for displaying items that are equipped
 */
public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentUI = null!;

    public Transform itemParent = null!;
    public ItemSlot[] equippedSlots = Array.Empty<ItemSlot>();

    public EquipmentManager equipmentManager = null!;

    // Set the equipment UI to be inactive on start
    void Start()
    {
        equipmentUI.SetActive(false);
    }

    // Open and close the equipment UI
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && !PauseMenu.GamePaused)
        {
            equipmentUI.SetActive(!equipmentUI.activeSelf);
        }

        UpdateEquipmentUI();
    }

    // Update the equipment UI on any item change
    void UpdateEquipmentUI()
    {
        for (int i = 0; i < equippedSlots.Length; i++)
        {
            if (EquipmentManager.Instance.ItemAt(i, out Equipment? item))
            {
                equippedSlots[i].addItem(item);
            }
            else
            {
                equippedSlots[i].clearSlot();
            }
        }
    }
}
