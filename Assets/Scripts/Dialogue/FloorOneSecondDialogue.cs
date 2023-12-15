using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOneSecondDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoDialogue());
    }

    IEnumerator DoDialogue()
    {
        // Display the first text
        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("I hear voices behind me... do not turn around... RUN");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("This place looks different... why did everything fall...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("I should find a tool to help me get through...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("Quickly... go to the exit...");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
