using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;

/*
 * Class for managing the players equipment and checking that the player only has 1 item equipped in each slot
 */
public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager Instance { get; private set; } = null!;

    void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    #endregion

    public delegate void OnEquipmentChanged(Equipment? newItem, Equipment? oldItem);

    public OnEquipmentChanged onEquipmentChanged = (_, _) => { };

    Inventory inventory = null!;

    [SerializeField]
    private Equipment?[] currentEquipment = Array.Empty<Equipment>(); // Set in the editor

    public IEnumerable<Equipment> Equipment
    {
        get
        {
            foreach (var item in currentEquipment) if (item != null) yield return item;
        }
    }

    // Returns true if slot is free
    public bool FreeSlot(int i) => i >= 0 && i < currentEquipment.Length && currentEquipment[i] == null;

    // Returns true if slot is occupied
    // Checks if there is an item at the given slot
    // Item may be null when method returns false
    public bool ItemAt(int i, [MaybeNullWhen(false)] out Equipment item)
    {
        if (FreeSlot(i))
        {
            item = null;
            return false;
        }
        Debug.Assert(currentEquipment[i] != null);
        item = currentEquipment[i]!;
        return true;
    }

    void Start()
    {
        inventory = Inventory.Instance;
        var numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];

    }

    // Equipping new item returns previous item in same slot to inventory
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.slot;

        Equipment? oldItem;
        if (ItemAt(slotIndex, out oldItem))
        {
            Inventory.Instance.Add(oldItem);
        }

        onEquipmentChanged(newItem, oldItem);
        currentEquipment[slotIndex] = newItem;
    }

    // Unequipping item returns it to inventory
    public void Unequip(int slotIndex)
    {
        if (ItemAt(slotIndex, out Equipment? oldItem))
        {
            Inventory.Instance.Add(oldItem);
            onEquipmentChanged(null, oldItem);
            currentEquipment[slotIndex] = null;
        }

    }
}
