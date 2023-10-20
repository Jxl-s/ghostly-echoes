using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();
    void Awake()
    {
        Instance = this;
    }
    public void addItem(ItemData item) {
        Items.Add(item);
    }
    public void removeItem(ItemData item) {
        Items.Remove(item);
    }
}
