using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

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
        Debug.Log("using battery now...");
    }
}
