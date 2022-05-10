using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    Cherry
}

public enum ItemType
{
    Healer
}

public class ItemObject : ScriptableObject
{
    public ItemName itemName;
    public ItemType itemType;
    public int amount;

    public bool IsStackable()
    {
        return itemName switch
        {
            ItemName.Cherry => true,
            _ => false,
        };
    }
}
