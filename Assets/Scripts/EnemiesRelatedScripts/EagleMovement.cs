using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    public float speed;
    public GameObject[] waypoints;
    public bool inCombat = false;

    private int currentWaypointIndex = 0;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat) NonHostileMovement();
    }

    public void NonHostileMovement()
    {
        if (CheckWayPoints())
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, step);
            if (transform.position == waypoints[currentWaypointIndex].transform.position)
            {
                // thinking about stop 4 a while
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            UpdateAnimation();
        }
    }

    public void UpdateAnimation()
    {
        if (waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
        {
            enemySprite.flipX = true;
        }
        else
        {
            enemySprite.flipX = false;
        }
    }

    public bool CheckWayPoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (!waypoints[i])
            {
                return false;
            }
        }
        return true;
    }

    public void FreezeEnemy()
    {
        anim.enabled = false;
        inCombat = true;
        //Freeze Rotation in Z
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
