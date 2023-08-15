using System;
using UnityEngine;

/*
 * This class is assigned to equipment that is supposed to be pickable enabling the player to interact with it
 */
public class ItemPickUp : MonoBehaviour
{
    public Item item = null!;
    public static ItemPickUp instance = null!;

    // Calls Add() when player interacts with item in Inventory.cs
    public void PickUpItem()
    {
        // Add item to inventory
        Debug.Log($"Picked up {item.name}");
        Debug.Log($"Item Value: {item.itemValue}");
        var picked = Inventory.Instance.Add(item);
        if (picked) Destroy(gameObject);
    }

    // Move object when player touches it
    public void MoveObject(Vector3 player)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(player * 0.5f, ForceMode.Impulse);
    }

}
