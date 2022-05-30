using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    Cherry,
    None
}

public enum ItemType
{
    Healer
}

public class ItemObject
{
    public ItemName itemName;
    public ItemType itemType;
    public int amount;
}
