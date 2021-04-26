using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will make this show up in the asset context menu so people can quickly make more food instances.
[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Food")]
public class EdibleItem : Item
{
    public float healthRestoration = 0;
    public float hungerRestoration;
    public float thirstRestoration;
    [Header("Set to 0 to do instant effects")]
    public float longTermEffectDuration;

    /// <summary>
    /// Foods can always be eaten.
    /// </summary>
    /// <returns>Always true</returns>
    public override bool CanBeUsed()
    {
        return true;
    }

    /// <summary>
    /// Will consume the food and apply its effects to the eater.
    /// This includes hunger, thirst, and/or health.
    /// </summary>
    /// <param name="user">The eater</param>
    /// <returns>Whether the food was truly consumed</returns>
    public override bool Use(GameObject user)
    {
        // Checks if it is usable by entity
        PlayerMainController userStats = user.GetComponent<PlayerMainController>();
        EntityHealth playerHealth = user.GetComponent<EntityHealth>();
        if (userStats != null)
        {
            if(healthRestoration > 0)
            {
                if(longTermEffectDuration > 0)
                {
                    playerHealth.HealOverTime(healthRestoration, longTermEffectDuration, Categories.HEAL_TYPE.MEDICINE);
                }
                else
                {
                    playerHealth.Heal(healthRestoration, Categories.HEAL_TYPE.MEDICINE);
                }
            }
            else
            {
                if (longTermEffectDuration > 0)
                {
                    playerHealth.TakeDamageOverTime(Math.Abs(healthRestoration), longTermEffectDuration, Categories.DAMAGE_TYPE.POISON);
                }
                else
                {
                    playerHealth.TakeDamage(Math.Abs(healthRestoration), Categories.DAMAGE_TYPE.POISON);
                }
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
