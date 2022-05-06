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
    }

    public void RefreshCherriesText()
    {
        cherriesCounterText.text = GetCherriesItemInInventory().amount.ToString();
    }

    public void OnCherryButtonPressed()
    {
        ItemWorld cherryItem = GetCherriesItemInInventory();
        if (cherryItem.amount >= 1 && playerStats.GetPlayerEnergyPoints() < playerStats.GetPlayerMaxEnergyPoints())
        {
            cherryItem.amount--;
            playerStats.HealPlayer(cherryItem.amountOfEnergyRestored);
        }
        RefreshCherriesText();
    }

    public ItemWorld GetCherriesItemInInventory()
    {
        ItemWorld cherryItem = null;
        List<ItemWorld> itemList = playerInventory.GetItemList();
        foreach (ItemWorld itemWorld in itemList)
        {
            if (itemWorld.item.itemName == Item.ItemName.Cherry)
            {
                cherryItem = itemWorld;
            }
        }
        return cherryItem;
    }
}
