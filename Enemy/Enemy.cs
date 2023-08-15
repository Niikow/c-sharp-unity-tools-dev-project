using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class for enemy AI controlling combat behaviour
 */

public class Enemy : MonoBehaviour
{
    private PlayerManager playerManager = null!;
    private Stats enemyStats = null!;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        enemyStats = GetComponent<Stats>();
    }
    public void beenHit()
    {
        Combat playerCombat = playerManager.player.GetComponent<Combat>();
        if (playerCombat != null) playerCombat.Attack(enemyStats);

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != this) enemy.Investigate(transform.position, 5f);
        }
    }
}
