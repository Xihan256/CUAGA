//修改于2023.1.8

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float XSpeed = 7f;
    public float jumpSpeed = 7f;
    public float secondJumpSpeed = 5f;
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 4f;
    public float dashSpeed = 50f;

    private float moveX;
    private float moveY;

    private Rigidbody2D playerRB;
    private Collider2D coll;
    private Animator anim;

    public LayerMask Terrian;

    public bool isOnGround, isJump;

    public bool jumpPressed;
    int jumpCount;

    public bool isDashing; 
    public bool hasDashed;

    public int dashCount;

    //private Collision collision;

    // Start is called before the first frame update
    void Start()
    {
        //collision = GetComponent<Collision>();
        playerRB = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround)
        {
            hasDashed = false;
            dashCount = 1;
            jumpCount = 2;
        }

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        //跳跃手感优化
        if (playerRB.velocity.y < 0)
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRB.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.J) && !hasDashed && dashCount > 0 )
        {
            if (xRaw != 0 || yRaw != 0)
            {
                if (!(isOnGround && (yRaw < 0)))//TODO
                {
                    Dash(xRaw, yRaw);
                }
            }
        }

        

        if (!isDashing)
        {
            playerRB.gravityScale = 1;
        }
    }

    private void Dash(float x, float y)
    {
        hasDashed = true;

        Vector2 dir =  new Vector2(x, y);

        if (y != 0)
        {
            isOnGround = false;
        }
        
        dashCount--;
        playerRB.velocity = dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        playerRB.gravityScale = 0;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        playerRB.gravityScale = 3;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (isOnGround)
        {
            hasDashed = false;
        }
           
    }


    private void FixedUpdate()
    {
        if (!isDashing)
        {

        movement();
        }
        Jump();
    }

    private void movement()
    {
        moveX = Input.GetAxis("Horizontal");

        playerRB.velocity = new Vector2(XSpeed * moveX, playerRB.velocity.y);

    }

    private void Jump()
    {
        if(isOnGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if(jumpPressed && isOnGround)
        {
            isOnGround = false;
            isJump = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount>0 && isJump)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, secondJumpSpeed);
            jumpCount--;
            jumpPressed = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Grouding(collision, false);
    }



    private void Grouding(Collision2D col,bool exitstate)
    {
        if(exitstate)//离开为真
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                isOnGround = false;
        }
        else
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !isOnGround && col.contacts[0].normal==Vector2.up)//判断从上往下
            {
                isOnGround = true;
            }
            else if(col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !isOnGround && col.contacts[0].normal == Vector2.down)//判断从下往上
            {
                JumpCancle();
            }
        }

    }

    private void JumpCancle()
    {

    }
}
