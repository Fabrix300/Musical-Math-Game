using System.Collections;
using System.Collections.Generic;
using System;
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
        /*TEST*/ //playerInventory.OnCherryUsed += RefreshCherriesText;
        RefreshCherriesText();
    }

    public void RefreshCherriesText()
    {
        //traer item y chequear su amount
    }

    public void OnCherryButtonPressed()
    {
        //reducir cantidad     
        //curar jugador
    }
}
