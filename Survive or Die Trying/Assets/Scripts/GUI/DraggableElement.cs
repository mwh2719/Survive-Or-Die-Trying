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
        BoxCollider2D slotCollidier = GetComponent<BoxCollider2D>();
        BoxCollider2D craftableAreaCollidier = craftableArea.GetComponent<BoxCollider2D>();
        if (slotCollidier.IsTouching(craftableAreaCollidier))
        {
            gameObject.transform.parent = craftableArea.transform;
        }
        else
        {
            gameObject.transform.parent = craftInventory.transform;
        }
    }
}
