using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement2 : MonoBehaviour
{

    //private GameManager gm;
    //public ParticleSystem dust;
    

    private float horizontal;
    private float speed = 9f;
    private float jumpingPower = 32f;
    private bool facingRight = true;
    private float hangTime = .2f;
    private float hangCounter;
    public float jumpBufferLength = .025f;
    private float jumpBufferCount;

    private bool wallSliding;
    private float wallSlidingSpeed = 2f;

    private bool wallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    
    //[SerializeField] GameObject Button1;
    //[SerializeField] GameObject PlatformButton1;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Transform player;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    
    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }
   
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal2");


        if(Grounded())
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump2"))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        if (jumpBufferCount >= 0 && hangCounter > 0f && Grounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpBufferCount = 0;
        }
        if (Input.GetButtonDown("Jump2") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        
        if(!wallJumping)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if(!wallJumping)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }

        if (horizontal != 0)
        {
            //CreateDust();
        }
    }

    private bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool Walled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (Walled() && !Grounded() && horizontal != 0f)
        {
            wallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            wallSliding = false;
        }
    }

    private void WallJump()
    {
        if (wallSliding || Walled())
        {
            wallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump2") && wallJumpingCounter > 0f)
        {
            wallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        wallJumping = false;
    }

    private void Flip()
    {
        if (facingRight && horizontal < 0f || !facingRight && horizontal > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /*void CreateDust()
    {
        if (horizontal != 0)
        {
            dust.Play();
        }
    }*/

    void OnCollisionEnter2D(Collision2D col)
    {
        /*if (col.gameObject.tag == "Lava" || col.gameObject.tag == "Enemy")
        {
            player.position = gm.lastCheckPointPos;
        }*/
    }
}
