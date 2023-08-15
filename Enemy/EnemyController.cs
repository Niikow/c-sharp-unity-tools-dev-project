using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

/*
 * Class for enemy AI controls and behaviour
 */
public class EnemyController : MonoBehaviour
{
    public float detectionRadius;
    public float investigationRadius;

    private Transform target = null!;
    private NavMeshAgent agent = null!;

    private Combat combat = null!;

    public Transform[] waypoints = Array.Empty<Transform>();
    private int currentWaypoint = 0;
    private Vector3 targetWaypoint;

    [SerializeField]
    private float
        patrolSpeed = 4f,
        chaseSpeed = 6f,
        stoppingDistance = 1f,
        stoppingDistancePlayer = 2f;

    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<Combat>();

        agent.speed = patrolSpeed;
        agent.stoppingDistance = stoppingDistance;
        // agent.autoBraking = true;

        UpdateDestination();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= detectionRadius)
        {
            agent.SetDestination(target.position);
            agent.stoppingDistance = stoppingDistancePlayer;
            agent.speed = chaseSpeed;

            if (distance <= agent.stoppingDistance)
            {
                agent.SetDestination(transform.position);
                FaceTarget();

                Stats playerStats = target.GetComponent<Stats>();
                if (playerStats != null)
                {
                    combat.Attack(playerStats);
                }
            }
        }
        // Patrolling
        else
        {
            agent.stoppingDistance = stoppingDistance;
            agent.speed = patrolSpeed;

            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                NextWaypoint();
                UpdateDestination();
            }
        }
    }

    // Sets the next waypoint as the target
    void UpdateDestination()
    {
        targetWaypoint = waypoints[currentWaypoint].position;
        agent.SetDestination(targetWaypoint);
    }

    // Called when the enemy reaches a waypoint
    void NextWaypoint()
    {
        currentWaypoint++;
        if (currentWaypoint == waypoints.Length) currentWaypoint = 0;
    }

    // Rotates the enemy to face the player
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    // Debug visualisation for enemy "detection" radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Called by the player when they make a noise
    public void Investigate(Vector3 position, float range)
    {
        if (Vector3.Distance(position, transform.position) <= investigationRadius)
        {
            Vector3 randPoint = position + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit investigationPoint;
            if (NavMesh.SamplePosition(randPoint, out investigationPoint, range, NavMesh.AllAreas))
            {
                agent.SetDestination(investigationPoint.position);
                agent.speed = chaseSpeed;
            }
        }
    }
}
