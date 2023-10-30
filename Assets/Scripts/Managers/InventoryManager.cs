using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();

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

    void Start()
    {
        // Add the initial flashlight (2 for equipped, 1 for unequipped)
        Items.Add(new ItemData { itemName = "Flashlight", value = 2, description = "You might need this to see...", type = ItemType.Equipment });
        HUDManager.Instance.UpdateInventory(Items);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Send a ray, check if the object is a pickupable
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
            {
                Pickupable pickupable = hit.collider.gameObject.GetComponent<Pickupable>();
                pickupable?.Pickup();
            }
        }
    }

    public void AddItem(ItemData item)
    {
        // Check if the item is already in the inventory, then increment the count
        foreach (ItemData i in Items)
        {
            if (i.itemName == item.itemName)
            {
                i.value++;
                HUDManager.Instance.UpdateInventory(Items);
                return;
            }
        }

        // If the item is not in the inventory, add it
        item.value = 1;
        Items.Add(item);

        HUDManager.Instance.UpdateInventory(Items);
    }

    public void RemoveItem(ItemData item)
    {
        // Check if the item is already in the inventory, then increment the count
        foreach (ItemData i in Items)
        {
            if (i.itemName == item.itemName)
            {
                i.value--;
                if (i.value <= 0)
                {
                    Items.Remove(i);
                }
                HUDManager.Instance.UpdateInventory(Items);
                return;
            }
        }
    }
}
