using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : Item
{
    public int healModifier;

    // Destroy potion on use
    public override void Use()
    {
        base.Use();
        PlayerStats.Instance.Heal(healModifier);
        Inventory.Instance.Remove(this);
    }
}
