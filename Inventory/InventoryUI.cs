using Cinemachine;
using UnityEngine;
using System;

/*
 * Class for displaying items in the inventory
 */
public class InventoryUI : MonoBehaviour
{
    Inventory inventory = null!;

    public Transform itemParent = null!;
    public GameObject inventoryUI = null!;

    private CinemachineBrain brain = null!;

    private bool isInventoryOpen = false;

    private PlayerMovement playerMovement = null!;

    ItemSlot[] slots = Array.Empty<ItemSlot>();

    private bool canUpdate = true;

    // Set the inventory UI to be inactive on start
    void Start()
    {
        brain = FindObjectOfType<CinemachineBrain>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        inventoryUI.SetActive(false);

        inventory = Inventory.Instance;
        inventory.OnItemChanged += updateUI; // Update UI when item is added
        slots = itemParent.GetComponentsInChildren<ItemSlot>();
    }

    // Open and close the inventory UI
    // Disable player movement and camera when inventory is open
    // Enable mouse cursor when inventory is open
    void Update()
    {
        if (!canUpdate) return;
        if (Input.GetButtonDown("Inventory") && !PauseMenu.GamePaused)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            isInventoryOpen = !isInventoryOpen;
        }
        if (inventoryUI.activeSelf)
        {
            playerMovement.enabled = false;
            brain.enabled = false;
            Cursor.visible = true;
        }
        else
        {
            playerMovement.enabled = true;
            brain.enabled = true;
            Cursor.visible = false;
        }

    }

    // Update the inventory UI on any item change
    void updateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].addItem(inventory.items[i]);
            }
            else
            {
                slots[i].clearSlot();
            }
        }
    }

    public void SetCanUpdate(bool value)
    {
        canUpdate = value;
    }
}
