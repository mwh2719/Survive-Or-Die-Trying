using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Source: https://www.youtube.com/watch?v=Pc8K_DVPgVM
public class DraggableElement : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public GameObject craftableArea;
    public GameObject craftInventory;
    void Start()
    {
        craftableArea = GameObject.Find("CraftingArea");
        craftInventory = GameObject.Find("InventoryPanel");
    }

    /// <summary>
    ///  Remove slot from inventory panel or crafting area parent
    /// </summary>
    /// <param name="eventData">mouse position</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.parent = gameObject.transform.parent.parent;
    }

    /// <summary>
    ///  Move slot to mouse cursor
    /// </summary>
    /// <param name="eventData">mouse position</param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    /// <summary>
    ///  Add slot to crafting area or back to inventory panel parent
    /// </summary>
    /// <param name="eventData">mouse position</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Colliders won't work for UI elements and position/Rect Transform is based on anchors and parent position stuff.
        // So we construct a new Rect based on screen space itself to properly check if UI elements overlap.
        RectTransform slotRectTransform = GetComponent<RectTransform>();
        Vector3[] worldCorner = new Vector3[4];
        slotRectTransform.GetWorldCorners(worldCorner);
        Rect slotRect = new Rect(worldCorner[0], worldCorner[2] - worldCorner[0]);

        RectTransform craftableAreaRectTransform = craftableArea.GetComponent<RectTransform>();
        craftableAreaRectTransform.GetWorldCorners(worldCorner);
        Rect craftableAreaRect = new Rect(worldCorner[0], worldCorner[2] - worldCorner[0]);
        
        // Adds the slot to the proper parent so they can move the slot to the right place.
        if (slotRect.Overlaps(craftableAreaRect))
        {
            gameObject.transform.parent = craftableArea.transform;
            craftableArea.GetComponent<CraftingInput>().UpdateCraftItem(GetComponent<InventorySlot>());
        }
        else
        {
            gameObject.transform.parent = craftInventory.transform;
            craftableArea.GetComponent<CraftingInput>().RemoveCraftItem(GetComponent<InventorySlot>());
        }
    }
}
