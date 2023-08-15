using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/*
 * Class that defines what a stats value is
 */

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int offset;
    private int maxOffset;

    private EquipmentManager? equipment;
    private Func<Equipment, int>? selector;

    public int Base;

    public int Min => 0;
    public int Max => Base + maxOffset;

    // Update the value of the stat based on the equipment
    public void SetSelector(Func<Equipment, int> selector, EquipmentManager equipment)
    {
        this.equipment = equipment;
        this.selector = selector;
        equipment.onEquipmentChanged += (newItem, oldItem) =>
        {
            // values of a null item is 0
            // for example if you have no helmet the armour is 0
            int diff = (newItem != null ? selector(newItem) : 0) - (oldItem != null ? selector(oldItem) : 0);
            maxOffset += diff;
            Value += diff;
        };
    }

    // Getter and setter for the stat value
    public int Value
    {
        get => Base + offset;
        set
        {
            value = Math.Clamp(value, Min, Max);
            offset = value - Base;
        }

    }

    public static implicit operator int(Stat stat) => stat.Value;

    public void Reset()
    {
        offset = 0;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
