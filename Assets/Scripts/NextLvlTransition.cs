using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLvlTransition : MonoBehaviour
{
    public bool monsterRequired = false;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (monsterRequired && !GameManager.Instance.MonsterActive)
        {
            StartCoroutine(NotYet());
            return;
        }
        GameManager.Instance.MonsterActive = false;
        Debug.Log("works");
        // Load the next level
        StartCoroutine(HUDManager.Instance.UpdateMask(1f, 3f));
        Invoke(nameof(NextScene), 3f);
    }

    void NextScene()
    {
        // if (SceneManager.GetActiveScene().buildIndex == 3)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator NotYet()
    {
        yield return new WaitForSeconds(0.5f);
        HUDManager.Instance.ShowDialogue("I can't leave yet...");
        yield return new WaitForSeconds(4f);
        HUDManager.Instance.HideDialogue();
    }
}
