using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerObject : ItemObject
{
    public float amountOfEnergyRestored;

    public HealerObject(ItemName _itemName, int _amount)
    {
        itemName = _itemName;
        itemType = ItemType.Healer;
        amount = _amount;
        amountOfEnergyRestored = 3f;
    }
}
