using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCell;
    [SerializeField] public RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryContainer;

    [SerializeField] private TextMeshProUGUI interactLabel;
    [SerializeField] private TextMeshProUGUI tooltipLabel;
    [SerializeField] private Button useItemButton;

    [SerializeField] private TextMeshProUGUI dialogueLabel;

    // Stats
    [SerializeField] private GameObject health;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private RectTransform healthBar;

    [SerializeField] private GameObject stamina;
    [SerializeField] private TextMeshProUGUI staminaLabel;
    [SerializeField] private RectTransform staminaBar;
    private Image staminaBarImage;
    [SerializeField] private RectTransform mask;

    [SerializeField] public GameObject battery;
    [SerializeField] public TextMeshProUGUI batteryLabel;
    [SerializeField] public RectTransform batteryBar;
    [SerializeField] private RectTransform elementContainer;

    [SerializeField] public RectTransform memoryPanel;

    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] private GameObject damagePanel;

    private Image batteryBarImage;

    private int dialogueOriginalY;
    public Color32 currentBattercolor = new Color32(255, 255, 0, 255);
    public Color32 currentStaminacolor = new Color32(124, 180, 255, 255);

    private ItemData selectedItem;

    public static HUDManager Instance;

    private bool isPausedMenu = false;
    private bool isPausedLetter = false;
    private bool isPausedInventory = false;

    [SerializeField] private RectTransform deathPanel;
    [SerializeField] private TextMeshProUGUI deathLabel;

    private string[] deathMessages = new string[] {
        "You have failed the mission",
        "You have died",
        "You have been caught",
        "You should have ran faster",
        "You should have been more sneaky",
        "You are trapped in here forever...",
        "Your soul has been taken...",
        "get good lol",
    };

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

    public IEnumerator ShowDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        deathPanel.gameObject.SetActive(true);

        // Set the random text
        deathLabel.text = deathMessages[Random.Range(0, deathMessages.Length)];

        // Lerp the background in
        Image deathImage = deathPanel.GetComponent<Image>();
        yield return UpdateImageAlpha(deathImage, 1f, 1f);
    }

    public void LeaveGame()
    {
        GameManager.Instance.LeaveGame();
    }

    void Start()
    {
        SetSprintColor(new Color32(124, 180, 255, 255));
        dialogueLabel.CrossFadeAlpha(0.0f, 0.0f, true);
        dialogueOriginalY = (int)dialogueLabel.rectTransform.anchoredPosition.y;

        staminaBarImage = staminaBar.GetComponent<Image>();
        batteryBarImage = batteryBar.GetComponent<Image>();

        UpdateInventory(InventoryManager.Instance.Items);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        HandleInventoryVisible();
        HandleDialogueFloat();
        MenuToggle();
    }
    
    void HandleDialogueFloat()
    {
        // Make the title bounce up and down, rotate a bit too, and fade in and out
        float newY = dialogueOriginalY + Mathf.Sin(Time.time * 2) * 10;
        dialogueLabel.rectTransform.anchoredPosition = new Vector2(dialogueLabel.rectTransform.anchoredPosition.x, newY);

        float newRot = Mathf.Sin(Time.time * 3) * 0.5f;
        dialogueLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newRot);
    }

    private void MenuToggle(){
        if (Input.GetKeyDown(KeyCode.Escape) && !isPausedLetter && !isPausedInventory && !GameManager.Instance.isCutscene)
        {
            isPausedMenu = !isPausedMenu;
            if (isPausedMenu)
            {
                GameManager.Instance.PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                GameManager.Instance.ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
            }
        }
    }

    private void HandleInventoryVisible()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isPausedLetter && !isPausedMenu && !GameManager.Instance.isCutscene)
        {
            //  = falseToggle Inventory Visibility
            bool newState = !inventoryPanel.gameObject.activeSelf;
            if (newState)
            {
                isPausedInventory = true;
                GameManager.Instance.PauseGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                isPausedInventory = false;
                GameManager.Instance.ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // De-select
                selectedItem = null;
                tooltipLabel.text = "";
                useItemButton.gameObject.SetActive(false);
            }
            // PauseGame(isPausedInventory);
            inventoryPanel.gameObject.SetActive(newState);
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
        else if (selectedItem.type == ItemType.Consumable)
        {
            useItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
        }
        else if (selectedItem.type == ItemType.Memory)
        {
            useItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "View";
        }

        useItemButton.gameObject.SetActive(true);
    }

    // Public UI methods
    public void EnableElement(string element)
    {
        switch (element)
        {
            case "battery":
                battery.SetActive(true);
                return;
            case "stamina":
                stamina.SetActive(true);
                return;
            case "health":
                health.SetActive(true);
                return;
            default:
                return;
        }
    }
    public void DisableElement(string element)
    {
        switch (element)
        {
            case "battery":
                battery.SetActive(false);
                return;
            case "stamina":
                stamina.SetActive(false);
                return;
            case "health":
                health.SetActive(false);
                return;
            default:
                return;
        }
    }
    public void SetInteractText(bool visible, string text)
    {
        interactLabel.alpha = visible ? 1 : 0;
        interactLabel.text = text;
    }

    public IEnumerator ShowDamageMask()
    {
        damagePanel.SetActive(true);
        yield return UpdateImageAlpha(damagePanel.GetComponent<Image>(), 0f, 2f);
        damagePanel.SetActive(false);
        damagePanel.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.2f);
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
                        if (itemData.value == 2)
                        {
                            transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIPPED!";
                            transform.GetComponent<Image>().color = new Color32(128, 100, 100, 128);
                        }
                        else
                        {
                            transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIP";
                            transform.GetComponent<Image>().color = new Color32(100, 75, 75, 128);
                        }
                    }

                    found = true;
                    break;
                }
            }

            if (found) continue;

            // If the item is not in the inventory, add it
            GameObject clone = Instantiate(inventoryCell, inventoryContainer);

            clone.gameObject.name = itemData.itemName;
            clone.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = itemData.itemName;
            clone.transform.Find("Image").GetComponent<RawImage>().texture = itemData.icon;
            clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemData.value.ToString();

            if (itemData.type == ItemType.Equipment)
            {
                if (itemData.value == 2)
                {
                    clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIPPED!";
                    clone.transform.GetComponent<Image>().color = new Color32(128, 100, 100, 128);
                }
                else
                {
                    clone.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = "EQUIP";
                    clone.transform.GetComponent<Image>().color = new Color32(100, 75, 75, 128);
                }
            }

            clone.SetActive(true);
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

        if (selectedItem.type == ItemType.Memory)
        {
            // Show an UI containing the memory
            StartCoroutine(HUDManager.Instance.ShowMemory(selectedItem.memoryText));
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


        UpdateInventory(InventoryManager.Instance.Items);
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
        staminaBarImage.color = currentStaminacolor;
    }

    public void DecrementBatteryPercentage(float decrement)
    {
        GameManager.Instance.BatteryPercentage -= decrement;

    }

    public void UpdateBatteryBar(float percent)
    {
        batteryBar.anchorMax = new Vector2(percent / 100, 0);
    }

    public void SetBatteryColor()
    {

        if (GameManager.Instance.BatteryPercentage > 70)
        {
            currentBattercolor = new Color32(255, 255, 0, 255);
        }
        else if (GameManager.Instance.BatteryPercentage > 40 && GameManager.Instance.BatteryPercentage <= 70)
        {
            currentBattercolor = new Color32(255, 190, 0, 255);
        }
        else if (GameManager.Instance.BatteryPercentage > 0 && GameManager.Instance.BatteryPercentage <= 40)
        {
            currentBattercolor = new Color32(255, 130, 0, 255);
        }
        else if (GameManager.Instance.BatteryPercentage == 0)
        {
            currentBattercolor = new Color(255f, 1f, 0f, 1.0f);
        }
    }

    public void SetSprintColor(Color32 color)
    {
        currentStaminacolor = color;
    }

    public void TakeDamage(float dmg)
    {
        GameManager.Instance.HealthPercentage -= dmg;
    }

    public void DrainStamina(float stamina)
    {
        GameManager.Instance.StaminaPercentage -= stamina;
    }

    public void ShowDialogue(string message)
    {
        dialogueLabel.text = message;
        dialogueLabel.CrossFadeAlpha(1, 0.5f, false);
        Invoke(nameof(HideDialogue), 3.0f);
    }

    public void HideDialogue()
    {
        dialogueLabel.CrossFadeAlpha(0, 0.5f, false);
    }

    private void ResetDialogue()
    {
        dialogueLabel.text = "";
    }

    public void SetContainerVisible(bool visible)
    {
        elementContainer.gameObject.SetActive(visible);
    }

    public IEnumerator UpdateMask(float endValue, float duration)
    {
        Image maskImage = mask.GetComponent<Image>();
        float elapsedTime = 0;
        float startValue = maskImage.color.a;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, newAlpha);
            yield return null;
        }
    }

    public IEnumerator UpdateImageAlpha(Image image, float endValue, float duration)
    {
        float elapsedTime = 0;
        float startValue = image.color.a;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }
    }

    public IEnumerator ShowMemory(string message)
    {
        // Make the panel active, fade in the background, show the text, then show the close buttono
        isPausedLetter = true;
        GameManager.Instance.PauseGame();
        memoryPanel.gameObject.SetActive(true);
        yield return UpdateImageAlpha(memoryPanel.GetComponent<Image>(), 0.8f, 0.5f);

        Transform text = memoryPanel.Find("Text");
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();

        textMesh.text = message;
        text.Find("Close").gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideMemory()
    {
        isPausedLetter = false;
        GameManager.Instance.ResumeGame();
        // PauseGame(isPausedLetter);
        StartCoroutine(HideMemoryCoroutine());
    }

    private IEnumerator HideMemoryCoroutine()
    {
        Transform text = memoryPanel.Find("Text");
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();
        textMesh.text = "";
        text.Find("Close").gameObject.SetActive(false);

        yield return UpdateImageAlpha(memoryPanel.GetComponent<Image>(), 0.0f, 0.5f);
        memoryPanel.gameObject.SetActive(false);
        
        if (!inventoryPanel.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
