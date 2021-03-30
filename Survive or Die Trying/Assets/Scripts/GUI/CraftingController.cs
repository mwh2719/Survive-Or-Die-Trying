using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    /// <summary>
    /// Should always be a child panel and not the same object as CraftingController is on.
    /// Otherwise we will disable outselves and CraftingController will no longer work.
    /// </summary>
    public GameObject craftingCanvas;
    public GameObject craftingArea;
    public GameObject prefabSlot;
    public Transform itemsParent;
    PlayerInventory playerInventory;
    List<InventorySlot> slots;

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
        craftingCanvas.SetActive(false);
        playerInventory = PlayerInventory.instance;
        playerInventory.onItemChangedCallback += UpdateUI;

        slots = new List<InventorySlot>();
        UpdateUI();
    }
    
    /// <summary>
    /// Will turn on/off craft screen based on inputs.
    /// Note: pause screen will always exit the current screen and 
    ///       other screens cannot activate while in crafting screen.
    /// </summary>
    void Update()
    {
        if(Input.GetButtonDown("Crafting"))
        {
            // Do not open crafting screen when in another screen
            if (PauseController.inGui && !craftingCanvas.activeSelf)
            {
                return;
            }
            craftingCanvas.SetActive(!craftingCanvas.activeSelf);
            PauseController.ForcePauseState(craftingCanvas.activeSelf, craftingCanvas.activeSelf);
        }

        // Exit crafting screen
        if (Input.GetButtonDown("Pause"))
        {
            craftingCanvas.SetActive(false);
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
        ClearAllChildren(craftingArea.transform);
        ClearAllChildren(itemsParent.transform);

        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            GameObject newSlot = Instantiate(prefabSlot, new Vector3(0, 0, 0), Quaternion.identity);
            newSlot.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            newSlot.transform.SetParent(itemsParent.transform);
            InventorySlot slotInv = newSlot.GetComponent<InventorySlot>();
            if (slotInv != null)
            {
                slotInv.AddItem(playerInventory.items[i]);
            }
        }
    }

    /// <summary>
    /// Utility method to nuke out all children gameobjects
    /// </summary>
    /// <param name="transform">the parent who we remove all children off of</param>
    /// <returns>the childless parent gameobject</returns>
    static Transform ClearAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        return transform;
    }
}
