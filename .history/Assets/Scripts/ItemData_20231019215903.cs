using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Data")]
public class ItemData : ScriptableObject
{
    // Start is called before the first frame update
    public int id;
    public String itemName;
    public int value;
    public Sprite icon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
