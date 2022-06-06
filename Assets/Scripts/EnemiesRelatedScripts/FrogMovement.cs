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

    private float dirX = 0f;
    private int currentWaypointIndex = 0;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer enemySprite;
    private CircleCollider2D frogCollider;
    private float timeElapsed = 0f;
    private bool isJumping = false;
    private bool isFacingLeft = true;
    private bool isFacingRight = true;
    private bool isIdle = true;
    private bool isInAir = false;

    private enum FrogMovementState { idle, jumping, falling }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        frogCollider = GetComponent<CircleCollider2D>();
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
            if (isIdle)
            {
                dirX = 0f;
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= idleTime) 
                {
                    isJumping = true;
                    isIdle = false;
                    timeElapsed = 0f;
                }
            }
            else if (isJumping)
            {
                rb2d.AddForce(Vector2.up * jumpForce);
                if (waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
                {
                    dirX = 1f;
                    isFacingRight = true;
                    isFacingLeft = false;
                    enemySprite.flipX = true;
                }
                else
                {
                    dirX = -1f;
                    isFacingRight = false;
                    isFacingLeft = true;
                    enemySprite.flipX = false;
                }
                isJumping = false;
                isInAir = true;
            }
            else if (isInAir)
            {
                if (rb2d.velocity.y <= -.1f)
                {
                    if (IsGrounded())
                    {
                        dirX = 0f;
                        isInAir = false;
                        isIdle = true;
                        if (isFacingLeft)
                        {
                            if(waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
                            {
                                isFacingLeft = false;
                                isFacingRight = true;
                                enemySprite.flipX = true;
                                currentWaypointIndex++;
                                if (currentWaypointIndex >= waypoints.Length)
                                {
                                    currentWaypointIndex = 0;
                                }
                            }
                        }
                        else if (isFacingRight)
                        {
                            if (waypoints[currentWaypointIndex].transform.position.x < transform.position.x)
                            {
                                isFacingLeft = true;
                                isFacingRight = false;
                                enemySprite.flipX = false;
                                currentWaypointIndex++;
                                if (currentWaypointIndex >= waypoints.Length)
                                {
                                    currentWaypointIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        FrogMovementState animState = FrogMovementState.idle;

        if (dirX == 0f)
        {
            animState = FrogMovementState.idle;
        }
        else if (rb2d.velocity.y > .1f)
        {
            animState = FrogMovementState.jumping;
        }
        else if (rb2d.velocity.y < -.1f)
        {
            animState = FrogMovementState.falling;
        }
        anim.SetInteger("state", (int)animState);
    }

    public void FreezeEnemy()
    {
        anim.enabled = false;
        //Freeze Rotation in Z
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private bool IsGrounded()
    {
        float extraHeightExtent = .1f;
        RaycastHit2D hitGroundLeft = Physics2D.Raycast(frogCollider.bounds.center + (Vector3.left * frogCollider.bounds.extents.x), Vector2.down, frogCollider.bounds.extents.y + extraHeightExtent);
        RaycastHit2D hitGroundRight = Physics2D.Raycast(frogCollider.bounds.center + (Vector3.right * frogCollider.bounds.extents.x), Vector2.down, frogCollider.bounds.extents.y + extraHeightExtent);
        RaycastHit2D hitGroundCenter = Physics2D.Raycast(frogCollider.bounds.center, Vector2.down, frogCollider.bounds.extents.y + extraHeightExtent);
        if (hitGroundCenter || hitGroundLeft || hitGroundRight) return true;
        return false;
    }
}
