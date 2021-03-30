using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source : https://www.youtube.com/watch?v=YLhj7SfaxSE
public class InventoryController : MonoBehaviour
{
    /// <summary>
    /// Should always be a child panel and not the same object as InventoryController is on.
    /// Otherwise we will disable outselves and InventoryController will no longer work.
    /// </summary>
    public GameObject inventoryCanvas;
    public Transform itemsParent;
    PlayerInventory playerInventory;
    InventorySlot[] slots;

    /// <summary>
    /// setup canvas by hiding the screen, 
    /// getting the player instances,
    /// setting the callback for updating the UI when inventory changes,
    /// and obtaining all the slots to change.
    /// 
    /// This reduces the amount of setup we have to do manually in inspector by a ton.
    /// </summary>
    void Start()
    {
        inventoryCanvas.SetActive(false);
        playerInventory = PlayerInventory.instance;
        playerInventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }


    /// <summary>
    /// Will turn on/off inventory screen based on inputs.
    /// Note: pause screen will always exit the current screen and 
    ///       other screens cannot activate while in inventory screen.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            // Do not open inventory screen when in another screen
            if(PauseController.inGui && !inventoryCanvas.activeSelf)
            {
                return;
            }

            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            PauseController.ForcePauseState(inventoryCanvas.activeSelf, inventoryCanvas.activeSelf);
            if (inventoryCanvas.activeSelf)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].ShowActions(false); // hide all action buttons
                }
            }
        }

        // Exit inventory screen
        if (Input.GetButtonDown("Pause"))
        {
            inventoryCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// Will reset slots to show the correct items currently in inventory.
    /// 
    /// ALWAYS ADD THIS AS A CALLBACK ON PlayerInventory.instance.onItemChangedCallback 
    /// so that the UI will update correctly upon any change to inventory.
    /// </summary>
    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < playerInventory.items.Count)
            {
                slots[i].AddItem(playerInventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
