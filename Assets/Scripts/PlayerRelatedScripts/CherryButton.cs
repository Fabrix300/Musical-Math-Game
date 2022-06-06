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

    private void OnDestroy()
    {
        playerInventory.OnCherryCollected -= RefreshCherriesText;
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
        Debug.Log("click");
        if (playerStats.GetIsPlayerAlive() &&
            playerStats.GetPlayerEnergyPoints() < playerStats.GetPlayerMaxEnergyPoints()
            )
        {
            Debug.Log("Player is alive and doesnt have max health");
            int cherriesUsed = playerInventory.ReduceAmountOfItem(ItemName.Cherry, 1);
            if (cherriesUsed > 0) 
            {
                Debug.Log("cherry used");
                int playerLevel = playerStats.level;
                HealerObject cherryObject = (HealerObject) playerInventory.GetItem(ItemName.Cherry);
                if (playerStats.HealPlayer(
                    cherryObject.amountOfEnergyRestored * cherriesUsed + (2 * playerLevel), ItemName.Cherry
                ))
                {
                    Debug.Log("player healed");
                    //CherryImageAnim.SetTrigger("CherryUsed");
                    yield return new WaitForSeconds(1f);
                    cherryButton.interactable = true;
                    RefreshCherriesText();
                }
                else
                {
                    Debug.Log("player not healed, returning cherry");
                    _ = playerInventory.AddAmountOfItem(ItemName.Cherry, 1);
                }
            }
        }
        cherryButton.interactable = true;
    }
}
