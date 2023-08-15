using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/*
 * Class for managing the players inventory and checking if the player has enough space for more items
 */
public class Inventory : MonoBehaviour
{
    // Singleton for accessing inventory from other scripts
    #region Singleton

    public static Inventory Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found.");
            Destroy(Instance);
        }
        Instance = this;
    }

    # endregion

    public int space = 20;

    public Action OnItemChanged = delegate { };

    public List<Item> items = new List<Item>();

    private GameObject player = null!;

    // Assign player
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Add item to inventory called from ItemPickup.cs
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        if (!item.isDefaultItem) items.Add(item);

        if (OnItemChanged != null) OnItemChanged.Invoke();
        return true;
    }

    // Destroy item from inventory (for potions)
    public void Remove(Item item)
    {
        items.Remove(item);
        if (OnItemChanged != null) OnItemChanged.Invoke();
    }

    // Drop item from inventory or add it to the inventory if it is equipped
    public void DropItem(Item item)
    {
        items.Remove(item);
        if (OnItemChanged != null) OnItemChanged.Invoke();

        // Create new game object to be dropped
        if (Inventory.Instance.items.Contains(item) || EquipmentManager.Instance.Equipment.Contains(item)) return;
        Debug.Log("Dropping " + item.name);

        GameObject droppedItem = Instantiate(item.prefab, player.transform.position, Quaternion.identity);
        droppedItem.GetComponent<Rigidbody>().AddForce(player.transform.forward * 10, ForceMode.Impulse);
    }


}
