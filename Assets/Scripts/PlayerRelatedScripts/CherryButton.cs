using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryButton : MonoBehaviour
{
    public Text cherriesCounterText;

    private PlayerInventory playerInventory;
    private PlayerStats playerStats;

    private void Start()
    {
        playerInventory = PlayerInventory.instance;
        playerStats = PlayerStats.instance;
        playerInventory.OnCherryCollected += RefreshCherriesText;
        RefreshCherriesText();
    }

    public void RefreshCherriesText()
    {
        if (cherriesCounterText != null)
        {
            cherriesCounterText.text = GetCherryItemInInventory().amount.ToString();
        }
    }

    public void OnCherryButtonPressed()
    {
        ItemObject cherryItem = GetCherryItemInInventory();
        if(cherryItem is HealerObject)
        {
            if (cherryItem.amount >= 1 && playerStats.GetPlayerEnergyPoints() < playerStats.GetPlayerMaxEnergyPoints())
            {
                cherryItem.amount--;
                playerStats.HealPlayer((cherryItem as HealerObject).amountOfEnergyRestored);
            }
            RefreshCherriesText();
        }
    }

    public ItemObject GetCherryItemInInventory()
    {
        ItemObject cherryItem = null;
        List<ItemObject> itemList = playerInventory.GetItemList();
        foreach (ItemObject itemObjectInInventory in itemList)
        {
            if (itemObjectInInventory.itemName == ItemName.Cherry)
            {
                cherryItem = itemObjectInInventory;
            }
        }
        return cherryItem;
    }
}
