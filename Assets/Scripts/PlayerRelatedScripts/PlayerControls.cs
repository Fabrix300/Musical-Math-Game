using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public PlayerMovement player;

    public EventTrigger[] buttonsTriggers;

    public void FindPlayer()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (player)
        {
            Debug.Log("PlayerControls encontro al jugador");
            AssignPointerEvents();
        }
    }

    public void AssignPointerEvents()
    {
        //EventTrigger.Entry entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerDown;
        //leftButton.triggers.Add(entry);
        foreach (EventTrigger eTB in buttonsTriggers)
        {
            Debug.Log(eTB.gameObject.name);
        }
    }

    /*private void OnEnable()
    {
        Debug.Log("a");
        if (!player)
        {
            Debug.Log("no hay player");
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
            if (player)
            {
                Debug.Log("PlayerControls encontro al jugador");
            }
        }
    }*/
}
