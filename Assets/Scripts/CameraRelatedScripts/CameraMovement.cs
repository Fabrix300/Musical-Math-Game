using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float MinPositionX; // left border
    [SerializeField] private float MaxPositionX; //  right border
    [SerializeField] private float MinPositionY;
    [SerializeField] private float MaxPositionY;

    // Update is called once per frame
    void Update()
    {
        /*if (player.position.x < 0f && player.position.y < 0f)
        {
            transform.position = new Vector3(0f, 0f, transform.position.z);
        }
        else if (player.position.y < 0f)
        {
            transform.position = new Vector3(player.position.x, 0f, transform.position.z);
        }
        else if (player.position.x < 0f)
        {
            transform.position = new Vector3(0f, player.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }*/
        transform.position = new Vector3(
            Mathf.Clamp(player.position.x, MinPositionX, MaxPositionX),
            Mathf.Clamp(player.position.y, MinPositionY, MaxPositionY),
            transform.position.z
        );
    }
}
