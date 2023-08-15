using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class controls the trader AI behaviour depending on player offer
 */

public class TradeOfferAI : MonoBehaviour
{
    // Singleton for accessing trader AI methods from other scripts
    #region Singleton

    public static TradeOfferAI Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        angryUI = GameObject.Find("Angry");
        neutralUI = GameObject.Find("Neutral");
        happyUI = GameObject.Find("Happy");
        traderOffer = GameObject.Find("Final Offer Value Text").GetComponent<Text>();
        placeOfferButton = GameObject.Find("Place Offer Button").GetComponent<Button>();
    }

    #endregion

    [SerializeField] private InputField playerOffer;

    [SerializeField] GameObject angryUI, neutralUI, happyUI;

    private Button placeOfferButton;

    private Text traderOffer;

    private int playerOfferInt, traderOfferInt;
    private float approval = 0.5f;
    private bool approvalChanged = false, finalOffer = false;

    private List<Item> itemsOffered = new List<Item>(), playerItemsOffered = new List<Item>();

    private Dictionary<Item, int> itemsOfferedPrices = new Dictionary<Item, int>();

    private void Start()
    {
        playerOffer = GameObject.Find("Player Offer").GetComponent<InputField>();

        angryUI.SetActive(false);
        neutralUI.SetActive(true);
        happyUI.SetActive(false);
    }

    // Logic for placing an offer on item from the trader
    public void BuyItem(Item item)
    {
        // Cannot change approval rating more than once with the same item
        if (itemsOffered.Contains(item))
        {
            approvalChanged = true;
        }

        if (!itemsOffered.Contains(item))
        {
            itemsOffered.Add(item);
            approvalChanged = false;
        }

        // Cannot place offer more than once with the same item
        if (itemsOfferedPrices.ContainsKey(item))
        {
            traderOfferInt = itemsOfferedPrices[item];
            traderOffer.text = traderOfferInt.ToString();
            return;
        }

        int value = item.itemValue, duplicateCount = 0;

        foreach (var i in TraderInventory.Instance.items)
        {
            if (i.name == item.name) duplicateCount++;

        }

        value -= Mathf.RoundToInt(value * 0.1f * duplicateCount); // 10% discount per duplicate

        int happyOffer = Mathf.RoundToInt(value * 0.80f);
        int neutralOffer = Mathf.RoundToInt(value * 0.60f);

        if (playerOfferInt >= happyOffer)
        {
            if (angryUI.activeSelf) angryUI.SetActive(false);
            if (neutralUI.activeSelf) neutralUI.SetActive(false);
            happyUI.SetActive(true);

            if (!approvalChanged && approval + 0.1f < 1f)
            {
                approval += 0.1f;
                approvalChanged = true;
            }
            else
            {
                approval = 1f;
            }

            traderOfferInt = value - Mathf.RoundToInt(value * 0.1f);
        }

        else if (playerOfferInt >= neutralOffer)
        {
            if (angryUI.activeSelf) angryUI.SetActive(false);
            if (happyUI.activeSelf) happyUI.SetActive(false);
            neutralUI.SetActive(true);

            if (!approvalChanged && approval + 0.05f < 1f)
            {
                approval += 0.05f;
                approvalChanged = true;
            }

            traderOfferInt = value - Mathf.RoundToInt(value * 0.05f);
        }

        else if (playerOfferInt < neutralOffer)
        {
            if (happyUI.activeSelf) happyUI.SetActive(false);
            if (neutralUI.activeSelf) neutralUI.SetActive(false);
            angryUI.SetActive(true);

            // high approval = 0.25f approval loss | low approval = 0.5f approval loss
            if (!approvalChanged && approval - 0.2f > 0f)
            {
                approval -= 0.2f;
            }
            if (!approvalChanged && approval - 0.05f > 0f)
            {
                approval -= 0.05f;
                approvalChanged = true;
            }

            traderOfferInt = value;
        }

        if (!itemsOfferedPrices.ContainsKey(item)) itemsOfferedPrices.Add(item, traderOfferInt);
        traderOffer.text = traderOfferInt.ToString();
    }

    // Logic for selling an item to the trader
    public void SellItem(Item item)
    {
        if (playerItemsOffered.Contains(item))
        {
            approvalChanged = true;
        }

        if (!playerItemsOffered.Contains(item))
        {
            playerItemsOffered.Add(item);
            approvalChanged = false;
        }

        int value = item.itemValue, duplicateCount = 0;

        foreach (var i in TraderInventory.Instance.items)
        {
            if (i.name == item.name) duplicateCount++;

        }

        value -= Mathf.RoundToInt(value * 0.1f * duplicateCount); // 10% discount per duplicate

        if (playerOfferInt <= value)
        {
            if (angryUI.activeSelf) angryUI.SetActive(false);
            if (neutralUI.activeSelf) neutralUI.SetActive(false);
            happyUI.SetActive(true);

            traderOfferInt = playerOfferInt;

            if (!approvalChanged && approval + 0.05f < 1f)
            {
                approval += 0.05f;
                approvalChanged = true;
            }
        }

        else if (playerOfferInt == value)
        {
            if (angryUI.activeSelf) angryUI.SetActive(false);
            if (happyUI.activeSelf) happyUI.SetActive(false);
            neutralUI.SetActive(true);

            traderOfferInt = value;

            approvalChanged = true;
        }

        else if (playerOfferInt > value)
        {
            if (approval >= 0.7f)
            {
                if (angryUI.activeSelf) angryUI.SetActive(false);
                if (happyUI.activeSelf) happyUI.SetActive(false);
                neutralUI.SetActive(true);

                if (playerOfferInt <= value + Mathf.RoundToInt(value * 0.1f))
                {
                    traderOfferInt = playerOfferInt;
                    if (!approvalChanged)
                    {
                        approval -= 0.1f;
                        approvalChanged = true;
                    }
                }
                else
                {
                    traderOfferInt = value + Mathf.RoundToInt(value * 0.05f);
                    if (!approvalChanged)
                    {
                        approval -= 0.15f;
                        approvalChanged = true;
                    }
                }
            }
            else
            {
                if (happyUI.activeSelf) happyUI.SetActive(false);
                if (neutralUI.activeSelf) neutralUI.SetActive(false);
                angryUI.SetActive(true);

                if (value - 30 > 0) traderOfferInt = value - 30;
                else traderOfferInt = 0;

                if (!approvalChanged && approval - 0.25f > 0f)
                {
                    approval -= 0.25f;
                    approvalChanged = true;
                }
                else if (!approvalChanged && approval - 0.05f > 0f)
                {
                    approval -= 0.05f;
                    approvalChanged = true;
                }
            }
        }

        traderOffer.text = traderOfferInt.ToString();
    }

    // Ensures that item will be placed in correct inventory
    public void confirmButton()
    {
        if (SelectTradeType.Instance.isBuy) buyItemConfirm();
        if (!SelectTradeType.Instance.isBuy) sellItemConfirm();
    }

    // Check if player has enough gold and enough space in inventory to buy item
    private void buyItemConfirm()
    {
        if ((PlayerGold.Instance.gold >= traderOfferInt) && (Inventory.Instance.items.Count < Inventory.Instance.space))
        {
            PlayerGold.Instance.gold -= traderOfferInt;
            TraderInventory.Instance.gold += traderOfferInt;
            Inventory.Instance.Add(SelectedItemSlot.Instance.item);
            SelectedItemSlot.Instance.clearSlot();
        }

        else if (PlayerGold.Instance.gold < traderOfferInt)
        {
            Debug.Log("Not enough gold");
        }

        else if (Inventory.Instance.items.Count >= Inventory.Instance.space)
        {
            Debug.Log("Not enough space");
        }
    }

    // Check if trader has enough gold and enough space in inventory to buy item
    private void sellItemConfirm()
    {
        if (TraderInventory.Instance.gold >= traderOfferInt && TraderInventory.Instance.items.Count < TraderInventory.Instance.space)
        {
            PlayerGold.Instance.gold += traderOfferInt;
            TraderInventory.Instance.gold -= traderOfferInt;
            TraderInventory.Instance.Add(SelectedItemSlot.Instance.item);
            SelectedItemSlot.Instance.clearSlot();
        }
        else if (TraderInventory.Instance.gold < traderOfferInt)
        {
            Debug.Log("Not enough gold");
        }
        else if (TraderInventory.Instance.items.Count >= TraderInventory.Instance.space)
        {
            Debug.Log("Not enough space");
        }
    }

    // Triggers AI to react to player's gold offer
    public void sendOffer()
    {
        if (int.TryParse(playerOffer.text, out playerOfferInt))
        {
            playerOfferInt = int.Parse(playerOffer.text);
        }

        if (SelectTradeType.Instance.isBuy) BuyItem(SelectedItemSlot.Instance.item);
        if (!SelectTradeType.Instance.isBuy) SellItem(SelectedItemSlot.Instance.item);
    }
}
