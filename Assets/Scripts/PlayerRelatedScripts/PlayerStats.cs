using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    // SINGLETON
    public static PlayerStats instance;

    // Life or mana idk
    private float playerEnergyPoints = 20;
    private float playerMaxEnergyPoints = 20;
    private bool playerDead = false;

    // STATS
    public int level;
    public int exp;
    public Stat damage;
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
    }

    public void NumbPlayer(float damage)
    {
        if (playerDead == false)
        {
            //damage -= (damage * (0.01f * armor.GetValue()));
            playerEnergyPoints -= damage;
            playerEnergyPoints = Mathf.Clamp(playerEnergyPoints, 0, playerMaxEnergyPoints);
            OnPlayerHealthPointsChange?.Invoke();
            if (playerEnergyPoints <= 0f)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        playerDead = true;
        //DIE IN SOME WAY
        Debug.Log("player died.");
    }

    public float GetPlayerEnergyPoints()
    {
        return playerEnergyPoints;
    }

    public float GetPlayerMaxEnergyPoints()
    {
        return playerMaxEnergyPoints;
    }
}
