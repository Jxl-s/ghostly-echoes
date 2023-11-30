using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNavMeshScript : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    NavMeshAgent agent;
    Transform[] waypoints; 
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
