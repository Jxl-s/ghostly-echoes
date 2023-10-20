using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemData item;
    void Pickup() {
        InventoryManager.Instance.addItem(item);
        Destroy(gameObject);
    }
}
