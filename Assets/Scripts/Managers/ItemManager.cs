using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public LightScript flashlight;

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

    public void Start()
    {
        PreloadItems();
    }

    public void PreloadItems()
    {
        // make references, player must exist
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject flashlightObject = player.transform.Find("flashlight").gameObject;

        flashlight = flashlightObject.GetComponent<LightScript>();
        Debug.Log("found flashlight!");
        Debug.Log(flashlight);
    }

    public void UseItem(string itemName)
    {
        PreloadItems();

        switch (itemName)
        {
            case "Battery":
                UseBattery();
                return;
            default:
                Debug.Log("Item not found: " + itemName);
                return;
        }
    }

    public void EquipItem(string itemName)
    {
        PreloadItems();

        // TODO: Unequip everything first
        switch (itemName)
        {
            case "Flashlight":
                HUDManager.Instance.EnableElement("battery");
                flashlight.gameObject.SetActive(true);
                return;
        }
    }

    public void UnequipItem(string itemName)
    {
        PreloadItems();

        switch (itemName)
        {
            case "Flashlight":
                HUDManager.Instance.DisableElement("battery");
                flashlight.gameObject.SetActive(false);
                return;
        }
    }

    private void UseBattery()
    {
        // Update battery numbers
        GameManager.Instance.BatteryPercentage += 20;
        GameManager.Instance.BatteryPercentage = Math.Min(GameManager.Instance.BatteryPercentage, 100);

        // Update display
        HUDManager.Instance.UpdateStats();
    }
}
