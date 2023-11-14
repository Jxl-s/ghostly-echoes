using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    [SerializeField] private RectTransform keyPanel;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI messageText;

    private bool monologueFinished = false;
    private bool isDisplaying = false;
    private string curDisplay = "";
    private float displayDebounce = 0;


    // For steps of tutorial
    private bool hasPlayerMoved = false;
    private bool hasPlayerInteracted = false;
    private bool hasPlayerOpenedInventory = false;
    private bool hasPlayerEquippedFlashlight = false;
    private bool hasPlayerToggledFlashlight = false;

    [SerializeField] private GameObject flashlightSpotlight;

    [SerializeField] private GameObject station_1;
    [SerializeField] private GameObject station_2;
    [SerializeField] private GameObject station_3;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine when the script starts
        GameManager.Instance.ControlsEnabled = false;
        StartCoroutine(DoMonologue());
    }

    IEnumerator DoMonologue()
    {
        // Display the first text
        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("This place looks pretty familiar...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("Have I been there before...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("I don't remember...");
        yield return new WaitForSeconds(4f);

        GameManager.Instance.ControlsEnabled = true;
        monologueFinished = true;
    }

    void DisplayKey(string key, string message)
    {
        // If the key is already being displayed, don't do anything
        if (curDisplay == key + "_" + message)
        {
            return;
        }

        isDisplaying = true;

        keyText.text = key;
        messageText.text = message;

        curDisplay = key + "_" + message;
    }

    void HideKey()
    {
        isDisplaying = false;
    }

    // Returns whether all tutorial steps are completed
    private bool CheckTutorialSteps()
    {
        // Also do checks for the tutorial steps
        if (!hasPlayerMoved)
        {
            station_1.SetActive(true);
            DisplayKey("W", "Use <color=yellow>\"wasd\"</color> to move");
            // Check the player position, make sure its not the default
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Vector3 position = player.transform.position;
                Vector2 position2 = new Vector2(position.x, position.z);
                Vector2 targetPosition = new Vector2(-4, -26f);

                if ((position2 - targetPosition).magnitude < 3f)
                {
                    hasPlayerMoved = true;
                    displayDebounce = 1f;
                    station_1.SetActive(false);
                    HideKey();
                }
            }

            return false;
        }

        if (!hasPlayerInteracted)
        {
            station_2.SetActive(true);
            DisplayKey("F", "Use <color=yellow>\"F\"</color> to interact");

            // Make sure the object is now in the inventory
            if (InventoryManager.Instance.Items.Count > 0)
            {
                hasPlayerInteracted = true;
                displayDebounce = 1f;
                station_2.SetActive(false);
                HideKey();
            }

            return false;
        }

        if (!hasPlayerOpenedInventory)
        {
            DisplayKey("T", "Use <color=yellow>\"T\"</color> to view inventory");

            // Check panel open
            if (HUDManager.Instance.inventoryPanel.gameObject.activeSelf)
            {
                hasPlayerOpenedInventory = true;
                displayDebounce = 1f;
                HideKey();
            }

            // Make sure the inventory is currently open
            return false;
        }

        if (!hasPlayerEquippedFlashlight)
        {
            DisplayKey("!", "Equip your flashlight");

            // Check if the flashlight is equipped
            // PROBLEM: make sure the instance is set soon
            if (ItemManager.Instance.flashlight.gameObject.activeSelf)
            {
                hasPlayerEquippedFlashlight = true;
                displayDebounce = 1f;
                HideKey();
            }

            return false;
        }

        if (!hasPlayerToggledFlashlight)
        {
            DisplayKey("R", "Use <color=yellow>\"R\"</color> to toggle your flashlight");

            // Check if the flashlight is toggled (some check with the spotlight)
            if (flashlightSpotlight.activeSelf)
            {
                hasPlayerToggledFlashlight = true;
                displayDebounce = 1f;
                HideKey();
            }

            return false;
        }

        station_3.SetActive(true);
        DisplayKey("!", "You are ready...");
        return true;
    }

    void Update()
    {
        // Lerp the position, to y = 75 if it's displaying, otherwise -75
        float targetY = isDisplaying ? 75f : -75f;
        keyPanel.anchoredPosition = Vector2.Lerp(keyPanel.anchoredPosition, new Vector2(keyPanel.anchoredPosition.x, targetY), Time.deltaTime * 2f);

        if (monologueFinished)
        {
            displayDebounce -= Time.deltaTime;
            if (displayDebounce <= 0)
            {
                displayDebounce = 0;
                CheckTutorialSteps();
            }
        }
    }
}
