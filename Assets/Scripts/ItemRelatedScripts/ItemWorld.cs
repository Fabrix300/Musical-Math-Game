using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public ItemObject item;

    public void DestroySelf ()
    {
        Destroy(gameObject);
    }
}
