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

    public void Interact()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        switch(type){
            case "door":
                if(checkItems() && !openState) {
                    Debug.Log("case1");
                    openState = true;
                    animator.SetBool("Locked", false);
                    animator.SetBool("Closed", false);
                    gameObject.GetComponent<Collider>().isTrigger = false;
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
            default:
                break;    
        }
    }

    public void OnMouseEnter()
    {
        if(item == null) {
            openState = true;
            HUDManager.Instance.SetInteractText(true, objectName+ " [F]");
        } else {
            if(checkItems()) {
                HUDManager.Instance.SetInteractText(true, "Use " + item.itemName + " [F]");
            } else {
                HUDManager.Instance.SetInteractText(true, "Locked");
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
