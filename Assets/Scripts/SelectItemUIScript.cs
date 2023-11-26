using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemUIScript : MonoBehaviour
{
    public void OnSelect()
    {
        HUDManager.Instance.OnObjectSelect(gameObject);
    }
}
