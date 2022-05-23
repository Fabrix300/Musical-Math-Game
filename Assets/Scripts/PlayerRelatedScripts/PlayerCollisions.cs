using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    public GameObject levelHolder;
    public Camera mainCamera;

    private PlayerInventory playerInventory;
    private GameManager gameManager;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.instance;
        gameManager = GameManager.instance; audioManager = AudioManager.instance;
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gO = collision.gameObject;
        // Checking if its an item
        if (gO.GetComponent<CherryItemWorld>())
        {
            //CherryItemWorld itemWorld = gO.GetComponent<CherryItemWorld>();
            if (playerInventory)
            {
                audioManager.Play("ItemCollected");
                /*playerInventory.AddItem(itemWorld.healerObject);
                itemWorld.DestroySelf();*/
                playerInventory.AddItem(gO.GetComponent<CherryItemWorld>().healerObject);
                gO.GetComponent<Animator>().SetInteger("state", 1);
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
            //StartFight(gO);
            gameManager.StartAFight(gO, levelHolder);
        }
    }

    /*IEnumerator PassToCombatScene()
    {
        yield return new WaitForSeconds(2);
        if (levelHolder != null)
        {
            levelHolder.SetActive(false);
            mainCamera.GetComponent<CameraMovement>().inCombat = true;
            mainCamera.transform.position = new Vector3(0f, 0f, -10);
        }
        SceneManager.LoadSceneAsync("Combat" + combatData.GetOriginScene(), LoadSceneMode.Additive);
    }*/

    /*private void StartFight(GameObject enemyGO)
    {
        Enemy enemyComponent = enemyGO.GetComponent<Enemy>();
        Debug.Log("Starting fight with a " + enemyComponent);
        playerMovement.FreezePlayer();
        enemyGO.GetComponent<EnemyMovement>().FreezeEnemy();
        //combatManager.SetEnemyToCombatData(enemy.enemyType, enemy.enemyName, enemy.level, enemy.healthPoints,enemy.maxHealthPoints);
        combatData.SetEnemyToCombat(enemyComponent);
        combatData.SetOriginScene(gameManager.savedSceneName);
        combatData.SetPreviousPlayerPosition(transform.position.x, transform.position.y, transform.position.z);
        combatData.SetPreviousCameraPosition(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        StartCoroutine(PassToCombatScene());
    }*/
}
