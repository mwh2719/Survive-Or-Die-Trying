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

    public virtual void TakeDamage(float damage, DAMAGE_TYPE type)
    {
        // Must always be positive
         Debug.Assert(damage >= 0);
        CurrentHealth -= damage;
    }

    public virtual void Heal(float heal, HEAL_TYPE type)
    {
        // Must always be positive
        Debug.Assert(heal >= 0);
        CurrentHealth += heal;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
