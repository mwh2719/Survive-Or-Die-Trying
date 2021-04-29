using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Categories;

public class PlayerHealth : EntityHealth
{

    /// <summary>
    /// Adds player specific modifiers to damage taken.
    /// </summary>
    /// <param name="damage">Raw damage to deal. Must be positive</param>
    /// <param name="type">The type of damage dealt</param>
    public override void TakeDamage(float damage, DAMAGE_TYPE type)
    {
        //Add armor damage reduction here
        if(type == DAMAGE_TYPE.KINETIC)
        {
            damage *= 0.8f;
        }

        //Adding chance of infection from animal bites
        if(type == DAMAGE_TYPE.ANIMAL)
        {
            this.GetComponent<PlayerCharacterController>().PlayAttackedSound();
            if (Random.Range(0,100) <= 25)
            {
                //Adding infection damage
                base.TakeDamage(5, DAMAGE_TYPE.INFECTION);
            }
        }

        base.TakeDamage(damage, type);
    }

    /// <summary>
    /// Call when healing the entity. Can do player specific modifiers here.
    /// </summary>
    /// <param name="heal">How much to heal. Must always be positive</param>
    /// <param name="type">The kind of healing that is occuring</param>
    public override void Heal(float heal, HEAL_TYPE type)
    {
        base.Heal(heal, type);
    }
}
