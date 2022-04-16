using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        //-0.01100001f
        if (player.position.x < 0f && player.position.y < 0f)
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
        }
    }
}
