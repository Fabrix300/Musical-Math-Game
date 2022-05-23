using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum EnemyType
{
    eagle,
    frog,
    opossum
}

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public string enemyName;
    public int level;
    public float damage;
    public float healthPoints;
    public float maxHealthPoints;
    public Text levelText;

    public event Action OnEnemyHealthPointsChange;

    private void Start()
    {
        levelText.text = level.ToString();
    }

    public void DestroySelf()
    {
        Debug.Log("Destroying enemy");
        Destroy(gameObject);
    }

    public bool Numb(float damage)
    {
        healthPoints -= damage;
        OnEnemyHealthPointsChange?.Invoke();
        if (healthPoints <= 0f) { healthPoints = 0f; return true; }
        else { return false; }
    }

    public float GetTotalDamage()
    {
        return damage * level;
    }

    public void SetAll(
        EnemyType enemyType,
        string name,
        int level,
        float healthPoints,
        float maxHealthPoints
    )
    {
        this.enemyType = enemyType;
        enemyName = name;
        this.level = level;
        this.healthPoints = healthPoints;
        this.maxHealthPoints = maxHealthPoints;
    }

    /*public GameObject GetPreFab(EnemyType typeEnemy)
    {
        return typeEnemy switch
        {
            EnemyType.little => EnemyAssets.Instance.enemyLittle,
            EnemyType.big => EnemyAssets.Instance.enemyBig,
            _ => throw new System.NotImplementedException(),
        };
    }*/

}
