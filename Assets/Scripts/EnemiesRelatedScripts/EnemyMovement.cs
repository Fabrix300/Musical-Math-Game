using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool inCombat = false;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer enemySprite;
    private float dirX;
    [SerializeField] private float speedX;

    private enum EnemyMovementState { idle, running }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        if (waypoints[0].transform.position.x > transform.position.x)
        {
            dirX = 1f;
            enemySprite.flipX = true;
        }
        else
        {
            dirX = -1f;
            enemySprite.flipX = false;
        }
        anim.SetInteger("state", (int)EnemyMovementState.running);
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

    private void NonHostileMovement()
    {
        if (waypoints[0] != null && waypoints[1] != null)
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
            {
                currentWaypointIndex++;
                dirX = -dirX;
                enemySprite.flipX = !enemySprite.flipX;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            //transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speedX);
            //rb2d.velocity = new Vector2(dirX * speedX, 0f);
        }
    }

    public void FreezeEnemy()
    {
        anim.enabled = false;
        //Freeze Rotation in Z
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}