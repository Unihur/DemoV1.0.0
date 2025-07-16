using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    //角色行走
    public float moveSpeed = 20.0f;
    private float moveX;
    public float jumpVelocity = 5.0f;
    private Rigidbody2D _rb;
    private Animator myAnim;

    //冲刺
    public float dashSpeed = 20.0f;
    public float dashTime = 1.0f;
    float startDashTimer;
    public bool isDashing = false;

    //二段跳
    public bool jumpAble = true;
    int jumpTimes;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        playerDash();

        PlayerJump();

        if(_rb.velocity.y<0)
        {
            _rb.velocity -= Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        }
        else if(_rb.velocity.y>0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        PlayerRun();
        Flip();
    }
    
    void Flip()
    {
        bool playerHasXSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
        if(playerHasXSpeed)
        {
            if(_rb.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            if (_rb.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    public void PlayerRun()
    {  
        //a和d控制左右移动
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        Vector2 playerVel = new Vector2(moveX ,_rb.velocity.y);
        _rb.velocity = playerVel;
        

        //动画
        bool playerHasXSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
        myAnim.SetBool("run",playerHasXSpeed);
    }

    public void PlayerJump()
    {
        //按“w"跳跃
        if (jumpAble == true)
        {
            jumpTimes = 1;
        }
        if (Input.GetKeyDown("w") && jumpTimes >= 0)
        {
            _rb.velocity = Vector2.up * jumpVelocity * 5;
            jumpTimes --;
        }
    }

    public void playerDash()
    {
        if (!isDashing)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDashing= true;
                startDashTimer = 0.1f;
            }
        }
        else
        {
            startDashTimer -= Time.deltaTime;
            if(startDashTimer <= 0 ) 
            {
                isDashing= false;
            }
            else
            {
                _rb.velocity = transform.right * 2000;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //在地面上
        if (collision.gameObject.tag == "Ground")
        {
            jumpAble = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //离开地面
        if (collision.gameObject.tag == "Ground")
        {
            jumpAble = false;
        }
    }
}
