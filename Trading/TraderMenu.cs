using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class updates the trading menu UI
 */


public class TraderMenu : MonoBehaviour
{
    [SerializeField] private GameObject traderMenu = null!;
    private TraderItemSlot[] traderItemSlots = Array.Empty<TraderItemSlot>();

    private TraderInventory traderInventory = null!;

    private void Start()
    {
        traderInventory = TraderInventory.Instance;
        traderItemSlots = traderMenu.GetComponentsInChildren<TraderItemSlot>();

        traderInventory.OnItemChanged += UpdateTraderMenu;
    }

    // Update traders inventory on change
    private void UpdateTraderMenu()
    {
        for (int i = 0; i < traderItemSlots.Length; i++)
        {
            if (i < traderInventory.items.Count)
            {
                traderItemSlots[i].addItem(traderInventory.items[i]);
            }
            else
            {
                traderItemSlots[i].clearSlot();
            }
        }
    }
}
