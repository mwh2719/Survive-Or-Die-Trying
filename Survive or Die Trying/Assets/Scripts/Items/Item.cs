using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will make this show up in the asset context menu so people can quickly make more food instances.
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    /// <summary>
    /// Name to display on screen to the Player.
    /// </summary>
    new public string name = "New Item";
    
    /// <summary>
    /// Link to the GameObject prefab of the item for spawning in the world.  
    /// </summary>
    public GameObject physicalPrefab = null;

    /// <summary>
    /// The type of item this is. Food, Material, or Tool.
    /// </summary>
    [SerializeField]
    private Categories.ITEM_TYPE type;
    public Categories.ITEM_TYPE Type
    {
        get { return type; }
    }

    /// <summary>
    /// The sprite of the item to show on item screens.
    /// </summary>
    [SerializeField]
    private Sprite image;
    public Sprite GetItemImage()
    {
        return image;
    }

    /// <summary>
    /// Whether this item has a use action or not
    /// </summary>
    /// <returns>Can item be used</returns>
    public virtual bool CanBeUsed()
    {
        return false;
    }

    /// <summary>
    /// Runs the use action of this item. Return true if the action was successful or false if it failed.
    /// </summary>
    /// <param name="user">The user invoking the use action of the item</param>
    /// <returns>Whether the using of the item was successful or not</returns>
    public virtual bool Use(GameObject user)
    {
        // Do nothing
        return false;
    }
}
