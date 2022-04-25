using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatData : MonoBehaviour
{
    // SINGLETON
    public static CombatData instance;

    //ENEMY TO COMBAT DATA
    private Enemy enemyToCombat;
    /*public EnemyType enemyToCombatType;
    public string enemyName;
    public int enemyToCombatLevel;
    public float enemyToCombatHealthPoints;
    public float enemyToCombatMaxHealthPoints;*/

    //public GameObject player;
    private string originScene;
    private Vector3 previousCameraPosition = Vector3.zero;
    private Vector3 previousPlayerPosition = Vector3.zero;

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

    public void SetPreviousPlayerPosition(float x, float y, float z)
    {
        previousPlayerPosition[0] = x; previousPlayerPosition[1] = y; previousPlayerPosition[2] = z;
    }

    public Vector3 GetPreviousPlayerPosition() { return previousPlayerPosition; }

    public void SetPreviousCameraPosition(float x, float y, float z)
    {
        previousCameraPosition[0] = x; previousPlayerPosition[1] = y; previousPlayerPosition[2] = z;
    }

    public Vector3 GetPreviousCameraPosition() { return previousCameraPosition; }

    public void SetOriginScene(string sceneName) { originScene = sceneName; }

    public string GetOriginScene() { return originScene; }

    /*public void SetEnemyToCombatData(
        EnemyType enemyToCombat,
        string name,
        int level,
        float healthPoints,
        float maxHealthPoints
    )
    {
        this.enemyToCombatType = enemyToCombat;
        this.enemyName = name;
        this.enemyToCombatLevel = level;
        this.enemyToCombatHealthPoints = healthPoints;
        this.enemyToCombatMaxHealthPoints = maxHealthPoints;
    }*/

    public void SetEnemyToCombat(Enemy enemy) { enemyToCombat = enemy; }

    public Enemy GetEnemyToCombat() { return enemyToCombat; }

    /*public void SetPlayer(GameObject player) { this.player = player; }

    public GameObject GetPlayer() { return player; }
    */

}
