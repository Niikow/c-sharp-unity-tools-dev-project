using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectTradeType : MonoBehaviour
{
    // Singleton for accessing trade type from other scripts
    #region Singleton

    public static SelectTradeType Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {

            Destroy(Instance);
        }
        Instance = this;

        tradeTypeDropdown = GetComponent<Dropdown>();
        tradeTypeText = GameObject.Find("Trade Type Text").GetComponent<Text>();
    }

    # endregion

    [SerializeField] private Dropdown tradeTypeDropdown = null!;

    private Text tradeTypeText = null!;

    public bool isBuy = true;

    // Only allow one trade type at a time
    // When buying an item, cannot switch to sell
    // When selling an item, cannot switch to buy
    private void Update()
    {
        const int dropDownBuying = 0, dropDownSelling = 1;

        if (SelectedItemSlot.Instance.lastTradeType == TradeType.buy && SelectedItemSlot.Instance.item != null)
        {
            tradeTypeDropdown.value = dropDownBuying;
        }
        if (SelectedItemSlot.Instance.lastTradeType == TradeType.sell && SelectedItemSlot.Instance.item != null)
        {
            tradeTypeDropdown.value = dropDownSelling;
        }

        SelectedItemSlot.Instance.lastTradeType = SelectedItemSlot.Instance.tradeType;

        if (tradeTypeDropdown.value == dropDownBuying)
        {
            isBuy = true;
            SelectedItemSlot.Instance.tradeType = TradeType.buy;
            tradeTypeText.text = "Purchase Item";
        }
        else if (tradeTypeDropdown.value == dropDownSelling)
        {
            isBuy = false;
            SelectedItemSlot.Instance.tradeType = TradeType.sell;
            tradeTypeText.text = "Sell Item";
        }
    }
}
