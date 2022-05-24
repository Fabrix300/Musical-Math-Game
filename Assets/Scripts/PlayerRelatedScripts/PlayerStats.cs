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
    private readonly float baseDamage = 3;
    [HideInInspector] public float damage;
    //public Stat armor;

    // INFO
    public string playerName;

    // CALLBACK para cuando el jugador recibe daño
    public event Action OnPlayerHealthPointsChange;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        damage = baseDamage + (2 * level);
        playerMaxEnergyPoints = basePlayerMaxEnergyPoints + (2 * level);
        playerEnergyPoints = playerMaxEnergyPoints;
        playerMaxExpPoints = basePlayerMaxExpPoints + (2 * level);
        playerExpPoints = 0;
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
                Die();
                return true;
            }
            return false;
        }
        return true;
    }

    public bool HealPlayer(float pointsHealed)
    {
        if (isPlayerAlive && playerEnergyPoints < playerMaxEnergyPoints)
        {
            playerEnergyPoints += pointsHealed;
            playerEnergyPoints = Mathf.Clamp(playerEnergyPoints, 0, playerMaxEnergyPoints);
            OnPlayerHealthPointsChange?.Invoke();
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        isPlayerAlive = false;
        //DIE IN SOME WAY
        Debug.Log("player died.");
    }

    public void AddExpPointsAndCheck(int points)
    {
        playerExpPoints += points;
        if (playerExpPoints >= playerMaxExpPoints)
        {
            int reminder = playerExpPoints - playerMaxExpPoints;
            //AdvanceOneLevel(reminder);
        }
    }

    public bool GetIsPlayerAlive()
    {
        return isPlayerAlive;
    }

    public float GetPlayerEnergyPoints()
    {
        return playerEnergyPoints;
    }

    public float GetPlayerMaxEnergyPoints()
    {
        return playerMaxEnergyPoints;
    }

    public int GetPlayerExpPoints()
    {
        return playerExpPoints;
    }

    public int GetPlayerMaxExpPoints()
    {
        return playerMaxExpPoints;
    }
}
