using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //config parrams
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Joystick joystick;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip deathSFX;
    float gravity;
    bool JumpTrigger = false;

    //state
    bool isAlive = true;

    //cathed refferences
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeet;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
        myFeet = GetComponentInChildren<BoxCollider2D>();
        gravity = myRigidBody.gravityScale;
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return; 
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
        
    }
    private void Die() 
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spike")))
            {
                isAlive = false;
                myAnimator.SetBool("IsDead", true);
                AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
                if(myRigidBody.velocity.y<=0)
                    myRigidBody.velocity = new Vector2 
                    ((myRigidBody.velocity.x + 6f) ,myRigidBody.velocity.y + 40f);
                else
                myRigidBody.velocity = new Vector2
                ((myRigidBody.velocity.x + 6f), myRigidBody.velocity.y + 20f);
            StartCoroutine(WaitAndDie());
        }
    }
    IEnumerator WaitAndDie() 
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
    private void Run()
    {
        //float controllThrow = joystick.Horizontal;
        float xMove = Input.GetAxisRaw("Horizontal");
        Vector2 newRunVelocity = new Vector2(xMove * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = newRunVelocity;
        bool isRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", isRunning);

        //Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        //myRigidbody.velocity = playerVelocity;

        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    private void Jump() 
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (JumpTrigger || Input.GetKeyDown(KeyCode.Space)) 
        {
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            Vector2 newJumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += newJumpVelocity;
            JumpTrigger = false;
        }
    }
    public void SetJumpTrigger() 
    {
        //JumpTrigger = true;
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
        Vector2 newJumpVelocity = new Vector2(0f, jumpSpeed);
        myRigidBody.velocity += newJumpVelocity;
    }
    private void FlipSprite() 
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
    private void ClimbLadder() 
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravity;
            myAnimator.SetBool("Climbing", false);
            return;
        }
        myRigidBody.gravityScale = 0;
        float climb = Input.GetAxisRaw("Vertical");
        Vector2 newClimbVelocity = new Vector2(myRigidBody.velocity.x, Mathf.Clamp(climb * climbSpeed, -5, 5));
        myRigidBody.velocity = newClimbVelocity;
        bool isClimbing = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", isClimbing);
    }
}
