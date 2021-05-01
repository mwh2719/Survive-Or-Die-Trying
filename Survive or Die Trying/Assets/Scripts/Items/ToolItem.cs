using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will make this show up in the asset context menu so people can quickly make more instances.
[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Tool")]
public class ToolItem : Item
{
    [SerializeField] protected float attackRange;
    [SerializeField] private float hitDamage;

    /// <summary>
    /// Tools can be equipped.
    /// </summary>
    /// <returns>Always true</returns>
    public override bool CanBeUsed()
    {
        return true;
    }

    /// <summary>
    /// Equip the tool
    /// </summary>
    /// <param name="user">The equipper</param>
    /// <returns>if it was equipped</returns>
    public override bool Use(GameObject user)
    {
        user.GetComponent<PlayerEquipped>().SetEquippedItem(this);
        return true;
    }

    /// <summary>
    /// Use the tool in the world 
    /// </summary>
    /// <param name="user">The user</param>
    public override void InWorldUse(GameObject user)
    {
        user.GetComponent<PlayerCharacterController>().playerAnim.SetTrigger("Punch");

        RaycastHit hit;
        AnimalBehavior animalHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange))
        {
            if (hit.transform.TryGetComponent(out animalHit))
            {
                animalHit.TakeDamage(hitDamage);
            }
        }
    }
}
