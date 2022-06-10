using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooker : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer gOSpriteRenderer;

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        gOSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            gOSpriteRenderer.flipX = true;
        }
        else
        {
            gOSpriteRenderer.flipX = false;
        }
    }
}
