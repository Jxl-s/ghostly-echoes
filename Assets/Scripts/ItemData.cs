using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Key,
    Memory
}

[CreateAssetMenu(fileName = "Item", menuName = "Items/InventoryData")]
public class ItemData : ScriptableObject
{
    // Name of the item. Will be unique, and the displayed value
    public string itemName;

    // The amount of the item in the inventory
    public int value;

    // A short description that appears as a tooltip when clicked
    public string description;

    // Whether it's stackable or not
    public ItemType type;

    public void Use() { }
}
