using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Food")]
public class EdibleItem : Item
{
    public float healthRestoration = 0;
    public float hungerRestoration;
    public float thirstRestoration;

    public override bool CanBeUsed()
    {
        return true;
    }

    public override bool Use(GameObject user)
    {
        // Checks if it is usable by entity
        PlayerMainController userStats = user.GetComponent<PlayerMainController>();
        EntityHealth playerHealth = user.GetComponent<EntityHealth>();
        if (userStats != null)
        {
            if(healthRestoration > 0)
            {
                playerHealth.Heal(healthRestoration, Categories.HEAL_TYPE.MEDICINE);
            }
            else
            {
                playerHealth.TakeDamage(Math.Abs(healthRestoration), Categories.DAMAGE_TYPE.POISON);
            }
            userStats.CurrentHunger += hungerRestoration;
            userStats.CurrentThirst += thirstRestoration;
            return true;
        }
        else
        {
            Debug.LogError("Unable to consume item because user does not have PlayerMainController on");
        }
        return false;
    }
}
