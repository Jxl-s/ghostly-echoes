using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();
    public static float RANGE = 4f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (ItemData item in Items)
        {
            Debug.Log(item.itemName);
            Debug.Log(item.value);

            if (item.type == ItemType.Equipment && item.value == 2)
            {
                ItemManager.Instance.EquipItem(item.itemName);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.ControlsEnabled)
        {
            // Send a ray, check if the object is a pickupable
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, RANGE))
            {
                Pickupable pickupable = hit.collider.gameObject.GetComponent<Pickupable>();
                pickupable?.Pickup();
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                interactable?.Interact();
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

        if (item.type == ItemType.Memory)
        {
            // Show an UI containing the memory
            StartCoroutine(HUDManager.Instance.ShowMemory(item.memoryText));
        }
 
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
