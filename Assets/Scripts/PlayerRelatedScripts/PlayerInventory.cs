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
    //public event Action OnCherryUsed;

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
                ItemObject newItemObj = new ItemObject();
                newItemObj.amount = item.amount;
                newItemObj.itemName = item.itemName;
                newItemObj.itemType = item.itemType;
                itemList.Add(newItemObj);
            }
        }
        else
        {
            ItemObject newItemObj = new ItemObject();
            newItemObj.amount = item.amount;
            newItemObj.itemName = item.itemName;
            newItemObj.itemType = item.itemType;
            itemList.Add(newItemObj);
            itemList.Add(item);
        }
        if (item.itemName == ItemName.Cherry)
        {
            Debug.Log("Cherry Collected");
            OnCherryCollected?.Invoke();
        }
    }

    public bool ReduceAmountOfItem(ItemName itemName)
    {
        foreach (ItemObject itemObjectInInventory in itemList)
        {
            if (itemObjectInInventory.itemName == itemName)
            {
                itemObjectInInventory.amount--;
                if (itemObjectInInventory.amount <= 0) 
                {
                    return itemList.Remove(itemObjectInInventory);
                }
                return true;
            }
        }
        return false;
    }

    public void ConsumeItem(ItemName itemName)
    {
        
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
