using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // Inventory of items
    private List<ItemObject> itemList;

    public event Action OnCherryCollected;

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
        itemList = new List<ItemObject>();
    }

    public void AddItem(ItemObject item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemObject itemObjectInInventory in itemList)
            {
                if (itemObjectInInventory.itemName == item.itemName)
                {
                    itemObjectInInventory.amount += item.amount;
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
        if (item.itemName == ItemName.Cherry)
        {
            OnCherryCollected?.Invoke();
        }
    }

    public List<ItemObject> GetItemList()
    {
        return itemList;
    }

    public int CountItems()
    {
        return itemList.Count;
    }
}
