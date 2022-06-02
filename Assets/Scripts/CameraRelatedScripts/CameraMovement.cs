using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool inCombat = false;
    //public Vector3 offset;

    [Range(1, 10)]
    public float smoothFactor;

    [SerializeField] private Transform player;
    [SerializeField] private float MinPositionX; // left border
    [SerializeField] private float MaxPositionX; //  right border
    [SerializeField] private float MinPositionY;
    [SerializeField] private float MaxPositionY;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inCombat == false && player)
        {
            /*transform.position = new Vector3(
                Mathf.Clamp(player.position.x, MinPositionX, MaxPositionX),
                Mathf.Clamp(player.position.y, MinPositionY, MaxPositionY),
                transform.position.z
            );*/

            //Vector3 targetPosition = player.position + offset;
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothFactor * Time.fixedDeltaTime);
            //transform.position = smoothedPosition;
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, MinPositionX, MaxPositionX),
                Mathf.Clamp(smoothedPosition.y, MinPositionY, MaxPositionY),
                transform.position.z
            );
        }
    }

    public void FindPlayer() {
        player = GameObject.Find("Player").transform;
    }
}
