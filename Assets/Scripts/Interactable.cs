using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public ItemData item;
    
    public bool openState;
    public string objectName;
    public string type;
    Animator animator;
    AudioSource audio;
    Collider collider;

    void Awake() {
        audio = gameObject.GetComponent<AudioSource>();
        collider = gameObject.GetComponent<BoxCollider>();
    }
    public void Interact()
    {
        animator = gameObject.GetComponent<Animator>();
        String interact_text = "";
        switch(type){
            case "door":
                List<GameObject> doors = new List<GameObject>();
                foreach(Transform door in this.gameObject.transform.GetChild(0).transform) {
                    doors.Add(door.gameObject);
                }
                if(checkItems() && !openState) {
                    Debug.Log("case1");
                    openState = true;
                    animator.SetBool("Locked", false);
                    animator.SetBool("Closed", false);
                    gameObject.GetComponent<Collider>().isTrigger = true;
                    foreach(GameObject door in doors){
                        door.GetComponent<Collider>().isTrigger = true;
                    }
                    audio.Play();
                    break;
                }
                if(openState && animator.GetBool("Closed")) {
                    Debug.Log("case2");
                    animator.SetBool("Closed", false);
                    gameObject.GetComponent<Collider>().isTrigger = true; 
                    foreach(GameObject door in doors){
                        door.GetComponent<Collider>().isTrigger = true;
                    }
                    audio.Play();
                    break;
                } else if(openState && !animator.GetBool("Closed")) {
                    Debug.Log("case3");
                    animator.SetBool("Closed", true);
                    gameObject.GetComponent<Collider>().isTrigger = false; 
                    foreach(GameObject door in doors){
                        door.GetComponent<Collider>().isTrigger = false;
                    }
                    break;
                } else {
                    Debug.Log("case4");
                    animator.SetTrigger("Shake");
                    break;
                }
            case "cabinet":
                if(animator.GetBool("Closed")) {
                    animator.SetBool("Closed", false);
                    if(item != null)
                        interact_text = item.itemName + " [F]";
                        collider.enabled = false;
                    break;
                } else if(!animator.GetBool("Closed") && item != null) {
                    InventoryManager.Instance.AddItem(item);
                    interact_text = objectName + " [F]";
                    item = null;
                    collider.enabled = false;
                    break;
                }
                break;
            default:
                break;    
        }
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4.0f)) {
            HUDManager.Instance.SetInteractText(true, interact_text);
        } else {
            HUDManager.Instance.SetInteractText(false, "");
        }
    }

    public void OnMouseOver()
    {
        animator = gameObject.GetComponent<Animator>();
        String interact_text = "";
        if(item == null || openState) {
            openState = true;
            interact_text = objectName + " [F]";
        } else {
            switch(type){
                case "cabinet":
                    if(!animator.GetBool("Closed")) {
                        interact_text = item.itemName + " [F]";
                    } else {
                        interact_text = objectName + " [F]";
                    }
                    break;
                default: 
                    if(checkItems()) {
                        interact_text = "Use " + item.itemName + " [F]";
                    } else {
                        interact_text = "Locked";
                    }
                    break; 
                
            }
        }
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4.0f)) {
            HUDManager.Instance.SetInteractText(true, interact_text);
        } else {
            HUDManager.Instance.SetInteractText(false, "");
        }
    }

    public void OnMouseExit()
    {
        HUDManager.Instance.SetInteractText(false, "");

    }
    public bool checkItems() {
        if(item == null)
            return false;
        foreach (ItemData i in InventoryManager.Instance.Items) {
            if (i.itemName == item.itemName) {
                return true;
            }
        }
        return false;    
    }
}
