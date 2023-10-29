using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryContainer;

    [SerializeField] private TextMeshProUGUI interactLabel;
    [SerializeField] private TextMeshProUGUI tooltipLabel;
    [SerializeField] private Button useItemButton;

    private ItemData selectedItem;

    public static HUDBehaviour Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInventoryVisible();
    }

    private void HandleInventoryVisible()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle Inventory Visibility
            bool newState = !inventoryPanel.gameObject.activeSelf;
            if (newState)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // De-select
                selectedItem = null;
                tooltipLabel.text = "";
                useItemButton.gameObject.SetActive(false);
            }

            inventoryPanel.gameObject.SetActive(newState);
        }
    }

    // Public UI methods
    public void SetInteractText(bool visible, string text)
    {
        interactLabel.alpha = visible ? 1 : 0;
        interactLabel.text = text;
    }

    public void UpdateInventory(List<ItemData> list)
    {
        // For new items, create a new button.
        // For existing ones, just change the displayed amount.
        // For the ones that aren't here anymore or are 0, delete

        foreach (ItemData itemData in list)
        {
            bool found = false;
            foreach (Transform transform in inventoryContainer)
            {
                if (transform.gameObject.name == itemData.itemName)
                {
                    // Update the amount
                    transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemData.value.ToString();
                    found = true;
                    break;
                }
            }

            if (found) continue;

            // If the item is not in the inventory, add it
            GameObject template = inventoryContainer.Find("TEMPLATE").gameObject;
            GameObject clone = Instantiate(template, inventoryContainer);

            clone.gameObject.name = itemData.itemName;
            clone.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = itemData.itemName;
            clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemData.value.ToString();
            clone.SetActive(true);

            // Add an event listener
            clone.GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { OnObjectSelect(clone); });
        }

        // Remove the ones that arent in the list anymore
        foreach (Transform transform in inventoryContainer)
        {
            if (transform.gameObject.name == "TEMPLATE") continue;

            bool found = false;
            foreach (ItemData itemData in list)
            {
                if (transform.gameObject.name == itemData.itemName)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void OnObjectSelect(GameObject selectedObject)
    {
        // InventoryManager.Instance.Items;
        foreach (ItemData itemData in InventoryManager.Instance.Items)
        {
            if (itemData.itemName == selectedObject.name)
            {
                selectedItem = itemData;
                break;
            }
        }

        if (selectedItem == null) return;
        tooltipLabel.text = selectedItem.description;
        useItemButton.gameObject.SetActive(true);
    }

    public void OnUseItem()
    {
        if (selectedItem == null) return;

        if (selectedItem.value > 0)
        {
            InventoryManager.Instance.RemoveItem(selectedItem);
            ItemManager.Instance.UseItem(selectedItem.itemName);
        }

        if (selectedItem.value <= 0)
        {
            // De-select
            selectedItem = null;
            tooltipLabel.text = "";
            useItemButton.gameObject.SetActive(false);
        }
    }
}
