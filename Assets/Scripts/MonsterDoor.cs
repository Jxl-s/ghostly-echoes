using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject basementDialogue;

    private Animator animator;
    private BasementDialogue dialogue;
    private bool hasOpened = false;

    void Start()
    {
        dialogue = basementDialogue.GetComponent<BasementDialogue>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("Closed") && !hasOpened)
        {
            hasOpened = true;
            StartCoroutine(dialogue.DoMonsterCutscene());
        }
    }
}
