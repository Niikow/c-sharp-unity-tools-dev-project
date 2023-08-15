using UnityEngine;

/*
 * Parent class defining all pickable items for the player
 */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{

    public new string name { get; private set; } = "New Item";

    public int itemValue = 0;
    public Sprite icon = null!;
    public bool isDefaultItem = false;

    public GameObject prefab = null!;

    public void OnEnable()
    {
        if (base.name != "") name = base.name;
    }

    // Override Use() for specific functionality in subclasses
    public virtual void Use()
    {
        Debug.Log($"Using {name}");
    }

    public virtual EquipmentSlot? EquipmentSlot => null;

    // Remove item from inventory
    public void RemoveFromInventory()
    {
        Inventory.Instance.DropItem(this);
    }


}
