using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollow : MonoBehaviour
{

    public NavMeshAgent agent;
    public Rigidbody rBody;
    public bool patroller;
    private bool currentlyPatrolling;
    private bool currentlyChasing;
    private Vector3 playerDirection;
    private float playerDistance;
    private GameObject player;
    public float fieldOfView;
    public float detectDistance;
    public float sightDistance;
    private RaycastHit hit;
    private GameObject[] patrolTargets;
    private Light enemyLight;

    public bool debug;

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        rBody = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerCapsule");

        enemyLight = GetComponentInChildren<Light>();
        enemyLight.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        //If enemy is set as a patroller and does not have a current action, calls onPatrol function
        if (patroller && !currentlyPatrolling && !currentlyChasing)
        {
            onPatrol();
        }

        //Gets direction vector of player from the enemy's location, then gets the angle the player is at
        playerDirection = player.transform.position - transform.position;
        float playerAngle = Vector3.Angle(playerDirection, transform.forward);

        //Gets distance between player and enemy
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        
        //If the player is within the set field of view angle, and within the sight range of the enemy
        if (playerAngle < fieldOfView && playerDistance < sightDistance)
        {

            //Creates a raycast, if ray hits player (direct line of sight, no walls) then enemy begins chasing
            if (Physics.Raycast(transform.position, playerDirection, out hit) && hit.collider.CompareTag("Player"))
            { 
                chasePlayer();
            }

        //If the player is within the set detection range (close to the enemy)
        } else if (playerDistance < detectDistance)
        {
            chasePlayer();

        //If the player is not in sight or detection ranges, if the enemy is not a patroller ResetPath is called to stop chasing
        } else if (!patroller)
        {
            agent.ResetPath();
        }


    }


    void chasePlayer()
    {
        //Sets Navmesh agent destination to current player lcoation
        agent.SetDestination(player.transform.position);
        currentlyPatrolling = false;
        currentlyChasing = true;
        enemyLight.color = Color.red;
    }

    void onPatrol()
    {
        //Gets a list of GameObjects which provide points
        patrolTargets = GameObject.FindGameObjectsWithTag("PointTarget");

        //Selects a random object from the above list for the agent to navigate to, simulating a patrol
        agent.SetDestination(patrolTargets[Random.Range(0, patrolTargets.Length)].transform.position);
        currentlyPatrolling = true;
        enemyLight.color = Color.yellow;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //If enemy is hit by projectile, "kills" it
        if (collision.gameObject.CompareTag("Projectile"))
        {
            agent.isStopped = true;
            rBody.freezeRotation = false;
            enemyLight.enabled = false;
        }

        //If enemy collides with PointTarget object, current patrol route has finished 
        if (collision.gameObject.CompareTag("PointTarget"))
        {
            currentlyPatrolling = false;
        }
    }
}
