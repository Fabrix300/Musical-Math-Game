using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float movementSpeed;
    public float distFromPlayer;

    private Rigidbody2D rb;
    private Animator anim;
    private int dirX;

    // Start is called before the first frame update
    void Start()
    {
        dirX = 0;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * movementSpeed * Time.deltaTime, rb.velocity.y);
    }

    public void UpdateAnimation()
    {
        int state;

        if (dirX > 0f)
        {
            //spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            state = 1;
        }
        else if (dirX < 0f)
        {
            //spriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            state = 1;
        }
        else
        {
            state = 0;
        }
        if (rb.velocity.y > .1f)
        {
            state = 2;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = 3;
        }
        anim.SetInteger("state", state);
    }
}
