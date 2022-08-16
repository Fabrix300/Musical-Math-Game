using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    private PlayerMovement player;

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

    public void SetPlayer(PlayerMovement _player)
    {
        player = _player;
        AssignPointerEvents();
    }

    public void AssignPointerEvents()
    {
        foreach (EventTrigger eTB in buttonsTriggers)
        {
            switch (eTB.gameObject.name)
            {
                case "LeftArrow":
                    {
                        // First Event
                        EventTrigger.Entry entry1 = new EventTrigger.Entry();
                        entry1.eventID = EventTriggerType.PointerDown;
                        entry1.callback.AddListener((_) => { player.MoveLeft(); });
                        buttonsTriggers[0].triggers.Add(entry1);
                        // Second Event
                        EventTrigger.Entry entry2 = new EventTrigger.Entry();
                        entry2.eventID = EventTriggerType.PointerUp;
                        entry2.callback.AddListener((_) => { player.StopMoving(); });
                        buttonsTriggers[0].triggers.Add(entry2);
                        // Third Event
                        EventTrigger.Entry entry3 = new EventTrigger.Entry();
                        entry3.eventID = EventTriggerType.PointerExit;
                        entry3.callback.AddListener((_) => { player.StopMoving(); });
                        buttonsTriggers[0].triggers.Add(entry3);
                        Debug.Log("move left added to left arrow");
                        break;
                    }
                case "RightArrow":
                    {
                        // First Event
                        EventTrigger.Entry entry1 = new EventTrigger.Entry();
                        entry1.eventID = EventTriggerType.PointerDown;
                        entry1.callback.AddListener((_) => { player.MoveRight(); });
                        buttonsTriggers[1].triggers.Add(entry1);
                        // Second Event
                        EventTrigger.Entry entry2 = new EventTrigger.Entry();
                        entry2.eventID = EventTriggerType.PointerUp;
                        entry2.callback.AddListener((_) => { player.StopMoving(); });
                        buttonsTriggers[1].triggers.Add(entry2);
                        // Third Event
                        EventTrigger.Entry entry3 = new EventTrigger.Entry();
                        entry3.eventID = EventTriggerType.PointerExit;
                        entry3.callback.AddListener((_) => { player.StopMoving(); });
                        buttonsTriggers[1].triggers.Add(entry3);
                        Debug.Log("move right added to right arrow");
                        break;
                    }
                case "JumpButton":
                    {
                        // First Event
                        EventTrigger.Entry entry1 = new EventTrigger.Entry();
                        entry1.eventID = EventTriggerType.PointerDown;
                        entry1.callback.AddListener((_) => { player.Jump(); });
                        buttonsTriggers[2].triggers.Add(entry1);
                        Debug.Log("jump added to jump button");
                        break;
                    }
            }
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
