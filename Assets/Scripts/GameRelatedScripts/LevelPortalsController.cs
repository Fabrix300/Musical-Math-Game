using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortalsController : MonoBehaviour
{
    /* Previous Level Portal */
    public GameObject previousLevelPortalDoorSprite;

    /* Previous Level Portal */
    public GameObject nextLevelPortalDoorSprite;

    private PlayerInventory playerInventory;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.instance;
        gameManager = GameManager.instance;
        //HideDoorIfPlayerHasPreviousLevelKey();
        //HideDoorIfPlayerHasNextLevelKey();
    }

    public void HideDoorIfPlayerHasPreviousLevelKey()
    {
        int actualLevelNumber = int.Parse(gameManager.savedSceneName[5..]);
        bool hasKey = playerInventory.HasPlayerSpecificKey("Level"+ (actualLevelNumber-1));
        if (previousLevelPortalDoorSprite)
        {
            if (hasKey) previousLevelPortalDoorSprite.SetActive(false);
            else previousLevelPortalDoorSprite.SetActive(true);
        }
    }

    public void HideDoorIfPlayerHasNextLevelKey()
    {
        int actualLevelNumber = int.Parse(gameManager.savedSceneName[5..]);
        bool hasKey = playerInventory.HasPlayerSpecificKey("Level" + (actualLevelNumber));
        if (nextLevelPortalDoorSprite)
        {
            if (hasKey) nextLevelPortalDoorSprite.SetActive(false);
            else nextLevelPortalDoorSprite.SetActive(true);
        }
    }
}
