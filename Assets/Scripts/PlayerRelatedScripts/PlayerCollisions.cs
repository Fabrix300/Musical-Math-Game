using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    public GameObject LevelHolder;
    public Camera LevelCamera;

    private PlayerInventory playerInventory;
    private CombatData combatData;
    private ItemWorld itemWorld;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.instance;
        combatData = CombatData.instance;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gO = collision.gameObject;
        // Checking if its an item
        if (gO.GetComponent<ItemWorld>())
        {
            itemWorld = collision.gameObject.GetComponent<ItemWorld>();
            if (playerInventory)
            {
                playerInventory.AddItem(itemWorld);
                itemWorld.DestroySelf();
                Debug.Log(itemWorld.item.itemName + " collected!" + "\n" + "Items in Inventory: " + playerInventory.CountItems());
            }
        }

        // Checking if its an enemy
        // I was using kinematic and isTrigger in enemy but i left it with dynamic body and isTrigger deactivated, works the same
        /*else if (gO.GetComponent<Enemy>())
        {
            StartFight(gO);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gO = collision.gameObject;
        // Checking if its an enemy
        if (gO.GetComponent<Enemy>())
        {
            StartFight(gO);
        }
    }

    IEnumerator PassToCombatScene()
    {
        yield return new WaitForSeconds(2);
        if (LevelHolder != null)
        {
            LevelHolder.SetActive(false);
            LevelCamera.GetComponent<CameraMovement>().inCombat = true;
            LevelCamera.transform.position = new Vector3(0f, 0f, -10);
        }
        SceneManager.LoadSceneAsync("Combat" + combatData.GetOriginScene(), LoadSceneMode.Additive);
    }

    private void StartFight(GameObject enemyGO)
    {
        /*STOP PLAYER*/ playerMovement.FreezePlayer();
        /*STOP ENEMY*/ enemyGO.GetComponent<EnemyMovement>().FreezeEnemy();
        Enemy enemyComponent = enemyGO.GetComponent<Enemy>();
        //combatManager.SetEnemyToCombatData(enemy.enemyType, enemy.enemyName, enemy.level, enemy.healthPoints,enemy.maxHealthPoints);
        combatData.SetEnemyToCombat(enemyComponent);
        combatData.SetOriginScene(SceneManager.GetActiveScene().name);
        combatData.SetPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        combatData.SetCameraPosition(LevelCamera.transform.position.x, LevelCamera.transform.position.y, LevelCamera.transform.position.z);
        Debug.Log("Starting fight with a " + enemyComponent);
        StartCoroutine(PassToCombatScene());
    }
}
