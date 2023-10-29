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
        GameObject interactLabel = GameObject.FindGameObjectWithTag("InteractLabel");

        if (interactLabel != null)
        {
            interactLabel.GetComponent<TextMeshProUGUI>().alpha = 1;
            interactLabel.GetComponent<TextMeshProUGUI>().text = "Pickup <b>" + item.name + "</b>[F]";
        }

        // Make the "Selected" child object visible
        transform.Find("Normal").gameObject.SetActive(false);
        transform.Find("Selected").gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        // Get object with tag "InteractLabel"
        GameObject interactLabel = GameObject.FindGameObjectWithTag("InteractLabel");
        if (interactLabel != null)
        {
            interactLabel.GetComponent<TextMeshProUGUI>().alpha = 0;
        }

        transform.Find("Normal").gameObject.SetActive(true);
        transform.Find("Selected").gameObject.SetActive(false);
    }
}
