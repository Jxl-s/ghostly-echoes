using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public ItemData item;
    
    public bool openState;
    public string objectName;
    public string type;
    Animator animator;

    public void Interact()
    {
        animator = gameObject.GetComponent<Animator>();
        switch(type){
            case "door":
                if(checkItems() && !openState) {
                    Debug.Log("case1");
                    openState = true;
                    animator.SetBool("Locked", false);
                    animator.SetBool("Closed", false);
                    gameObject.GetComponent<Collider>().isTrigger = true;
                    break;
                }
                if(openState && animator.GetBool("Closed")) {
                    Debug.Log("case2");
                    animator.SetBool("Closed", false);
                    gameObject.GetComponent<Collider>().isTrigger = true; 
                    break;
                } else if(openState && !animator.GetBool("Closed")) {
                    Debug.Log("case3");
                    animator.SetBool("Closed", true);
                    gameObject.GetComponent<Collider>().isTrigger = false; 
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
                        HUDManager.Instance.SetInteractText(true, item.itemName + " [F]");
                    break;
                }
                if(!animator.GetBool("Closed") && item != null) {
                    InventoryManager.Instance.AddItem(item);
                    HUDManager.Instance.SetInteractText(true, objectName + " [F]");
                    item = null;
                    break;
                } else {
                    HUDManager.Instance.SetInteractText(true, objectName + " [F]");
                    animator.SetBool("Closed", true);
                    break;
                }
            default:
                break;    
        }
    }

    public void OnMouseEnter()
    {
        animator = gameObject.GetComponent<Animator>();
        if(item == null || openState) {
            openState = true;
            HUDManager.Instance.SetInteractText(true, objectName + " [F]");
        } else {
            switch(type){
                case "cabinet":
                    if(!animator.GetBool("Closed")) {
                        HUDManager.Instance.SetInteractText(true, item.itemName + " [F]");
                    } else {
                        HUDManager.Instance.SetInteractText(true, objectName + " [F]");
                    }
                    break;
                default: 
                    if(checkItems()) {
                        HUDManager.Instance.SetInteractText(true, "Use " + item.itemName + " [F]");
                    } else {
                        HUDManager.Instance.SetInteractText(true, "Locked");
                    }
                    break; 
                
            }
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
