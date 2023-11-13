using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    [SerializeField] private RectTransform keyPanel;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI messageText;

    private bool isDisplaying = false;

    // For steps of tutorial
    private bool hasPlayerMoved = false;
    private bool hasPlayerInteracted = false;
    private bool hasPlayerOpenedInventory = false;
    private bool hasPlayerEquippedFlashlight = false;
    private bool hasPlayerToggledFlashlight = false;


    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine when the script starts
        StartCoroutine(DisplayTextWithDelay());
    }

    IEnumerator DisplayTextWithDelay()
    {
        // Display the first text
        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("This place looks pretty familiar...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("Have I been there before...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("I don't remember...");
        yield return new WaitForSeconds(4f);

        // You can add more text displays with delays here if needed
        DisplayKey("W", "Use <color=yellow>\"wasd\"</color> to move");
        // DisplayKey("F", "Use <color=yellow>\"F\"</color> to interact");
        // DisplayKey("T", "Use <color=yellow>\"T\"</color> to view inventory");
        // DisplayKey("!", "Equip your flashlight");
        // DisplayKey("R", "Use <color=yellow>\"R\"</color> to toggle your flashlight");
        // DisplayKey("!", "You are ready...");
    }

    void DisplayKey(string key, string message)
    {
        isDisplaying = true;

        keyText.text = key;
        messageText.text = message;
    }

    void HideKey()
    {
        isDisplaying = false;
    }

    void CheckTutorialSteps()
    {
        // Also do checks for the tutorial steps
        if (!hasPlayerMoved)
        {
            return;
        }
        if (!hasPlayerInteracted)
        {
            return;
        }
        if (!hasPlayerOpenedInventory)
        {
            return;
        }
        if (!hasPlayerEquippedFlashlight)
        {
            return;
        }
        if (!hasPlayerToggledFlashlight)
        {
            return;
        }
    }

    void Update()
    {
        // Lerp the position, to y = 75 if it's displaying, otherwise -75
        float targetY = isDisplaying ? 75f : -75f;
        keyPanel.anchoredPosition = Vector2.Lerp(keyPanel.anchoredPosition, new Vector2(keyPanel.anchoredPosition.x, targetY), Time.deltaTime * 2f);

        CheckTutorialSteps();
    }
}
