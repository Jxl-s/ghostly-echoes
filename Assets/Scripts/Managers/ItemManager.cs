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

    private void UseBattery()
    {
        if(flashlight.battery + 20 > 100 && flashlight.battery < 100){
            flashlight.battery = 100;
        } else if (flashlight.battery < 100) {
            flashlight.battery += 20;
        }    
            flashlight.SetBatteryText();
    }
}
