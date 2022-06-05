using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    public float speedX;
    public float jumpForce;
    public GameObject[] waypoints;
    public bool inCombat = false;
    public float idleTime;

    private float startDirX;
    private float dirX = 0f;
    private int currentWaypointIndex = 0;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer enemySprite;
    private float timeElapsed = 0f;
    private bool isJumping = false;
    private bool isIdle = true;
    private bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        /*if (waypoints[0] && waypoints[1])
        {
            if (waypoints[0].transform.position.x > transform.position.x)
            {
                startDirX = 1f;
                enemySprite.flipX = true;
            }
            else
            {
                startDirX = -1f;
                enemySprite.flipX = false;
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat) NonHostileMovement();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(dirX * speedX * Time.deltaTime, rb2d.velocity.y);
    }

    public void NonHostileMovement()
    {
        if (waypoints[0] != null && waypoints[1] != null)
        {
            if (timeElapsed >= idleTime)
            {
                Debug.Log("startJump");
                isJumping = true;
                isIdle = false;
                if (waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
                {
                    dirX = 1f;
                    enemySprite.flipX = true;
                }
                else
                {
                    dirX = -1f;
                    enemySprite.flipX = false;
                }
                rb2d.AddForce(Vector2.up * jumpForce);
                timeElapsed = 0f;
            }
            else
            {
                timeElapsed += Time.deltaTime;
            }
        }
    }
}
