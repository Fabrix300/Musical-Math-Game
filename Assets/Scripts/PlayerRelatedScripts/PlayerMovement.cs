using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce;
    public float movementSpeed;
    public bool inCombat = false;

    /*HealEffect*/
    public Animator healEffectAnimator;

    /*Audio*/
    public AudioSource jumpAS;
    public AudioSource cherryEatAS;

    private float dirX;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private PlayerStats playerStats;

    private enum AnimationState { Idle, Running, Jumping, Falling };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        playerStats = PlayerStats.instance;
        playerStats.OnCherryItemUsed += TriggerHealEffectAnimation;

        /*MOBILE*/
        dirX = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(inCombat == false)
        {
            dirX = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }*/

        if(inCombat == false) UpdateAnimation();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStats.NumbPlayer(2f);
        }
        //UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * movementSpeed * Time.deltaTime, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        /*
         * Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + extraHeightExtent), Color.green);
        Debug.DrawRay(playerCollider.bounds.center + (Vector3.left * playerCollider.bounds.extents.x), Vector2.down * (playerCollider.bounds.extents.y + extraHeightExtent), Color.green);
        Debug.DrawRay(playerCollider.bounds.center + (Vector3.right * playerCollider.bounds.extents.x), Vector2.down * (playerCollider.bounds.extents.y + extraHeightExtent), Color.green);
        */
        //us? esto originalmente pero obliga a poner los pisos saltables en una layer especifica:return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        float extraHeightExtent = .1f;
        /*RaycastHit2D hitGround = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightExtent);
        Debug.Log(hitGround.collider);
        return hitGround.collider != null;*/
        //return Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightExtent);
        RaycastHit2D hitGroundLeft = Physics2D.Raycast(playerCollider.bounds.center + (Vector3.left * playerCollider.bounds.extents.x), Vector2.down, playerCollider.bounds.extents.y + extraHeightExtent);
        RaycastHit2D hitGroundRight = Physics2D.Raycast(playerCollider.bounds.center + (Vector3.right * playerCollider.bounds.extents.x), Vector2.down, playerCollider.bounds.extents.y + extraHeightExtent);
        RaycastHit2D hitGroundCenter = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightExtent);
        if (hitGroundCenter || hitGroundLeft || hitGroundRight) return true;
        return false;
    }

    private void UpdateAnimation()
    {
        AnimationState state;

        if (dirX > 0f)
        {
            spriteRenderer.flipX = false;
            state = AnimationState.Running;
        }
        else if (dirX < 0f)
        {
            spriteRenderer.flipX = true;
            state = AnimationState.Running;
        }
        else
        {
            state = AnimationState.Idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = AnimationState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = AnimationState.Falling;
        }
        anim.SetInteger("state", (int)state);
    }

    public void SetInCombat(bool value)
    {
        inCombat = value;
    }

    public void FreezePlayer()
    {
        anim.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.velocity = Vector2.zero;
        SetInCombat(true);
    }

    public void UnfreezePlayer()
    {
        anim.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        SetInCombat(false);
        rb.AddForce(Vector2.up * 10f);
    }

    public void Jump()
    {
        if (inCombat == false && IsGrounded())
        {
            //jumpSoundEffect.Play();
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeedY);
            jumpAS.Play();
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    public void StopMoving()
    {
        dirX = 0f;
    }


    public void MoveLeft()
    {
        if (inCombat == false)
        {
            dirX = -1f;
        }
    }
    public void MoveRight()
    {
        if (inCombat == false)
        {
            dirX = 1f;
        }
    }

    public void TriggerHealEffectAnimation()
    {
        if (healEffectAnimator && cherryEatAS)
        {
            healEffectAnimator.SetTrigger("Start");
            cherryEatAS.Play();
        }
    }

}
