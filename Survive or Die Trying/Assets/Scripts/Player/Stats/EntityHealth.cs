using System.Collections.Generic;
using UnityEngine;
using static Categories;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    public float maxHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        protected set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }
  
    /**
     * A list of pairs where each pair is the total amount of remaining damage/healing to deal and how much damage/heal to each each update.
     * True for healing and false for damaging.
     * Calculate the amount to deal each update with   totalDamage / (damageDuration / Time.fixedDeltaTime)
     */
    private List<LongTermEffect<float, float, bool>> longTermEffectList = new List<LongTermEffect<float, float, bool>>();
    public class LongTermEffect<T1, T2, T3>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public T3 Third { get; set; }

        public LongTermEffect(T1 first, T2 second, T3 third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }


    /**
     * For dealing damage over time
     */
    private void FixedUpdate()
    {
        for(int i = longTermEffectList.Count - 1; i >= 0; i--)
        {
            LongTermEffect<float, float, bool> currentDamagePair = longTermEffectList[i];
            CurrentHealth -= currentDamagePair.Second;
            float newTotal = currentDamagePair.First - currentDamagePair.Second;

            // Removed finished long-term damage
            if(newTotal <= 0)
            {
                longTermEffectList.RemoveAt(i);
                continue;
            }
            // Update long-term damage
            else
            {
                currentDamagePair.First -= currentDamagePair.Second;
            }
        }
    }

    
    /// <summary>
    /// Call when harming the entity with long term damage.
    /// </summary>
    /// <param name="damage">How much total damage to deal. Must always be positive</param>
    /// <param name="damageDuration">Length of time (seconds) the total damage will be divide up over. Must always be positive</param>
    /// <param name="type">The kind of damaging that is occuring</param>
    public virtual void TakeDamageOverTime(float damage, float damageDuration, DAMAGE_TYPE type)
    {
        // Must always be positive
        Debug.Assert(damage >= 0);
        Debug.Assert(damageDuration >= 0);
        float amountOfUpdateCycles = (damageDuration / Time.fixedDeltaTime);
        longTermEffectList.Add(new LongTermEffect<float, float, bool>(damage, damage / amountOfUpdateCycles, false));
    }


    /// <summary>
    /// Call when healing the entity over long term.
    /// </summary>
    /// <param name="damage">How much total damage to deal. Must always be positive</param>
    /// <param name="damageDuration">Length of time (seconds) the total damage will be divide up over. Must always be positive</param>
    /// <param name="type">The kind of damaging that is occuring</param>
    public virtual void HealOverTime(float damage, float damageDuration, HEAL_TYPE type)
    {
        // Must always be positive
        Debug.Assert(damage >= 0);
        Debug.Assert(damageDuration >= 0);
        float amountOfUpdateCycles = (damageDuration / Time.fixedDeltaTime);
        longTermEffectList.Add(new LongTermEffect<float, float, bool>(damage, damage / amountOfUpdateCycles, true));
    }


    /// <summary>
    /// Call when harming the entity.
    /// </summary>
    /// <param name="damage">How much to damage. Must always be positive</param>
    /// <param name="type">The kind of damaging that is occuring</param>
    public virtual void TakeDamage(float damage, DAMAGE_TYPE type)
    {
        // Must always be positive
         Debug.Assert(damage >= 0);
        CurrentHealth -= damage;
    }

    /// <summary>
    /// Call when healing the entity.
    /// </summary>
    /// <param name="heal">How much to heal. Must always be positive</param>
    /// <param name="type">The kind of healing that is occuring</param>
    public virtual void Heal(float heal, HEAL_TYPE type)
    {
        // Must always be positive
        Debug.Assert(heal >= 0);
        CurrentHealth += heal;
    }

    /// <summary>
    /// Puts entity's health back to maximum
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
