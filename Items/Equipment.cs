using UnityEngine;

/*
 * Child class of Item for player gear and weapons
 */
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot slot;

    public override EquipmentSlot? EquipmentSlot => slot;


    public int healthModifier;
    public int staminaModifier;
    public int armourModifier;
    public int damageModifier;

    // Equip the item
    public override void Use()
    {
        base.Use();
        EquipmentManager.Instance.Equip(this);
        RemoveFromInventory();
    }
}

// Item slot indexes for equipment UI slots
public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Boots,
    Weapon1,
    Weapon2,
    Accessory1,
    Accessory2
}
