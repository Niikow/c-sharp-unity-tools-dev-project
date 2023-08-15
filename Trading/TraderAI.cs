using System;
using UnityEngine;
using UnityEngine.AI;

/*
 *  This class controls the trader AI movement and starts the trade
 */

public class TraderAI : MonoBehaviour
{
    // AI Movement
    private NavMeshAgent agent = null!;
    private Transform target = null!;

    [SerializeField] private float walkSpeed;

    [SerializeField] private Transform[] waypoints = Array.Empty<Transform>();
    private int currentWaypoint = 0;
    private Vector3 targetWaypoint;

    [SerializeField] private float stoppingDistance = 3f;

    // Trading
    [SerializeField] private GameObject inventoryUI, equipmentUI, tradingUI;

    public bool isTrading = false;

    private void Awake()
    {
        inventoryUI = GameObject.Find("Inventory");
        equipmentUI = GameObject.Find("Equipment");
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.Instance.player.transform;
        agent.speed = walkSpeed;
        agent.stoppingDistance = stoppingDistance;

        UpdateDestination();

        tradingUI = GameObject.Find("Trading");
        tradingUI.SetActive(false);
    }

    // AI movement between waypoints
    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= agent.stoppingDistance)
        {
            agent.SetDestination(transform.position);
            FaceTarget();
        }

        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                NextWaypoint();
                UpdateDestination();
            }
        }
    }

    // Sets the next waypoint as the target
    private void UpdateDestination()
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

    // Opens the trade when called from PlayerInteractions.cs
    // Opens players inventory and equipment UI
    internal void OpenTrade()
    {
        isTrading = true;
        inventoryUI.gameObject.SetActive(true);
        equipmentUI.gameObject.SetActive(true);
        tradingUI.SetActive(true);
        Debug.Log("Open Trade");
    }

    // Closes the trade panel
    internal void CloseTrade()
    {
        isTrading = false;
        inventoryUI.gameObject.SetActive(false);
        equipmentUI.gameObject.SetActive(false);
        tradingUI.SetActive(false);
    }
}
