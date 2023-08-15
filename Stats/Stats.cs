using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Base class for defining stats for all characters in the game
 */

public class Stats : MonoBehaviour
{

    public Stat Health = new();

    public Stat Damage = new();
    public Stat Armour = new();
    public Stat Stamina = new();

    public Stat AttackSpeed = new();

    public bool BlockStaminaRegen;

    private float elapsed;

    // Used for testing stat
    // Start stamina regeneration
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) DamageTaken(10);
        if (Input.GetKeyDown(KeyCode.Z)) Debug.Log("health: " + Health);

        elapsed += Time.deltaTime;
        if (elapsed >= 1)
        {
            elapsed -= 1;
            if (!BlockStaminaRegen)
                RegenStamina();

        }
    }

    // Set character stats based on equipment
    protected void Register(EquipmentManager equipment)
    {
        Health.SetSelector(item => item.healthModifier, equipment);
        Armour.SetSelector(item => item.armourModifier, equipment);
        Stamina.SetSelector(item => item.staminaModifier, equipment);
        Damage.SetSelector(item => item.damageModifier, equipment);
    }

    // Character regenerates 2 stamina points every second
    private void RegenStamina()
    {
        Stamina.Value += 2;
    }

    // Damage taken from combat
    public void DamageTaken(int damage)
    {

        float ratio = Math.Clamp((float)damage / (float)Armour, 0, 1);
        int before = Health;
        Health.Value -= (int)(damage * ratio);
        int after = Health;
        Debug.Log($"{transform.name} took {damage} damage, health {before} => {after}");

        if (Health == 0) Die();
    }

    // Override Die method in subclasses
    public virtual void Die()
    {
        Debug.Log(transform.name + " died");
    }

    // Reduce stamina depending on action
    public bool DrainStamina(int points)
    {
        if (points > Stamina) return false;
        Stamina.Value -= points;
        return true;
    }

}
