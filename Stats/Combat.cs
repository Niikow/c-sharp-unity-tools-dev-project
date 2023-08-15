using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class controls the combat behaviour of all characters
 */

[RequireComponent(typeof(Stats))]
public class Combat : MonoBehaviour
{
    private Stats myStats = null!;
    private float lastAttackTime;
    private System.Random rng = new();

    private void Start()
    {
        myStats = GetComponent<Stats>();
    }

    // Attack the target
    public void Attack(Stats targetStats)
    {
        float attackSpeed = myStats.AttackSpeed;
        float timeBetweenAttacks = 1f / attackSpeed;

        if (Time.time > lastAttackTime + timeBetweenAttacks && myStats.DrainStamina(5))
        {
            // Attack has a chance to deal bonus damage
            int damage = myStats.Damage;
            if (rng.Next() % 10 == 0)
            { //crit 10%
                damage = (int)(damage * 1.5);
            }
            // Damage the target and reset the time between last attack
            targetStats.DamageTaken(myStats.Damage);
            lastAttackTime = Time.time;
        }
    }
}
