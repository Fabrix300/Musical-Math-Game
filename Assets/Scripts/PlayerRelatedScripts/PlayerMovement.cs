using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce;
    public float movementSpeed;
    public bool allowMovement = true;

    private float dirX;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private enum AnimationState { Idle, Running, Jumping, Falling };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(allowMovement == true)
        {
            dirX = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        UpdateAnimation();
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
        //usé esto originalmente pero obliga a poner los pisos saltables en una layer especifica:return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
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

    public void SetAllowMovement(bool value)
    {
        allowMovement = value;
    }

    public void FreezePlayer()
    {
        anim.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        //rb.velocity = Vector2.zero;
        SetAllowMovement(false);
    }
}
