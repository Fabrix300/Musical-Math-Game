using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealerItem", menuName = "Inventory System/Items/Healer")]
public class HealerObject : ItemObject
{
    public float amountOfEnergyRestored;

    private void Awake()
    {
        itemType = ItemType.Healer;
    }
}
