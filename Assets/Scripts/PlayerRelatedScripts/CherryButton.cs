using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CherryButton : MonoBehaviour
{
    public Text cherriesCounterText;
    public Button cherryButton;
    public Animator CherryImageAnim;

    private PlayerInventory playerInventory;
    private PlayerStats playerStats;

    private void Start()
    {
        playerInventory = PlayerInventory.instance;
        playerStats = PlayerStats.instance;
        playerInventory.OnCherryCollected += RefreshCherriesText;
        /*TEST*/ //playerInventory.OnCherryUsed += RefreshCherriesText;
        RefreshCherriesText();
    }

    public void RefreshCherriesText()
    {
        HealerObject cherryObject = (HealerObject) playerInventory.GetItem(ItemName.Cherry);
        if (cherryObject != null)
        {
            cherriesCounterText.text = cherryObject.amount.ToString();
        }
    }

    public void OnCherryButtonPressed()
    {
        StartCoroutine(HealPlayerOnButtonPressed());
    }

    private IEnumerator HealPlayerOnButtonPressed()
    {
        cherryButton.interactable = false;
        if (playerStats.GetIsPlayerAlive() &&
            playerStats.GetPlayerEnergyPoints() < playerStats.GetPlayerMaxEnergyPoints()
            )
        {
            int cherriesUsed = playerInventory.ReduceAmountOfItem(ItemName.Cherry, 1);
            if (cherriesUsed > 0) 
            {
                int playerLevel = playerStats.level;
                HealerObject cherryObject = (HealerObject) playerInventory.GetItem(ItemName.Cherry);
                if (playerStats.HealPlayer(
                    cherryObject.amountOfEnergyRestored * cherriesUsed + playerLevel, ItemName.Cherry
                ))
                {
                    //CherryImageAnim.SetTrigger("CherryUsed");
                    yield return new WaitForSeconds(1f);
                    cherryButton.interactable = true;
                    RefreshCherriesText();
                }
                else
                {
                    _ = playerInventory.AddAmountOfItem(ItemName.Cherry, 1);
                }
            }
        }
        cherryButton.interactable = true;
    }
}
