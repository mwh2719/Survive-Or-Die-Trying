using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CraftingInput : MonoBehaviour
{
    List<InventorySlot> selectedSlots = new List<InventorySlot>();
    string currentRecipe = "";
    public GameObject craftOutputPanel;
    public InventorySlot craftOuputSlot;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        selectedSlots.Add(other.GetComponent<InventorySlot>());

        // Sort the slots by item name so order of dropping items into crafting doesn't matter
        selectedSlots.Sort((slot1, slot2) => string.Compare(
            slot1.GetItem().physicalPrefab.name, 
            slot2.GetItem().physicalPrefab.name
        ));

        // Converts the sorted selected slots into a String of their item names for dictionary
        currentRecipe = selectedSlots
            .Select(slot => slot.GetItem().physicalPrefab.name)
            .Aggregate((x, y) => x + y);

        // Gets resultant recipe and displays it.
        string resultantRecipe = craftingRecipes[currentRecipe];
        if (resultantRecipe != null)
        {
            craftOutputPanel.SetActive(true);
            GameObject loadedPrefab = (GameObject)Instantiate(Resources.Load(resultantRecipe));
            craftOuputSlot.AddItem(loadedPrefab.GetComponent<ItemPickupable>().item);
        }
        else
        {
            craftOuputSlot.ClearSlot();
            craftOutputPanel.SetActive(false);
        }

        Debug.Log("Triggered entered");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        selectedSlots.Remove(other.GetComponent<InventorySlot>());
        Debug.Log("Triggered removed");
    }


    ////////////////////////////////////////////////////
    //Crafting dictionary
    
    public static Dictionary<string, string> craftingRecipes = new Dictionary<string, string>() {
        { "ApplePear", "Corn" }
    };

}
