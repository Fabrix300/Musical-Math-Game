using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool inCombat = false;

    [SerializeField] private Transform player;
    [SerializeField] private float MinPositionX; // left border
    [SerializeField] private float MaxPositionX; //  right border
    [SerializeField] private float MinPositionY;
    [SerializeField] private float MaxPositionY;

    // Update is called once per frame
    void Update()
    {
        if (inCombat == false && player)
        {
            transform.position = new Vector3(
                Mathf.Clamp(player.position.x, MinPositionX, MaxPositionX),
                Mathf.Clamp(player.position.y, MinPositionY, MaxPositionY),
                transform.position.z
            );
        }
    }

    public void FindPlayer() {
        player = GameObject.Find("Player").transform;
    }
}
