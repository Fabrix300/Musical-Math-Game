using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerSpawnPoints
{
    public string levelName;
    public Vector3 leftSpawnPoint;
    public Vector3 rightSpawnPoint;

    public LevelPlayerSpawnPoints(string _levelName, Vector3 _leftSpawnPoint, Vector3 _rightSpawnPoint)
    {
        levelName = _levelName;
        leftSpawnPoint = _leftSpawnPoint;
        rightSpawnPoint = _rightSpawnPoint;
    }
}
