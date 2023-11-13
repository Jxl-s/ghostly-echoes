using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    [SerializeField] private RectTransform keyPanel;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI messageText;


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
        // Wait for 2 seconds

        // You can add more text displays with delays here if needed
        // DisplayKey("W", "Use <color=yellow>\"wasd\"</color> to move");
        // DisplayKey("F", "Use <color=yellow>\"F\"</color> to interact");
        // DisplayKey("T", "Use <color=yellow>\"T\"</color> to view inventory");
        // DisplayKey("!", "Equip your flashlight");
        // DisplayKey("R", "Use <color=yellow>\"R\"</color> to toggle your flashlight");
        DisplayKey("!", "You are ready...");
    }

    void DisplayKey(string key, string message)
    {
        keyText.text = key;
        messageText.text = message;

        // Lerp the key panel to the center of the screen (set the Y to 75)
        keyPanel.anchoredPosition = Vector2.Lerp(keyPanel.anchoredPosition, new Vector2(0, 75), 0.5f);
    }
}
