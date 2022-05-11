using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryItemWorld : MonoBehaviour
{
    public HealerObject healerObject = null;

    private PlayerInventory playerInventory = null;

    private void Start()
    {
        playerInventory = PlayerInventory.instance;
        if (playerInventory)
        {
            healerObject = playerInventory.cherryObject;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
