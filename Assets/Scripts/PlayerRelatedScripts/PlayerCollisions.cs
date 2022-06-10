using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    private GameObject levelHolder;
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
        levelHolder = GameObject.Find(gameManager.savedSceneName+"Holder");
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
                /*playerInventory.AddItem(itemWorld.healerObject); itemWorld.DestroySelf();*/
                playerInventory.AddItem(gO.GetComponent<CherryItemWorld>().healerObject);
                gO.GetComponent<Animator>().SetInteger("state", 1);
            }
        }
        else if (gO.CompareTag("NextLevelPortal"))
        {
            if (playerInventory.HasPlayerSpecificKey(gameManager.savedSceneName))
            {
                if (gO.transform.Find("DoorBlock").gameObject.activeInHierarchy)
                {
                    audioManager.Play("DoorOpen");
                }
                StartCoroutine(gameManager.AdvanceToNextLevel());
            }
        }
        /*else if (gO.CompareTag("PreviousLevelPortal"))
        {
            if (playerInventory.HasPlayerSpecificKey(gameManager.savedSceneName))
            {
                StartCoroutine(gameManager.GoBackToPreviousLevel());
            }
        }*/
        else if (gO.CompareTag("FinishGameTrigger"))
        {
            gameManager.FinishGame();
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
            Debug.Log("starting fight");
            gameManager.StartAFight(gO, levelHolder);
        }
        else if (gO.CompareTag("Key"))
        {
            playerInventory.AddKey(gameManager.savedSceneName);
            audioManager.Play("ItemCollected");
            Destroy(gO);
        }
    }
}
