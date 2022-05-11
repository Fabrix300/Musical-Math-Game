using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // Inventory of items
    //private List<ItemObject> itemList;
    private HealerObject cherryHolder = new HealerObject(ItemName.Cherry, 0, 6);

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

    public HealerObject cherryObject = new HealerObject(ItemName.Cherry, 1, 6);
}
