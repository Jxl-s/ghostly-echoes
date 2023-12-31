using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNavMeshScript : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public Animator animator;                       //  Animator component
    public float startWaitTime = 0.5f;                 //  Wait time of every action
    public float startTeleportTime = 2;             //  Wait time of every teleport
    public float startAttackTime = 3;             //  Wait time of every teleport
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speed = 5;                         //  Walking speed, speed in the nav mesh agent  
    float radius = 0.03f;                            //  Teleport radius                     
    public float viewRadius = 5;                   //  Radius of the enemy view
    public float viewAngle = 180;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacles with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calculate the a minumun and a maximum raycast when hits something       

    public List<Transform> waypoints;                  //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to
    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy
    float m_WaitTime;                               //  Variable of the wait time that makes the delay
    float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_IsPatrol;                                //  If the enemy is patrol, state of patrolling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player
    float m_TeleportTime;                           //  Variable of the wait time between teleports
    float m_AttackTime;                             //  Variable of the wait time between attacks
    public string monsterTag;

    bool monsterStarted = false;
    void StartMonster()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;
        m_AttackTime = 0;

        animator = GetComponent<Animator>();

        playerMask = LayerMask.GetMask("Player");
        obstacleMask = LayerMask.GetMask("Obstacle");

        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("waypoint");
        waypoints = new List<Transform>();
        foreach (GameObject obj in objects)
        {
            Debug.Log(obj.name);
            if(obj.name.Contains(monsterTag) || string.IsNullOrEmpty(monsterTag))
                waypoints.Add(obj.transform);
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
    }

    private void Update()
    {
        // Only chase if the monster is active
        if (!GameManager.Instance.MonsterActive)
        {
            return;
        }

        if (!monsterStarted)
        {
            monsterStarted = true;
            StartMonster();
        }

        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!m_IsPatrol)
        {
            Chasing();
            if (m_AttackTime > 0)
            {
                m_AttackTime -= Time.deltaTime;
            }
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!m_CaughtPlayer)
        {
            if (m_TeleportTime <= 0)
            {
                Teleport();
            }
            Move(speed);
            navMeshAgent.SetDestination(m_PlayerPosition);
            m_TeleportTime -= Time.deltaTime;
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speed);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 1.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(speed);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Debug.Log(navMeshAgent.destination);
                    Move(speed);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Debug.Log("stopped patrolling");
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Count;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void Stop()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("Running", false);
        navMeshAgent.speed = 0;
    }
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        animator.SetBool("Running", true);
        navMeshAgent.speed = speed;
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speed);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_AttackTime <= 0)
        {
            animator.SetTrigger("Attack");
            switch(monsterTag){
                case "monster_1":
                    AudioManager.Instance.PlayEffect(1);
                    break;
                case "monster_2":
                    AudioManager.Instance.PlayEffect(2);
                    break;
                default:
                    AudioManager.Instance.PlayEffect(1);
                    break;        
            }
            GameManager.Instance.ReduceHealth(20);
            Teleport();
            m_AttackTime = startAttackTime;
        }
    }

    /*
    * Teleport the monster within a circle radius   
    */
    private void Teleport()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        Vector2 point = Random.insideUnitCircle * radius;      //  set the destination of the enemy to the player location
        this.transform.position = new Vector3(x * (point.x + 1), y, z * (point.y + 1));
        m_TeleportTime = startTeleportTime;
    }
}
