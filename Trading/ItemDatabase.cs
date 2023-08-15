using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class contains all items that can be picked up in the game
 */

public class ItemDatabase : MonoBehaviour
{
    // Singleton for accessing items from other scripts
    #region Singleton

    public static ItemDatabase Instance { get; private set; } = null!;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    # endregion

    // Items added to list in editor
    public List<Item> allItems = null!;
}
