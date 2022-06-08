using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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
    public bool hasKey;

    private CombatAssets combatAssets;
    private GameManager gameManager;

    private readonly float baseDamage = 1;
    private readonly float baseMaxHealthPoints = 18;
    [HideInInspector] public float damage;
    [HideInInspector] public float healthPoints;
    [HideInInspector] public float maxHealthPoints;

    public Text levelText;

    public event Action OnEnemyHealthPointsChange;

    private void Start()
    {
        combatAssets = CombatAssets.instance;
        gameManager = GameManager.instance;

        levelText.text = level.ToString();
        //damage = baseDamage + (2 * level);
        damage = baseDamage + level;
        maxHealthPoints = baseMaxHealthPoints + (2 * level);
        healthPoints = maxHealthPoints;
    }

    public void DestroySelf()
    {
        if (hasKey)
        {
            GameObject keyGO = Instantiate(combatAssets.keyPreFab, transform.position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(keyGO, SceneManager.GetSceneByName(gameManager.savedSceneName));
            keyGO.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f * 100, 1f * 100));
        }
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
        return damage;
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

    public void MakeColliderTrigger()
    {
        switch (enemyType)
        {
            case EnemyType.opossum:
                {
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    break;
                }
            case EnemyType.frog:
                {
                    gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                    break;
                }
            case EnemyType.eagle:
                {
                    gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
                    break;
                }
        }
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
