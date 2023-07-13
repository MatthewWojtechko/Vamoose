using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullslugAI : MonoBehaviour {

    [Range(1, 8)]
    [Tooltip("WAIT = 1; ALERT = 2; PREP = 3; WARN = 4; CHARGE = 5; STOP = 6; FALL = 7; RETURN = 1")]
    public int currentState = 0;
    public bool isTowardRightWayPoint;
    public GameObject sightPoint;
    public GameObject Player;
    public bool isHittingPlayer = false;
    public bool isPlayerInZone = false;

    [Header("Stop Positions (x values; infinity represents N/A)")]
    public float leftWallStop = Mathf.Infinity;
    public float rightWallStop = Mathf.Infinity;
    public float leftFallStop = Mathf.Infinity;
    public float rightFallStop = Mathf.Infinity;

    [Header("Waypoints (X values)")]
    public float leftWaypoint;
    public float rightWaypoint;

    [Header("Tuning")]
    public float walkSpeed;
    public float prepSpeed;
    public float chargeSpeed;
    public float chargeStartLerpConstant;
    public float chargeStopLerpConstant;
    [Tooltip("After turn, how long until can turn again")]
    public float chargeTurnTime;
    public float minChargeSpeed;
    public float alertPause;
    public float prepPause;
    public float warnPause;
    public float stopPause;
    [Tooltip("How far horizontally bullslug can spot player.")]
    public float sightDistance;
    [Tooltip("How far upwards the bullslug can see at its max sight distance.")]
    public float sightHeight;
    public float fallGravity;
    public float maxFallSpeed;
    public float flingConstant;
    public float minimumFlingSpeed;
    public float fallRotationStart;
    public float maxRotation;
    public float rotationConstant;

    [Header("Player Helpless")]
    public float helplessYFactorStart;
    public float helplessXFactorStart;
    public float helplessDuration;

    private bool isFacingRight = false;
    private bool isPlayerToRight = false;
    private bool isTiming = false;
    private float inclinedRayDistance;
    private float prepDistanceSoFar;
    private bool mustStartCharge = true;
    public float currentChargeSpeed = 0;
    private bool mustLerpTurn;
    private bool isOffGround = false;
    private float currentFallSpeed;
    private bool isMovingForward = true;
    private float currentRotation;
    private SpriteRenderer sRenderer;
    private float xMovement;
    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        // Find distance and angle of inclined ray. 
        inclinedRayDistance = Mathf.Sqrt((sightDistance * sightDistance) + (sightHeight * sightHeight));

        currentRotation = fallRotationStart;

        sRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        if (currentState == 1)
        {
            sRenderer.color = Color.blue;
            wait();
        }
        else if (currentState == 2)
        {
            sRenderer.color = Color.yellow;
            alert();
        }
        else if (currentState == 3)
        {
            sRenderer.color = Color.black;
            prep();
        }
        else if (currentState == 4)
        {
            sRenderer.color = Color.magenta;
            warn();
        }
        else if (currentState == 5)
        {
            sRenderer.color = Color.red;
            charge();
        }
        else if (currentState == 6)
        {
            sRenderer.color = Color.green;
            stop();
        }
        else if (currentState == 7)
        {
            sRenderer.color = Color.gray;
            fall();
        }
        else
        {
            reset();
        }
	}

    // State 1: Paces between two waypoints.
    private void wait()
    {
        mustStartCharge = true;
        mustLerpTurn = false;
        if (isTowardRightWayPoint)
        {
            isFacingRight = isMovingForward = true;
            move(walkSpeed);
            if (this.transform.position.x >= rightWaypoint)
            {
                isTowardRightWayPoint = false;
            }
        }
        else
        {
            isFacingRight = false;
            isMovingForward = true;
            move(walkSpeed);
            if (this.transform.position.x <= leftWaypoint)
            {
                isTowardRightWayPoint = true;
            }
        }

        if (isPlayerSpot())
        {
            currentState = 2;
        }
    }

    // State 2: Stops and shows it has spot the player.
    private void alert()
    {
        if (!isTiming)
        {
            StartCoroutine(waitAlert());
        }
    }

    // State 3: walks backwards a bit, gathering momentum.
    private void prep()
    {
        if (!isTiming)
            StartCoroutine(waitPrep());

        trackPlayer();
        isMovingForward = false;
        move(prepSpeed);
    }


    // State 4: Stops for a bit, telegraphing the charge that follows.
    private void warn()
    {
        if (!isTiming)
            StartCoroutine(waitWarn());
    }

    // State 5: charges toward player, following
    private void charge()
    {
        if (!isPlayerInZone)   // If player out of zone, set to slow to 0 if above. If not above, then reset states.
        {
            if (currentChargeSpeed > 0.1f)
            {
                mustLerpTurn = true;
                mustStartCharge = false;
            }
            else
            {
                Debug.Log("f");
                currentState = 8;
            }
        }
        else
        {
            if (!isTiming)  // turn bullslug if needed
            {
                bool tempDir = isFacingRight;
                trackPlayer();
                if (tempDir != isFacingRight && currentChargeSpeed >= 0.1)
                {
                    mustLerpTurn = true;   
                    mustStartCharge = false;     
                    StartCoroutine(turnTimer());
                }
            }

        }

        if (mustStartCharge)  // if starting, lerp up to speed with start constant
        {
            currentChargeSpeed = Mathf.Lerp(currentChargeSpeed, chargeSpeed, chargeStartLerpConstant);
            isMovingForward = true;
            moveCharge(currentChargeSpeed);
            if (currentChargeSpeed >= chargeSpeed - 0.1f)
                mustStartCharge = false;
        }
        else if (mustLerpTurn) // if turning, lerp the speed to 0
        {
            currentChargeSpeed = Mathf.Lerp(currentChargeSpeed, 0, chargeStopLerpConstant);
            isMovingForward = false;
             moveCharge(currentChargeSpeed);
            if (isOffGround)
            {
                currentState = 7;
            }
            if (currentChargeSpeed <= 0.1f)  // Now that we've lerped to 0, need to start back up
            {
                mustLerpTurn = false;
                mustStartCharge = true;
                isMovingForward = true;
            }
        }
        else // else, keep at the same max charge speed!
        {
            isMovingForward = true;
            moveCharge(chargeSpeed);
            if (isOffGround)
            {
                currentState = 7;
            }
        }
        if (isHittingPlayer) // rammer adjusts field this automatically
            currentState = 6;
    }

    // State 6: Show some impact from ramming
    private void stop()
    {
        if (!isTiming)
        {
            isHittingPlayer = false;
            StartCoroutine(waitStop());
        }
    }

    // State 7: fall to death
    private void fall()
    {
        float movement;
        if (currentChargeSpeed < minimumFlingSpeed)
            movement = Mathf.Lerp(currentChargeSpeed, minimumFlingSpeed, flingConstant) * Time.deltaTime;
        else movement = currentChargeSpeed;
        if (isFacingRight)
        {
            if (!isMovingForward)
                movement *= -1;
        }
        else // facing left
        {
            if (isMovingForward)
                movement *= -1;
        }

        currentFallSpeed = Mathf.Lerp(currentFallSpeed, maxFallSpeed, fallGravity);

        this.transform.position = new Vector2(transform.position.x + (movement * Time.deltaTime), transform.position.y - (currentFallSpeed * Time.deltaTime));
        float rot = Mathf.Lerp(currentRotation, maxRotation, rotationConstant);
        if (transform.position.x >= rightFallStop)
            this.transform.Rotate(0, 0, -1 * rot * Time.deltaTime);
        else
            this.transform.Rotate(0, 0, rot * Time.deltaTime);

        
    }

    // State 8: revert values to return to state 1
    private void reset()
    {
        if (this.transform.position.x < leftWaypoint)
        {
            isTowardRightWayPoint = true;
            isFacingRight = true;
        }
        else
        {
            isTowardRightWayPoint = false;
            isFacingRight = false;
        }

        currentState = 1;
    }

    // Moves based on a given left/right orienatation and speed.
    // Flips the bullslug if needed, and stops it from backing into a wall or off the platform.
    private void move(float speed)
    {
        // Find the amount to move, taking into account direction.
        // Also reflects bullslug to face the proper direction in-game.
        float movement = speed * Time.deltaTime;
        if (isFacingRight)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            if (!isMovingForward)
                movement *= -1;
        }
        else // facing left
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
            if (isMovingForward)
                movement *= -1;
        }
        xMovement = movement;


        // Move
        float newPos = this.transform.position.x + movement;
        if (!isMovingForward)
        {
            if (canMoveBack())    
                this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
        else
            this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
    }

    // Essentially the regular move function, except it ensures the bullslug does not move forward into a platform either.
    // It DOES allow the bullslug to move OFF a platform.
    // If the bullslug hits a wall, it stops, and it set to not lerp a trun, and to start up again.
    private void moveCharge(float speed)
    {
        // Find the amount to move, taking into account direction.
        // Also reflects bullslug to face the proper direction in-game.
        float movement = speed * Time.deltaTime;
        if (isFacingRight)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            if (!isMovingForward)
                movement *= -1;
        }
        else // facing left
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
            if (isMovingForward)
                movement *= -1;
        }
        xMovement = movement;


        // Move
        float newPos = this.transform.position.x + movement;
        if (!isMovingForward)
        {
            if (canMoveBackOnlyPlatforms())
                this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
        else
        {
            if ((movement > 0 && canMoveRightOnlyPlatforms()) || (movement < 0 && canMoveLeftOnlyPlatforms()))
                this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
            else
            {
                mustLerpTurn = false;
                mustStartCharge = true;
                currentChargeSpeed = 0;
            }
        }
    }

    // Returns true if player is in sight - else false.
    // Uses rays to create a cone of sight - player is seen if the player comes in contact
    // with it. Note: if any part of the cone hits something non-player, false is returned. This 
    // should allow the player to hide behind even the smallest cover to escape the bullslug's sight.
    // This also means that care must be taken this cone does not touch the ceiling above it, 
    // or the bullslug will never spot player.
    private bool isPlayerSpot()
    {
        // Set ray directions and finds vertical ray's start position
        Vector2 horizontalRayDir, inclinedRayDir, verticalRayDir; 
        Vector2 verticalRayStart;  
        if (sightPoint.transform.position.x >= this.transform.position.x) // bullslug is facing right
        {
            horizontalRayDir = new Vector2(1, 0);
            inclinedRayDir = new Vector2(sightDistance, sightHeight);
            inclinedRayDir.Normalize();
            verticalRayStart = new Vector2(sightPoint.transform.position.x + sightDistance, sightPoint.transform.position.y);
        }
        else
        {
            horizontalRayDir = new Vector2(-1, 0);
            inclinedRayDir = new Vector2(-1 * sightDistance, sightHeight);
            inclinedRayDir.Normalize();
            verticalRayStart = new Vector2(sightPoint.transform.position.x - sightDistance, sightPoint.transform.position.y);
        }
        verticalRayDir = new Vector2(0, 1);
        
        // Raycast
        RaycastHit2D[] horizontalRayHits = Physics2D.RaycastAll(sightPoint.transform.position, horizontalRayDir, sightDistance);
        RaycastHit2D[] verticalRayHits = Physics2D.RaycastAll(verticalRayStart, verticalRayDir, sightHeight);
        RaycastHit2D[] inclinedRayHits = Physics2D.RaycastAll(sightPoint.transform.position, inclinedRayDir, inclinedRayDistance);

        // Draw Rays - for debugging
        Debug.DrawRay(sightPoint.transform.position, horizontalRayDir * sightDistance, Color.blue, Time.deltaTime);
        Debug.DrawRay(verticalRayStart, verticalRayDir * sightHeight, Color.green, Time.deltaTime);
        Debug.DrawRay(sightPoint.transform.position, inclinedRayDir * inclinedRayDistance, Color.red, Time.deltaTime);

        // Determine if rays spot player
        // If both rays inclined and horizontal rays hit a ground before the player, then bullslug never spots player.
        // If either of those two rays hit the player, but the one that did hit a ground first, never spots player.
        // In all other conditions, if any ray hits the player, the player is spot.
        bool inclinedSpotGroundFirst = false,
             horizontalSpotGroundFirst = false,
             inclinedSpotPlayer = false,
             horizontalSpotPlayer = false,
             verticalSpotPlayer = false;

        foreach (RaycastHit2D iHit in inclinedRayHits)
        {
            if (iHit.collider != null)
            {
                if (iHit.collider.tag == "Platform" && !inclinedSpotPlayer)
                    inclinedSpotGroundFirst = true;
                else if (iHit.collider.tag == "Player")
                    inclinedSpotPlayer = true;
            }
        }

        foreach (RaycastHit2D hHits in horizontalRayHits)
        {
            if (hHits.collider != null)
            {
                if (hHits.collider.tag == "Platform" && !horizontalSpotPlayer)
                    horizontalSpotGroundFirst = true;
                else if (hHits.collider.tag == "Player")
                    horizontalSpotPlayer = true;
            }
        }

        foreach (RaycastHit2D vHit in verticalRayHits)
        {
            if (vHit.collider != null)
            {
                if (vHit.collider.tag == "Player")
                    verticalSpotPlayer = true;
            }
        }

        if (inclinedSpotGroundFirst && horizontalSpotGroundFirst)
            return false;
        else if (inclinedSpotPlayer && inclinedSpotGroundFirst)
            return false;
        else if (horizontalSpotPlayer && horizontalSpotGroundFirst)
            return false;
        else if (inclinedSpotPlayer || horizontalSpotPlayer || verticalSpotPlayer)
        {
            //Debug.Log("Spots:");
            //Debug.Log("inclined spot ground first: " + inclinedSpotGroundFirst);
            //Debug.Log("horizontal spot ground first: " + horizontalSpotGroundFirst);
            //Debug.Log("inclined spot player: " + inclinedSpotPlayer);
            //Debug.Log("hotizontal spot player: " + horizontalSpotPlayer);
            //Debug.Log("vertical spot player: " + verticalSpotPlayer);
            return true;
        }
        else
            return false;  
    }

    private IEnumerator waitAlert()
    {
        isTiming = true;
        yield return new WaitForSeconds(alertPause);
        currentState = 3;
        isTiming = false;
    } 

    private IEnumerator waitPrep()
    {
        isTiming = true;
        yield return new WaitForSeconds(prepPause);
        currentState = 4;
        isTiming = false;
    }

    private IEnumerator waitWarn()
    {
        isTiming = true;
        yield return new WaitForSeconds(warnPause);
        currentState = 5;
        isTiming = false;
    }

    private IEnumerator waitStop()
    {
        isTiming = true;
        isHittingPlayer = false;
        yield return new WaitForSeconds(stopPause);
        mustStartCharge = true;
        currentChargeSpeed = 0;
        mustLerpTurn = false;
        isMovingForward = true;
        currentState = 8;
        isTiming = false;
    }

    private IEnumerator turnTimer()
    {
        isTiming = true;
        yield return new WaitForSeconds(chargeTurnTime);
        isTiming = false;
    }

    // Returns true if there's nothing immediately behind, false if there is.
    private bool canMoveBack()
    {
        if (isFacingRight)
        {
            if (leftFallStop != Mathf.Infinity)
            {
                if (this.transform.position.x <= leftFallStop)
                    return false;
            }
            if (leftWallStop != Mathf.Infinity)
            {
                if (this.transform.position.x <= leftWallStop)
                    return false;
            }

        }
        else
        {
            if (rightFallStop != Mathf.Infinity)
            {
                if (this.transform.position.x >= rightFallStop)
                    return false;
            }
            if (rightWallStop != Mathf.Infinity)
            {
                if (this.transform.position.x >= rightWallStop)
                    return false;
            }
        }
        return true;
    }

    // Returns true if there're no platforms (only) immediately behind, false if there are.
    private bool canMoveBackOnlyPlatforms()
    {
        if (isFacingRight)
        {
            if (leftWallStop != Mathf.Infinity)
            {
                if (this.transform.position.x <= leftWallStop)
                    return false;
            }
        }
        else
        {
            if (rightWallStop != Mathf.Infinity)
            {
                if (this.transform.position.x >= rightWallStop)
                    return false;
            }
        }
        return true;
    }

    // Returns true if there's no platforms immediately to left, false if there are.
    private bool canMoveLeftOnlyPlatforms()
    {
         if (leftWallStop != Mathf.Infinity)
         {
             if (this.transform.position.x <= leftWallStop)
                 return false;
         }
        return true;
    }

    // Returns true if there's no platforms immediately to right, false if there are.
    private bool canMoveRightOnlyPlatforms()
    {
        if (rightWallStop != Mathf.Infinity)
        {
            if (this.transform.position.x >= rightWallStop)
                return false;
        }
        return true;
    }

    // determines whether bullslug should face left (false) or right (true)
    private void trackPlayer()
    {
        if (Player.transform.position.x - this.transform.position.x >= 0)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Platform")
        {
            isOffGround = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Platform")
            isOffGround = true;
    }
}
