using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Categories;

public class PlayerHealth : EntityHealth
{

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
            if(Random.Range(0,100) <= 25)
            {
                //Adding infection damage
                TakeDamage(5, DAMAGE_TYPE.INFECTION);
            }
        }

        base.TakeDamage(damage, type);
    }

    public override void Heal(float heal, HEAL_TYPE type)
    {
        base.Heal(heal, type);
    }
}
