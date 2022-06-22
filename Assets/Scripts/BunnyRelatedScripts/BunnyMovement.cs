using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float movementSpeed;
    public float distFromPlayer;

    private Rigidbody2D rb;
    private int dirX;

    // Start is called before the first frame update
    void Start()
    {
        dirX = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.x > transform.position.x && Mathf.Abs(transform.position.x - playerTransform.position.x) > distFromPlayer)
        {
            dirX = 1;
        }
        else if (playerTransform.position.x < transform.position.x && Mathf.Abs(transform.position.x - playerTransform.position.x) > distFromPlayer)
        {
            dirX = -1;
        }
        if (Mathf.Abs(transform.position.x - playerTransform.position.x) < distFromPlayer)
        {
            dirX = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * movementSpeed * Time.deltaTime, rb.velocity.y);
    }
}
