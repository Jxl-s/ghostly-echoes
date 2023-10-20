using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void addItem(ItemData item) {
        Items.Add(item);
    }
    public void removeItem(ItemData item) {
        Items.Remove(item);
    }

    public void listItems() {
        foreach(var item in Items) {
        }
    }
}
