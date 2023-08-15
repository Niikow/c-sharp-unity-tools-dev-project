using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class enables the player to interact with game objects such as equipment 
 */
public class PlayerInteractions : MonoBehaviour
{
    private GameObject? interactableObject;

    [SerializeField]
    private GameObject equipmentUI = null!;

    private float lastAttackTime;

    [SerializeField]
    private float coneRadius = 4f,
        coneAngle = 70f,
        interactionRadius = 1f,
        tradingInteractionRadius = 4f;

    [SerializeField] private LayerMask enemyLayer;

    private Combat combat = null!; // late initialised in start so we surpress null

    private void Start()
    {
        combat = GetComponent<Combat>();
    }

    void Update()
    {
        PickUpItem();
        Trade();

        if (!equipmentUI.activeSelf && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    // Pick up an item from the environment
    void PickUpItem()
    {
        // Check if player is within radius of interactable object
        var hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Item")) continue;
            interactableObject = hitCollider.gameObject;
            break;
        }

        // If player presses interaction key, pick up the interactable object
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.GetComponent<ItemPickUp>().PickUpItem();

            interactableObject = null;
        }
    }

    // Interact with trader
    // Calls Trade() in TraderAI.cs
    void Trade()
    {
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Trader").transform.position) < 4f)
        {
            TraderAI traderAI = GameObject.FindWithTag("Trader").GetComponent<TraderAI>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (traderAI.isTrading)
                {
                    traderAI.CloseTrade();
                    return;
                }

                traderAI.OpenTrade();
            }
        }
    }

    // Interact with enemy by attacking
    void Attack()
    {
        // Creates cone in front of player in which they can attack the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coneRadius, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            Vector3 dirToEnemy = hitCollider.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);
            if (angle < coneAngle)
            {
                Enemy enemy = hitCollider.transform.parent.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.beenHit();
                }
            }
        }
    }

}