using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello?");
        StartCoroutine(DoEnterMonologue());
    }

    IEnumerator DoEnterMonologue()
    {
        // Display the first text
        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("Hello... anyone...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("I hear a voice from the right side...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.ShowDialogue("I should enter that room and investigate...");
        yield return new WaitForSeconds(4f);
    }

    public IEnumerator DoMonsterCutscene()
    {
        Quaternion startRotation = Camera.main.transform.rotation;

        GameManager.Instance.SetCutscene(true);
        Camera.main.transform.position = new Vector3(5.948f, 5.159f, 185.069f);
        Camera.main.transform.rotation = Quaternion.Euler(17.983f, 39.443f, 0f);

        yield return new WaitForSeconds(2f);
        HUDManager.Instance.ShowDialogue("What is this creature...");
        yield return new WaitForSeconds(4f);

        // Do a little camera shake
        for (int i = 0; i <= 10; i++)
        {
            int randomTilt = Random.Range(-10, 10);
            Camera.main.transform.rotation = Quaternion.Euler(17.983f, 39.443f, randomTilt);
            yield return new WaitForSeconds(0.05f);
        }

        Camera.main.transform.rotation = Quaternion.Euler(17.983f, 39.443f, 0f);
        HUDManager.Instance.ShowDialogue("He sees me... I better run");
        yield return new WaitForSeconds(2f);

        Camera.main.transform.localPosition = new Vector3(0, 0.824f, 0.338f);
        Camera.main.transform.rotation = startRotation;

        GameManager.Instance.ControlsEnabled = true;
        GameManager.Instance.SetCutscene(false);

        yield return new WaitForSeconds(2f);
        GameManager.Instance.MonsterActive = true;
        HUDManager.Instance.ShowDialogue("GET OUT OF HERE! NOW!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
