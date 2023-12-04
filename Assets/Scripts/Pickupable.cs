using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ItemData item;


    public void Pickup()
    {
        AudioManager.Instance.PlayEffect();
        InventoryManager.Instance.AddItem(item);
        HUDManager.Instance.SetInteractText(false, "");
        Destroy(gameObject);
    }

    // public void OnMouseEnter()
    // {
    //     HUDManager.Instance.SetInteractText(true, "Pickup " + item.name + " [F]");

    //     // Make the "Selected" child object visible
    //     transform.Find("Normal").gameObject.SetActive(false);
    //     transform.Find("Selected").gameObject.SetActive(true);
    // }

    public void OnMouseExit()
    {
        HUDManager.Instance.SetInteractText(false, "");
        gameObject.GetComponent<Outline>().OutlineWidth = 0.0f;
        // transform.Find("Normal").gameObject.SetActive(true);
        // transform.Find("Selected").gameObject.SetActive(false);
    }

    public void OnMouseOver()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4.0f) && GameManager.Instance.ControlsEnabled) {
            HUDManager.Instance.SetInteractText(true, "Pickup " + item.name + " [F]");

            // Make the "Selected" child object visible
            // bool normal = transform.Find("Normal").activeSelf;
            // bool selected = transform.Find("Selected").activeSelf;
            // transform.Find("Normal").gameObject.SetActive(false);
            // transform.Find("Selected").gameObject.SetActive(true);
            gameObject.GetComponent<Outline>().OutlineWidth = 10.0f;
        } else {
            HUDManager.Instance.SetInteractText(false, "");
            gameObject.GetComponent<Outline>().OutlineWidth = 0.0f;
            // transform.Find("Normal").gameObject.SetActive(true);
            // transform.Find("Selected").gameObject.SetActive(false);
        }
    }
}
