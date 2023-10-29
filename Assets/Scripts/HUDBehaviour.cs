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
        foreach (Transform transform in inventoryContainer)
        {
            if (transform.gameObject.name == "TEMPLATE") continue;
            Destroy(transform.gameObject);
        }

        // Get the template object
        GameObject template = inventoryContainer.Find("TEMPLATE").gameObject;
        foreach (ItemData itemData in list)
        {
            // Clone the template
            GameObject clone = Instantiate(template, inventoryContainer);

            // Set the name, amount
            clone.gameObject.name = itemData.itemName;
            clone.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = itemData.itemName;
            clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemData.value.ToString();
            clone.SetActive(true);

            // Add an event listenre
            clone.GetComponent<EventTrigger>().triggers[0].callback.AddListener((data) => { OnObjectSelect(clone); });
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

        InventoryManager.Instance.RemoveItem(selectedItem);
        ItemManager.Instance.UseItem(selectedItem.itemName);
    }
}
