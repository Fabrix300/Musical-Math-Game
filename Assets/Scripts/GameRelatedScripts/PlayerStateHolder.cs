using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHolder
{
    public int playerLevel;
    public int playerExpPoints;
    public int playerMaxExpPoints;
    public float playerEnergyPoints;
    public float playerMaxEnergyPoints;
    public int amountOfCherries;
    public List<string> levelkeys;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    public PlayerStateHolder
    (
        int _playerLevel,
        int _playerExpPoints,
        int _playerMaxExpPoints,
        float _playerEnergyPoints,
        float _playerMaxEnergyPoints,
        int _amountOfCherries,
        List<string> _levelkeys,
        Vector3 _spawnPosition,
        Quaternion _spawnRotation
    )
    {
        playerLevel = _playerLevel;
        playerExpPoints = _playerExpPoints;
        playerMaxExpPoints = _playerMaxExpPoints;
        playerEnergyPoints = _playerEnergyPoints;
        playerMaxEnergyPoints = _playerMaxEnergyPoints;
        amountOfCherries = _amountOfCherries;
        levelkeys = _levelkeys;
        spawnPosition = _spawnPosition;
        spawnRotation = _spawnRotation;
    }
}
