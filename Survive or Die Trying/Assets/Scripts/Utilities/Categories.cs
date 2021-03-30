using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Categories : MonoBehaviour
{
    public enum DAMAGE_TYPE {
        KINETIC,
        HEAT,
        COLD,
        DROWNING,
        HUNGER,
        THIRST,
        POISON,
        ANIMAL,
        INFECTION
    }

    public enum HEAL_TYPE
    {
        BANDAGE,
        REST,
        MEDICINE
    }

    public enum ITEM_TYPE
    {
        FOOD,
        TOOL,
        MATERIAL
    }

    public enum INVENTORY_CATEGORY
    {
        ALL,
        FOOD,
        TOOL,
        MATERIAL
    }
}
