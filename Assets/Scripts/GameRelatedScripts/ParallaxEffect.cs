using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform gameCamera;
    public float relativeMove;
    public bool lockY;

    void Start()
    {
        gameCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(gameCamera.position.x * relativeMove, transform.position.y);
        }
        else 
        {
            transform.position = new Vector2(gameCamera.position.x * relativeMove, gameCamera.position.y * relativeMove);
        }
    }
}
