using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EndingUIBehaviour : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text displayText;
    private bool isFinalDialogueComplete = false;
    public float delay = 0.09f; // delay between characters
    private string fullText;

    // Start is called before the first frame update
    void Start()
    {
        button.gameObject.SetActive(false);
        fullText = displayText.text;
        displayText.text = "";
        StartCoroutine(ShowText());
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinalDialogueComplete)
        {
            button.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < fullText.Length; i++)
        {
            if(fullText[i] == '*'){
                yield return new WaitForSeconds(2.5f);
                displayText.text = "";
                continue;
            }
            displayText.text += fullText[i];
            yield return new WaitForSeconds(delay);
        }

        displayText.transform.position = Vector3.Lerp(displayText.transform.position, new Vector3(displayText.transform.position.x, displayText.transform.position.y + 50, displayText.transform.position.z), Time.deltaTime/5);

        yield return new WaitForSeconds(1f);

        isFinalDialogueComplete = true;
    }
}
