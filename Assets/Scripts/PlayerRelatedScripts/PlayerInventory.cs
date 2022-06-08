using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // Inventory of items
    //private List<ItemObject> itemList;
    private HealerObject cherryHolder = new HealerObject(ItemName.Cherry, 0);
    private List<string> levelKeys = new();

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
        //itemList = new List<ItemObject>();
    }

    public int ReduceAmountOfItem(ItemName itemName, int amountToReduce)
    {
        switch (itemName)
        {
            case ItemName.Cherry:
                {
                    if (cherryHolder.amount == 0)
                    {
                        return 0;
                    }
                    else if (cherryHolder.amount < amountToReduce)
                    {
                        int temp = cherryHolder.amount;
                        cherryHolder.amount = 0;
                        return temp;
                    }
                    else
                    {
                        cherryHolder.amount -= amountToReduce;
                        return amountToReduce;
                    }
                }
        }
        return 0;
    }

    public bool AddAmountOfItem(ItemName itemName, int amountToAdd)
    {
        switch (itemName)
        {
            case ItemName.Cherry:
                {
                    cherryHolder.amount += amountToAdd;
                    return true;
                }
        }
        return false;
    }

    public void AddKey(string levelName)
    {
        for (int i = 0; i < levelKeys.Count; i++)
        {
            if (levelKeys[i] == levelName)
            {
                return;
            }
        }
        levelKeys.Add(levelName);
    }

    public bool HasPlayerSpecificKey(string levelName)
    {
        for (int i = 0; i < levelKeys.Count; i++)
        {
            if (levelKeys[i] == levelName)
            {
                return true;
            }
        }
        return false;
    }

    public ItemObject GetItem(ItemName itemName)
    {
        switch (itemName)
        {
            case ItemName.Cherry:
                {
                    HealerObject cherryHolderCopy = cherryHolder;
                    return cherryHolderCopy;
                }
        }
        return null;
    }

    public void AddItem(ItemObject item)
    {
        switch (item.itemName)
        {
            case ItemName.Cherry:
                {
                    if (item is HealerObject)
                    {
                        HealerObject hOHolder = (HealerObject) item;
                        cherryHolder.amount += hOHolder.amount;
                        Debug.Log("New Amount of Cherries: " + cherryHolder.amount);
                        OnCherryCollected?.Invoke();
                    }
                    break;
                }
        }
        /*if (item.IsStackable())
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
                ItemObject newItemObj = ScriptableObject.CreateInstance<ItemObject>();
                newItemObj.amount = item.amount;
                newItemObj.itemName = item.itemName;
                newItemObj.itemType = item.itemType;
                itemList.Add(newItemObj);
            }
        }
        else
        {
            ItemObject newItemObj = ScriptableObject.CreateInstance<ItemObject>();
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
        }*/
    }

    public HealerObject cherryObject = new HealerObject(ItemName.Cherry, 1);
}
