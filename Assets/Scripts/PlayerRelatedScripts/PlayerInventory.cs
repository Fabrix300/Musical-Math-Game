using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // Inventory of items
    private List<ItemWorld> itemList;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        itemList = new List<ItemWorld>();
    }

    public void AddItem(ItemWorld item)
    {
        if (item.item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemWorld inventoryItem in itemList)
            {
                if (inventoryItem.item.itemName == item.item.itemName)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
    }

    public int CountItems()
    {
        return itemList.Count;
    }
}
