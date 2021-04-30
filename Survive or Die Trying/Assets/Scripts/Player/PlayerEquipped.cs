using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipped : MonoBehaviour
{
    [SerializeField] private float playerAttackRange;
    [SerializeField] private float punchDamage;
    private Camera camera;
    private GameObject equippedItem = null;
    public PlayerCharacterController playerController;

    void Start()
    {
        camera = Camera.main;
        playerController = GetComponent<PlayerCharacterController>();
    }

    /// <summary>
    /// Changes the equipped item on the player's hand
    /// </summary>
    /// <returns>The previous item that was equipped. CAN BE NULL</returns>
    public Item SetEquippedItem(Item itemToEquip)
    {
        Item olditem = null;
        if (equippedItem)
        {
            olditem = equippedItem.GetComponent<ItemPickupable>().item;
            Destroy(equippedItem);
        }
        if (itemToEquip)
        {
            // Should be attached to the player's hand bone instead but it's hard to see
            equippedItem = Instantiate(itemToEquip.physicalPrefab, camera.transform.position + camera.transform.forward + camera.transform.right - (camera.transform.up / 2), camera.transform.rotation, camera.transform);
        }
        else
        {
            equippedItem = null;
        }

        return olditem;
    }

    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            //Add code here to see if the player is holding an item
            if (equippedItem)
            {
                equippedItem.GetComponent<ItemPickupable>().item.InWorldUse(gameObject);
            }
            else
            {
                Punch();
            }
        }
        else if (Input.GetButtonDown("PutAway"))
        {
            PlayerInventory.instance.Add(SetEquippedItem(null));
        }
    }

    private void Punch()
    {
        playerController.playerAnim.SetTrigger("Punch");
        RaycastHit hit;
        AnimalBehavior animalHit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, playerAttackRange))
        {
            if (hit.transform.TryGetComponent(out animalHit))
            {
                animalHit.TakeDamage(punchDamage);
            }
        }
    }
}
