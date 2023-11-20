using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOneDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    private bool doingFirstMonologue = false;
    private bool doingSecondMonologue = false;

    void Start()
    {
        if (!GameManager.Instance.BasementFinished)
        {
            doingFirstMonologue = true;
            FirstTimeCutscene();
        }
        else
        {
            doingSecondMonologue = true;
            SecondTimeCutscene();
        }
    }

    void FirstTimeCutscene()
    {
        GameManager.Instance.ControlsEnabled = false;
        HUDManager.Instance.SetContainerVisible(false);
        StartCoroutine(HUDManager.Instance.UpdateMask(0f, 3f));

        StartCoroutine(DoFirstMonologue());
    }

    void SecondTimeCutscene()
    {
        GameManager.Instance.ControlsEnabled = false;
        HUDManager.Instance.SetContainerVisible(false);
        StartCoroutine(HUDManager.Instance.UpdateMask(0f, 3f));
    }

    IEnumerator DoFirstMonologue()
    {
        // Display the first text
        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("Where are the voices...");
        yield return new WaitForSeconds(4f);

        Camera.main.transform.localPosition = new Vector3(0, 0.824f, 0.338f);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);

        GameManager.Instance.ControlsEnabled = true;
        HUDManager.Instance.SetContainerVisible(true);
        doingFirstMonologue = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFirstMonologue();
        HandleSecondMonologue();
    }

    void HandleFirstMonologue()
    {
        if (doingFirstMonologue)
        {
            // Make the camera turn, do random things
            Camera.main.transform.rotation = Quaternion.Euler(
                Mathf.Sin(Time.time * 2.0f) * 2f,
                Time.time * 5.0f,
                Mathf.Sin(Time.time * 2f) * 2f
            );
        }
    }

    void HandleSecondMonologue()
    {

    }
}
