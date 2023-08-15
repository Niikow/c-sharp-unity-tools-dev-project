using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderInventory : MonoBehaviour
{
    // Singleton for accessing trader's inventory
    #region Singleton

    public static TraderInventory Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    #endregion

    public System.Action OnItemChanged = delegate { };

    public List<Item> items = new List<Item>();

    public int gold = 100, space = 10;

    private void Start()
    {
        GenerateItems();
    }

    // At start, generate random items from database
    public void GenerateItems()
    {
        items.Clear();
        for (int i = 0; i < Random.Range(4, 10); i++)
        {
            items.Add(ItemDatabase.Instance.allItems[Random.Range(0, ItemDatabase.Instance.allItems.Count)]);
        }
        OnItemChanged.Invoke();
    }

    // Generate gold depending on inventory value
    private void GenerateGold()
    {
        foreach (var item in items)
        {
            gold += item.itemValue;
        }
    }

    // Add item to inventory
    public void Add(Item item)
    {
        items.Add(item);
        OnItemChanged.Invoke();
    }

    // Remove item from inventory
    public void Remove(Item item)
    {
        Debug.Log($"{item.name} removed");
        items.Remove(item);
        OnItemChanged.Invoke();
    }

    // Update UI
    public void interactWithItem()
    {
        OnItemChanged.Invoke();
    }
}
