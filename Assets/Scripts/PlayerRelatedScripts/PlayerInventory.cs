using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // Inventory of items
    private List<ItemWorld> itemList;

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
                Debug.Log(item.amount);
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        if (item.item.itemName == Item.ItemName.Cherry)
        {
            OnCherryCollected?.Invoke();
        }
    }

    public List<ItemWorld> GetItemList()
    {
        return itemList;
    }

    public int CountItems()
    {
        return itemList.Count;
    }
}
