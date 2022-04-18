using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public int amount;
    public int amountOfEnergyRestored;

    public void DestroySelf ()
    {
        Destroy(gameObject);
    }
}
