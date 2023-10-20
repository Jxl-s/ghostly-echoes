using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/InventoryData")]
public class ItemData : ScriptableObject
{
    public int id;
    public String itemName;
    public int value;
    public String description;
}
