using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    [Header("Body Colliders")]
    public BodyCollisions[] rightCollisions;
    public BodyCollisions[] leftCollisions;

    [Header("Vertical State")]
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isGrounded = true;

    [Header("Movement Statuses")]
    public float xMovement;
    public float yMovement;
    public Color normalColor;
    public bool canMoveLeft = true; // make these initial values DYNAMIC!!?
    public bool canMoveRight = true;
    public bool canMoveUp = true;

    [Header("Horizontal Movement")]
    public float normalWalkSpeed;
    public float aerialWalkSpeed;
    public float aerialWalkAccelerationConstant;
    public float aerialWalkingStopLerpConstant;
    public float floatWalkSpeed;
    public float floatWalkAccelerationConstant;
    public float floatWalkStopLerpConstant;
    public float floatWalkLerpConstant;
    public float jumpWalkStartSpeed;

    [Header("Normal Vertical Movement")]
    public float normalJumpSpeed;
    public float normalJumpStopLerpConstant;
    public float normalFallSpeed;
    public float normalGravityLerpConstant;
    public float normalJumpMaxHeight;

    [Header("Float Vertical Movement")]
    public float floatJumpHeight;
    public float floatJumpSpeed;
    public float floatJumpStopLerpConstant;
    public float floatFallSpeed;
    public float floatGravityLerpConstant;
    public float floatUpwardStopLerpConstant;
    public Color floatColor;

    [Header("Slam Vertical Movement")]
    public float slamSpeed;
    public Color slamColor;
    public float slamStopLerpConstant;
    private bool isSlamming = false;

    private bool enterHelpless;
    private bool currentlyHelpless;
    private float helplessYFactorStart;
    private float helplessXFactorStart;
    private float helplessDuration;
    private float chargeImpactSpeed;

    private float currentSmallJumpHeight;
    private bool isFloatJumpActive = false;
    private bool hasFloatJumpStarted = false;
    private SpriteRenderer sRenderer;
    private float xUpdate, yUpdate;
    private bool upKeyReleased = true;
    private bool alreadySlammed = false;
    private bool facingRight = true;
    private Vector3 lastPosition;
    private bool isInSlime = false;
    //private bool isOnSlugBack;
    //private GameObject slug;

    // Use this for initialization
    void Start() {
        lastPosition = transform.position;
        xMovement = 0;
        yMovement = 0;
        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.color = normalColor;

    }

    // Update is called once per frame
    void Update() {
        canMoveLeft = canMoveRight = true;
        for (int i = 0; i < rightCollisions.Length; i++)
        {
            if (rightCollisions[i].canMove == false)
                canMoveRight = false;
            if (leftCollisions[i].canMove == false)
                canMoveLeft = false;
        }


        if (isGrounded && !isJumping)   // default values so long as player is on ground
        {
            isFalling = false;
            isGrounded = true;
            hasFloatJumpStarted = false;
            isFloatJumpActive = false;
            yMovement = 0f;
            currentSmallJumpHeight = 0f;
            alreadySlammed = false;
        }

        if (enterHelpless)
            StartCoroutine(beHelpless());
        else if (!currentlyHelpless)
            move();
        else
            helplessMove();
            
        //setColor();

        if (Input.GetKey("up"))
            upKeyReleased = false;
        else
            upKeyReleased = true;

        lastPosition = transform.position;

    }

    void setColor()
    {
        if (isFloatJumpActive && isJumping)
            sRenderer.color = floatColor;
        else if (isSlamming)
            sRenderer.color = slamColor;
        else
            sRenderer.color = normalColor;
    }

    void move()
    {
        // Set xMovement and yMovement to proper values, given player input and collisions.
        getXMovement();
        getYMovement();

        xUpdate = xMovement * Time.deltaTime;
        yUpdate = yMovement * Time.deltaTime;

        // if the player is trying to move into a wall, update x motion to 0
        if ((xMovement > 0f && !canMoveRight) || (xMovement < 0f && !canMoveLeft))
            xMovement = xUpdate = 0f;
        else if (isSlamming)    // if slamming, no don't move in x, but keep momentum saved
            xUpdate = 0f;

        if (!canMoveUp && yMovement > 0f) // if trying to move up into ceiling, time to fall
        {
            yMovement = yUpdate = 0f;
            isFalling = true;
            canMoveUp = true;
        }

        // Move!
        //if (!isOnSlugBack)
            transform.position = new Vector3(transform.position.x + xUpdate, transform.position.y + yUpdate, transform.position.z);
        //else // if on bullslug, movement is relative to it
        //{
        //    transform.position = new Vector3(slug.transform.position.x, transform.position.y + yUpdate, transform.position.z);
        //}
    }

    void helplessMove()
    {
        xUpdate = xMovement * Time.deltaTime;
        yUpdate = yMovement * Time.deltaTime;
        // if the player is trying to move into a wall: BOUNCE off it!
        if ((xMovement > 0f && !canMoveRight) || (xMovement < 0f && !canMoveLeft))
        {
            xMovement *= -1;
            xUpdate *= -1;
        }

        if (!canMoveUp && yMovement > 0f) // if trying to move up into ceiling: BOUNCE off that too!
        {
            yMovement *= -1;
            yUpdate *= -1;
        }

        // Move!
        transform.position = new Vector3(transform.position.x + xUpdate, transform.position.y + yUpdate, transform.position.z);
    }

    void getXMovement()
    {
        // if grounded
        if (isGrounded)
        {
            // Set movement to walk in left or right direction given proper input AND if the player is NOT inputting a jump.
            // If player IS inputting a jump, the xMovement should start at (+/-) jumpWalkStartSpeed.
            // If there's no input at all, xMovement is 0.
            if (!Input.GetKey("up"))
            {
                if (Input.GetKey("left"))
                    xMovement = -1 * normalWalkSpeed;
                else if (Input.GetKey("right"))
                    xMovement = normalWalkSpeed;
                else
                    xMovement = 0f;
            }
            else
            {
                if (Input.GetKey("left"))
                    xMovement = -1 * jumpWalkStartSpeed;
                else if (Input.GetKey("right"))
                    xMovement = jumpWalkStartSpeed;
                else
                    xMovement = 0;
            }
        }
        // if normally airborne
        else if (!isFloatJumpActive)
        {
            // If inputting left or right: accelerate in that direction.
            // If going the opposite direction, first set movement to intial walking speed for jump.
            if (Input.GetKey("left"))
            {
                if (xMovement > -1 * jumpWalkStartSpeed)
                    xMovement = -1 * jumpWalkStartSpeed;
                xMovement = -1 * Mathf.Lerp(-1 * xMovement, aerialWalkSpeed, aerialWalkAccelerationConstant); //xMovement = -1 * aerialWalkSpeed;
            }
            else if (Input.GetKey("right"))
            {
                if (xMovement < jumpWalkStartSpeed)
                    xMovement = jumpWalkStartSpeed;
                xMovement = Mathf.Lerp(xMovement, aerialWalkSpeed, aerialWalkAccelerationConstant);  //xMovement = aerialWalkSpeed;
            }
            // Else no input. So, naturally decelerate movement.
            else
            {
                if (xMovement < 0f)
                {
                    if (xMovement > -0.1f)
                        xMovement = 0f;
                    else
                        xMovement = -1 * Mathf.Lerp(-1f * xMovement, 0f, aerialWalkingStopLerpConstant);
                }
                else
                {
                    if (xMovement < 0.1f)
                        xMovement = 0f;
                    else
                        xMovement = Mathf.Lerp(xMovement, 0f, aerialWalkingStopLerpConstant);
                }
            }
        }
        // Else, float jump is active. 
        else
        {
            // If motionless: if the left or right are input, accelerate in that direction.
            if (xMovement == 0f)
            {
                if (Input.GetKey("left"))
                    xMovement = -1 * Mathf.Lerp(-1 * xMovement, floatWalkSpeed, floatWalkAccelerationConstant); //xMovement = -1 * floatWalkSpeed;
                else if (Input.GetKey("right"))
                    xMovement = Mathf.Lerp(xMovement, floatWalkSpeed, floatWalkAccelerationConstant);  //xMovement = floatWalkSpeed;
            }
            // Else if moving to right: if right is input, accelerate in the right direction.
            //                          if left is input, decelerate towards 0.
            else if (xMovement > 0f) 
            {
                if (Input.GetKey("right") && xMovement <= floatWalkSpeed)
                    xMovement = floatWalkSpeed;
                else
                {
                    if (Input.GetKey("left"))
                        xMovement = Mathf.Lerp(xMovement, 0f, floatWalkLerpConstant * 6f);
                    else
                        xMovement = Mathf.Lerp(xMovement, 0f, floatWalkLerpConstant);
                }

                if (xMovement < 1.5f && Input.GetKey("left"))
                    xMovement = -1 * Mathf.Lerp(-1 * xMovement, floatWalkSpeed, floatWalkAccelerationConstant);
                else if (xMovement < 0.1f)
                    xMovement = 0f;
            }
            // Else moving to left:  if right is input, decelerate towards 0.
            //                       if left is input, accelerate in the left direction.
            else
            {
                if (Input.GetKey("left") && xMovement >= -1f * floatWalkSpeed)
                    xMovement = -1f * floatWalkSpeed;
                else
                {
                    if (Input.GetKey("right"))
                        xMovement = -1f * Mathf.Lerp(-1f * xMovement, 0f, floatWalkLerpConstant * 6f);
                    else
                        xMovement = -1f * Mathf.Lerp(-1f * xMovement, 0f, floatWalkLerpConstant);
                }

                if (xMovement > -1.5f && Input.GetKey("right"))
                    xMovement = Mathf.Lerp(xMovement, floatWalkSpeed, floatWalkAccelerationConstant);
                else if (xMovement > -0.1f)
                    xMovement = 0f;
            }
        }
    }

    void getYMovement()
    {
        if (Input.GetKey("down") && !isGrounded)
        {
            if (!alreadySlammed || isSlamming)  // if able to slam, or already slamming, then slam!
            {
                isSlamming = true;
                yMovement = -1f * slamSpeed;
                isJumping = false;
                isFalling = true;
                isFloatJumpActive = false;
                alreadySlammed = true;
            }
        }
        else if (!isFloatJumpActive)  // if normal jump or falling
        {
            isSlamming = false;
            getNormalYMovement();
            if (currentSmallJumpHeight > normalJumpMaxHeight)   // if reach small jump apex: float jump!
                isFloatJumpActive = true;
        }
        else   // else if floating: initiate float or calculate movement due to post-float falling
        {
            isSlamming = false;
            if (!hasFloatJumpStarted)
            {
                StartCoroutine(floatUpwards());
                hasFloatJumpStarted = true;
            }
            else // else float jump already started
            {
                if (isFalling)
                {
                    isJumping = false;
                    if (yMovement * -1f > floatFallSpeed + 0.5f)    // if going down fast from slamming, lerp into float fall
                        yMovement = Mathf.Lerp(yMovement, floatFallSpeed, slamStopLerpConstant);
                    else if (yMovement > 0f) // if still moving up, lerp to 0
                    {
                        yMovement = Mathf.Lerp(yMovement, 0f, floatUpwardStopLerpConstant);
                        if (yMovement < 0.1f)
                        {
                            yMovement = 0f;
                        }
                    } // else, time to fall: lerp into negative numbers
                    else
                    {
                        yMovement = -1 * Mathf.Lerp(-1 * yMovement, floatFallSpeed, floatGravityLerpConstant);
                    }
                }
            }
        }
    }

    void getNormalYMovement()
    {
        if (Input.GetKey("up") && !isFalling && !isInSlime)   // jump!
        {
            if (isGrounded)
            {
                if (upKeyReleased)
                    yMovement = normalJumpSpeed;
            }
            else
                yMovement = normalJumpSpeed;
            currentSmallJumpHeight += yMovement * Time.deltaTime * 60f;
            isJumping = true;
        }
        else if (!isGrounded)   // else, falling, so if not grounded, start falling
        {
            if (yMovement * -1f > normalFallSpeed + 0.5f)   // if falling fast from slamming, lerp into normalFallSpeed
                yMovement = Mathf.Lerp(yMovement, normalFallSpeed, slamStopLerpConstant);
            else if (yMovement > 0f)
                yMovement = 0f; // lerpSmallFall();
            else
                yMovement = -1 * Mathf.Lerp(-1 * yMovement, normalFallSpeed, normalGravityLerpConstant); // yMovement = -1 * normalFallSpeed;
            isFalling = true;
            isJumping = false;
        }
    }

    void lerpSmallJump()
    {
        // if has not begun moving up, set movement; decrease movement
        if (yMovement == 0f)
            yMovement = normalJumpSpeed;
        yMovement = Mathf.Lerp(yMovement, 0f, normalJumpStopLerpConstant);
    }

    void lerpSmallFall()
    {
        // if moving up, stop; otherwise, negatively increase movement
        //if (yMovement > 0f)
        //yMovement = 0f;
        yMovement = -1 * Mathf.Lerp(-1 * yMovement, normalFallSpeed, normalGravityLerpConstant);
    }

    IEnumerator floatUpwards()
    {
        yMovement = floatJumpSpeed;
        yield return new WaitForSeconds(floatJumpHeight);
        isFalling = true;
    }


    public void hitCeiling()
    {
        transform.position = lastPosition;
        yMovement = 0f;
        isFalling = true;
    }

    IEnumerator beHelpless()
    {
        enterHelpless = false;
        currentlyHelpless = true;
        xMovement = helplessXFactorStart * chargeImpactSpeed;
        yMovement = helplessYFactorStart * chargeImpactSpeed;
        alreadySlammed = true;
        if (yMovement < 0)
            yMovement *= -1;
        isFloatJumpActive = true;
        isFalling = true;
        yield return new WaitForSeconds(helplessDuration);
        currentlyHelpless = false;
    }

    public void setGrounded()
    {
        isGrounded = true;
    }
    public void hitWall(string WallType)    // "LeftWall", "RightWall" or "HorizontalWall"
    {
        //if (WallType == "LeftWall")
        //{
        //    canMoveLeft = false;
        //}
        //else if (WallType == "RightWall")
        //    canMoveRight = false;
        //else if (WallType == "HorizontalWall")
        //    canMoveUp = false;
    }

    public void wentOffGround()
    {
        isGrounded = false;
    }

    // When rammed, set state to helpless.
    // When on Bullslug, note this.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Rammer")
        {
            RamAI impactScript = collider.gameObject.GetComponent<RamAI>();
            if (impactScript.isCharging())
            {
                impactScript.playerHit();
                enterHelpless = true;

                helplessYFactorStart = impactScript.helplessYFactorStart;
                helplessXFactorStart = impactScript.helplessXFactorStart;
                helplessDuration = impactScript.helplessDuration;

                float tempSpeed = impactScript.xSpeed;
                if (impactScript.Bullslug.transform.position.x > this.transform.position.x)
                    tempSpeed *= -1;
                chargeImpactSpeed = tempSpeed;
            }
        }
        else if (collider.gameObject.tag == "Slime")
        {
            isInSlime = true;
        }
        //else if (collider.gameObject.tag == "Bullslug")
        //{
        //    isOnSlugBack = true;
        //    slug = collider.gameObject;
        //}
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Slime")
        {
            isInSlime = false;
        }
    }



}