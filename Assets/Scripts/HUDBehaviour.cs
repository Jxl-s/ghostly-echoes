using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private TextMeshProUGUI interactLabel;

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
}
