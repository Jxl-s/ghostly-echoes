using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public light_Script flashlight;

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

    public void UseItem(string itemName)
    {
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
        // Unequip everything first
        switch (itemName)
        {
            case "Flashlight":
                flashlight.gameObject.SetActive(true);
                return;
        }
    }

    public void UnequipItem(string itemName)
    {
        switch (itemName)
        {
            case "Flashlight":
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
