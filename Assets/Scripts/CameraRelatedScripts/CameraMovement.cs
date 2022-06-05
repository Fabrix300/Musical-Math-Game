using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool inCombat = false;

    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 offset;

    [SerializeField] private Transform player;
    public float minPositionX; // left border
    public float maxPositionX; //  right border
    public float minPositionY;
    public float maxPositionY;

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

            Vector3 targetPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothFactor * Time.fixedDeltaTime);
            //transform.position = smoothedPosition;
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, minPositionX, maxPositionX),
                Mathf.Clamp(smoothedPosition.y, minPositionY, maxPositionY),
                transform.position.z
            );
        }
    }

    public void FindPlayer() {
        player = GameObject.Find("Player").transform;
        if (player)
        {
            Debug.Log("Camara encontro al jugador");
        }
    }

    public void SetCameraLimits(LevelCameraLimits lvlCamLimits)
    {
        minPositionX = lvlCamLimits.minPositionX;
        maxPositionX = lvlCamLimits.maxPositionX;
        minPositionY = lvlCamLimits.minPositionY;
        maxPositionY = lvlCamLimits.maxPositionY;
        offset = lvlCamLimits.offset;
    }
}
