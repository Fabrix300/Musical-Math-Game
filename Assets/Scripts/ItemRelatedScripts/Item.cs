using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemName itemName;
    public ItemType itemType;
    //public int amount;

    public enum ItemName
    {
        Cherry
    }

    public enum ItemType
    {
        Healer
    }

    public bool IsStackable()
    {
        return itemName switch
        {
            ItemName.Cherry => true,
            _ => false,
        };
    }
}
