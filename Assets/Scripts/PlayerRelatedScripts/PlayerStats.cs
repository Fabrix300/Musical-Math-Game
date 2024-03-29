using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    // SINGLETON
    public static PlayerStats instance;

    // Life or mana idk
    private bool isPlayerAlive = true;
    private readonly float basePlayerMaxEnergyPoints = 18;
    private float playerEnergyPoints;
    private float playerMaxEnergyPoints;

    // OTHER STATS
    public int level;
    private readonly int basePlayerMaxExpPoints = 18;
    private int playerExpPoints;
    private int playerMaxExpPoints;
    //private readonly float baseDamage = 3;
    private readonly float baseDamage = 3;
    [HideInInspector] public float damage;
    //public Stat armor;

    // INFO
    public string playerName;

    // CALLBACK para cuando el jugador recibe da�o
    public event Action OnPlayerHealthPointsChange;
    public event Action OnCherryItemUsed;
    public event Action OnPlayerExpPointsChange;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SetStatsBasedOnLevel(0);
    }

    public bool NumbPlayer(float damage)
    {
        if (isPlayerAlive)
        {
            //damage -= (damage * (0.01f * armor.GetValue()));
            playerEnergyPoints -= damage;
            playerEnergyPoints = Mathf.Clamp(playerEnergyPoints, 0, playerMaxEnergyPoints);
            OnPlayerHealthPointsChange?.Invoke();
            if (playerEnergyPoints <= 0f)
            {
                //Die();
                return true;
            }
            return false;
        }
        return true;
    }

    public bool HealPlayer(float pointsHealed, ItemName itemNameUsed = ItemName.None)
    {
        if (isPlayerAlive && playerEnergyPoints < playerMaxEnergyPoints)
        {
            playerEnergyPoints += pointsHealed;
            playerEnergyPoints = Mathf.Clamp(playerEnergyPoints, 0, playerMaxEnergyPoints);
            OnPlayerHealthPointsChange?.Invoke();
            if (itemNameUsed == ItemName.Cherry)
            {
                OnCherryItemUsed?.Invoke();
            }
            return true;
        }
        return false;
    }

    /*public virtual void Die()
    {
        isPlayerAlive = false;
        //DIE IN SOME WAY
        Debug.Log("player died.");
    }*/

    public bool AddExpPointsAndCheck(int points)
    {
        playerExpPoints += points;
        if (playerExpPoints >= playerMaxExpPoints)
        {
            int reminder = playerExpPoints - playerMaxExpPoints;
            AdvanceOneLevel(reminder);
            return true;
        }
        OnPlayerExpPointsChange?.Invoke();
        return false;
    }

    public void AdvanceOneLevel(int reminder)
    {
        level += 1;
        SetStatsBasedOnLevel(reminder);
    }

    public void SetStatsBasedOnLevel(int reminder) 
    {
        //damage = baseDamage + (2 * level);
        damage = baseDamage + level;
        playerMaxEnergyPoints = basePlayerMaxEnergyPoints + (2 * level);
        playerEnergyPoints = playerMaxEnergyPoints;
        playerMaxExpPoints = basePlayerMaxExpPoints + (2 * level);
        playerExpPoints = reminder;
        OnPlayerExpPointsChange?.Invoke();
        OnPlayerHealthPointsChange?.Invoke();
    }

    public bool GetIsPlayerAlive()
    {
        return isPlayerAlive;
    }

    public float GetPlayerEnergyPoints()
    {
        return playerEnergyPoints;
    }

    public void SetPlayerEnergyPoints(float energyPoints)
    {
        playerEnergyPoints = energyPoints;
    }

    public float GetPlayerMaxEnergyPoints()
    {
        return playerMaxEnergyPoints;
    }

    public void SetPlayerMaxEnergyPoints(float maxEnergyPoints)
    {
        playerMaxEnergyPoints = maxEnergyPoints;
    }

    public int GetPlayerExpPoints()
    {
        return playerExpPoints;
    }

    public void SetPlayerExpPoints(int expPoints)
    {
        playerExpPoints = expPoints;
    }

    public int GetPlayerMaxExpPoints()
    {
        return playerMaxExpPoints;
    }

    public void SetPlayerMaxExpPoints(int maxExpPoints)
    {
        playerMaxExpPoints = maxExpPoints;
    }
}
