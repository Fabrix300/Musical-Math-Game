using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAssets : MonoBehaviour
{
    // SINGLETON
    public static CombatAssets instance;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject playerPreFab;
    public Sprite playerImage;
    public GameObject[] enemyPreFabs;
    public Sprite[] enemyImages;
    //turnIndicator
    public GameObject turnIndicatorPlayer;
    public GameObject turnIndicatorEnemy;
    //attack Effect
    public GameObject attackEffect;
    //AudioHelper
    public GameObject audioHelper;

    public GameObject GetEnemyPreFab(EnemyType typeEnemy)
    {
        return typeEnemy switch
        {
            EnemyType.eagle => enemyPreFabs[0],
            EnemyType.frog => enemyPreFabs[1],
            EnemyType.opossum => enemyPreFabs[2],
            _ => throw new System.NotImplementedException(),
        };
    }

    public Sprite GetEnemyImage(EnemyType typeEnemy)
    {
        return typeEnemy switch
        {
            EnemyType.eagle => enemyImages[0],
            EnemyType.frog => enemyImages[1],
            EnemyType.opossum => enemyImages[2],
            _ => throw new System.NotImplementedException(),
        };
    }
}
