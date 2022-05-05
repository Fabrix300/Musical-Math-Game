using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryButton : MonoBehaviour
{
    public Text cherriesCounterText;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerInventory.instance;
        playerInventory.OnCherryCollected += RefreshCherriesText;
    }

    public void RefreshCherriesText()
    {
        int cherriesCount= 0;
        List<ItemWorld> itemList = playerInventory.GetItemList();
        foreach (ItemWorld itemWorld in itemList)
        {
            if (itemWorld.item.itemName == Item.ItemName.Cherry)
            {
                cherriesCount= itemWorld.amount;
            }
        }
        cherriesCounterText.text = cherriesCount.ToString();
    }
}
