using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
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
        // CONSIDER USE LOADSCENEADDITIVE
        SceneManager.LoadSceneAsync("Combat" + combatData.getOriginScene());
    }

    private void StartFight(GameObject gO)
    {
        /*STOP PLAYER*/
        playerMovement.FreezePlayer();
        // SHOULD STOP ENEMY AS WELL
        Enemy enemy = gO.GetComponent<Enemy>();
        //combatManager.SetEnemyToCombatData(enemy.enemyType, enemy.enemyName, enemy.level, enemy.healthPoints,enemy.maxHealthPoints);
        combatData.SetEnemyToCombat(enemy);
        combatData.SetOriginScene(SceneManager.GetActiveScene().name);
        combatData.SetPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        Debug.Log("Starting fight with a " + enemy);
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        //StartCoroutine(PassToCombatScene());
        //SceneManager.LoadSceneAsync("Combat" + combatData.getOriginScene());
    }
}
