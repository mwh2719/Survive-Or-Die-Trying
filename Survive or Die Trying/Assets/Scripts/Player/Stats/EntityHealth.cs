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

    /// <summary>
    /// Call when harming the entity.
    /// </summary>
    /// <param name="heal">How much to damage. Must always be positive</param>
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
