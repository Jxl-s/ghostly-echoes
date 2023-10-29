using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ItemData item;

    void Pickup()
    {
        InventoryManager.Instance.addItem(item);
        Destroy(gameObject);
    }

    public void OnMouseEnter()
    {
        HUDBehaviour.Instance.SetInteractText(true, "Pickup " + item.name + " [F]");

        // Make the "Selected" child object visible
        transform.Find("Normal").gameObject.SetActive(false);
        transform.Find("Selected").gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        HUDBehaviour.Instance.SetInteractText(false, "");

        transform.Find("Normal").gameObject.SetActive(true);
        transform.Find("Selected").gameObject.SetActive(false);
    }
}
