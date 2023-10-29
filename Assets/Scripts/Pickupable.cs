using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ItemData item;

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }

    public void OnMouseEnter()
    {
        // Make sure distance is less than 2f
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > 4f)
        {
            return;
        }

        HUDBehaviour.Instance.SetInteractText(true, "Pickup " + item.name + " [F]");

        // Make the "Selected" child object visible
        transform.Find("Normal").gameObject.SetActive(false);
        transform.Find("Selected").gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        // Make sure distance is less than 2f
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > 4f)
        {
            return;
        }

        HUDBehaviour.Instance.SetInteractText(false, "");

        transform.Find("Normal").gameObject.SetActive(true);
        transform.Find("Selected").gameObject.SetActive(false);
    }
}
