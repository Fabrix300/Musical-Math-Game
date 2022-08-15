using UnityEngine;

public class BunnyMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    public float distFromPlayer;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform playerTransform;
    private int dirX;
    private bool isJumping;
    private bool isInAir;
    private bool playerJumped;
    private Vector3 playerJumpPoint;

    // Start is called before the first frame update
    void Start()
    {
        dirX = 0;
        playerJumped = false;
        playerTransform = GameObject.Find("Player").transform;
        //playerTransform.gameObject.GetComponent<PlayerMovement>().OnPlayerJump += ChangePlayerJumped;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerJumped)
        {
            if (playerTransform.position.x > transform.position.x && Mathf.Abs(transform.position.x - playerTransform.position.x) > distFromPlayer)
            {
                dirX = 1;
            }
            else if (playerTransform.position.x < transform.position.x && Mathf.Abs(transform.position.x - playerTransform.position.x) > distFromPlayer)
            {
                dirX = -1;
            }
            /*if (Mathf.Abs(transform.position.x - playerTransform.position.x) < distFromPlayer)
            {
                dirX = 0;
            }*/
            else { dirX = 0; }
        }
        else
        {
            Debug.Log("Player Jumped");
            playerJumped = false;
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

        if (dirX > 0f || rb.velocity.x > 0f)
        {
            //spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            state = 1;
        }
        else if (dirX < 0f || rb.velocity.x < 0f)
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

    public void ChangePlayerJumped(Vector3 _playerJumpPoint)
    {
        playerJumped = true;
        playerJumpPoint = _playerJumpPoint;
    }
}
