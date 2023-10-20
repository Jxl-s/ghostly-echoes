using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Name").GetComponent<Text>();
            var itemValue = obj.transform.Find("Value").GetComponent<Text>();
            var itemDesc = obj.transform.Find("Description").GetComponent<Text>();
    
            itemName.text = item.itemName;
            itemValue.text = item.value.ToString();
            itemDesc.text = item.description;
        }
    }
}
