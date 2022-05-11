using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerObject : ItemObject
{
    public float amountOfEnergyRestored;

    public HealerObject(ItemName _itemName, int _amount, float _amountOfEnergyRestored)
    {
        itemName = _itemName;
        itemType = ItemType.Healer;
        amount = _amount;
        amountOfEnergyRestored = _amountOfEnergyRestored;
    }
}
