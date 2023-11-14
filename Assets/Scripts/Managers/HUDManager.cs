using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] public RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryContainer;

    [SerializeField] private TextMeshProUGUI interactLabel;
    [SerializeField] private TextMeshProUGUI tooltipLabel;
    [SerializeField] private Button useItemButton;

    [SerializeField] private TextMeshProUGUI dialogueLabel;

    // Stats
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private RectTransform healthBar;

    [SerializeField] private TextMeshProUGUI staminaLabel;
    [SerializeField] private RectTransform staminaBar;

    [SerializeField] public TextMeshProUGUI batteryLabel;
    [SerializeField] public RectTransform batteryBar;
    [SerializeField] public Image batteryBarImage;

    private int dialogueOriginalY;
    public Color currentBattercolor = Color.yellow;

    private ItemData selectedItem;

    public static HUDManager Instance;

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

    void Start()
    {
        dialogueLabel.CrossFadeAlpha(0.0f, 0.0f, true);
        dialogueOriginalY = (int)dialogueLabel.rectTransform.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInventoryVisible();
        HandleDialogueFloat();
    }

    void HandleDialogueFloat()
    {
        // Make the title bounce up and down, rotate a bit too, and fade in and out
        float newY = dialogueOriginalY + Mathf.Sin(Time.time * 2) * 10;
        dialogueLabel.rectTransform.anchoredPosition = new Vector2(dialogueLabel.rectTransform.anchoredPosition.x, newY);

        float newRot = Mathf.Sin(Time.time * 3) * 0.5f;
        dialogueLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newRot);
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


    private void OnObjectSelect(GameObject selectedObject)
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

        if (selectedItem.type == ItemType.Equipment)
        {
            if (selectedItem.value == 1)
            {

                useItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
            else
            {
                useItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
            }
        }
        else
        {
            useItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
        }

        useItemButton.gameObject.SetActive(true);
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

                    if (itemData.type == ItemType.Equipment)
                    {
                        transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIPMENT";
                    }

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

            if (itemData.type == ItemType.Equipment)
            {
                clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIPMENT";
            }

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


    public void OnUseItem()
    {
        if (selectedItem == null) return;

        if (selectedItem.value > 0 && selectedItem.type == ItemType.Consumable)
        {
            InventoryManager.Instance.RemoveItem(selectedItem);
            ItemManager.Instance.UseItem(selectedItem.itemName);
        }

        if (selectedItem.type == ItemType.Equipment)
        {
            // Toggle the equipment
            if (selectedItem.value == 2)
            {
                selectedItem.value = 1;
                ItemManager.Instance.UnequipItem(selectedItem.itemName);
            }
            else
            {
                selectedItem.value = 2;
                ItemManager.Instance.EquipItem(selectedItem.itemName);
            }

            // De-select
            selectedItem = null;
            tooltipLabel.text = "";
            useItemButton.gameObject.SetActive(false);
        }
        else if (selectedItem.value <= 0)
        {
            // De-select
            selectedItem = null;
            tooltipLabel.text = "";
            useItemButton.gameObject.SetActive(false);
        }

        UpdateStats();
    }

    // Stats, such as health, stamina, flashlight
    public void UpdateStats()
    {
        // Update text
        healthLabel.text = $"Health: <color=red>{GameManager.Instance.HealthPercentage}</color> / 100";
        batteryLabel.text = $"Battery: <color=yellow>{GameManager.Instance.BatteryPercentage}</color> / 100";
        staminaLabel.text = $"Stamina: <color=lightblue>{GameManager.Instance.StaminaPercentage}</color> / 100";

        // Update bars
        healthBar.anchorMax = new Vector2(GameManager.Instance.HealthPercentage / 100, 0);
        batteryBar.anchorMax = new Vector2(GameManager.Instance.BatteryPercentage / 100, 0);
        staminaBar.anchorMax = new Vector2(GameManager.Instance.StaminaPercentage / 100, 0);

        batteryBar.gameObject.SetActive(GameManager.Instance.BatteryPercentage > 0);
        SetBatteryColor();
        batteryBarImage.color = currentBattercolor;
    }

    public void DecrementBatteryPercentage(float decrement)
    {
        GameManager.Instance.BatteryPercentage -= decrement;
        UpdateStats();
    }

    public void UpdateBatteryBar(float percent)
    {
        batteryBar.anchorMax = new Vector2(percent / 100, 0);
    }

    public void SetBatteryColor(){

        Debug.Log(GameManager.Instance.BatteryPercentage);
        if(GameManager.Instance.BatteryPercentage > 70){
            currentBattercolor = Color.yellow;
        }
        else if (GameManager.Instance.BatteryPercentage > 40 && GameManager.Instance.BatteryPercentage <= 70){
            currentBattercolor = Color.blue;
        }
        else if (GameManager.Instance.BatteryPercentage > 0 && GameManager.Instance.BatteryPercentage <= 40){
            currentBattercolor = Color.green;
        }
        else if (GameManager.Instance.BatteryPercentage == 0){
            currentBattercolor = Color.red;
        }
    }

    public void ShowDialogue(string message)
    {
        dialogueLabel.text = message;
        dialogueLabel.CrossFadeAlpha(1, 0.5f, false);
        Invoke(nameof(HideDialogue), 3.0f);
    }

    private void HideDialogue()
    {
        dialogueLabel.CrossFadeAlpha(0, 0.5f, false);
    }

    private void ResetDialogue()
    {
        dialogueLabel.text = "";
    }

    // public void Blink(){
    //     StartCoroutine(BatteryFlash(3f));
    // }

    // public IEnumerator BatteryFlash(float time){
    //     float maxtime = time;
    //     bool shouldBlink = true;
    //     // batteryText.text = "No more battery!!!";
    //     UpdateBatteryBar(100f);
    //     while(shouldBlink){
    //         SetBatteryColor(Color.white);
    //         yield return new WaitForSeconds(0.2f);
    //         SetBatteryColor(currentBattercolor);
    //         yield return new WaitForSeconds(0.2f);
    //         if (maxtime != 0){
    //             maxtime -= 1;
    //         }
    //         else{
    //             shouldBlink = false;
    //         }
    //     }
    //     UpdateBatteryBar(GameManager.Instance.BatteryPercentage);
    // }
}
