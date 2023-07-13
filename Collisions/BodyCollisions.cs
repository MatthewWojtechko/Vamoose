
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollisions : MonoBehaviour {
    public GameObject Player;
    public GameObject bodypartPartner;
    public bool isRight;
    public bool canMove;
    public float extraClosenessNum; // how much closer to keep player to wall from this collision

    private float playerXDim, playerYDim;
    private PlayerMovement pMovement;
    private float seekDistance;
    private float middleDistance;
    // Use this for initialization
    void Start()
    {
        pMovement = Player.GetComponent<PlayerMovement>();
        SpriteRenderer rend = pMovement.GetComponent<SpriteRenderer>();
        playerXDim = rend.bounds.size.x;
        playerYDim = rend.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        // initialize positions, directions, and distances
        Vector2 startPosition1, startPosition2, mainDirection, middleDirection;
        startPosition1 = this.transform.position;
        startPosition2 = bodypartPartner.transform.position;

        if (isRight)
            mainDirection = new Vector2(1, 0); 
        else
            mainDirection = new Vector2(-1, 0);
        middleDirection = new Vector2(0, -1);

        if ((isRight && pMovement.xMovement > 0) || (!isRight && pMovement.xMovement < 0)) // if moving in the ray direction
            seekDistance = 1.5f * pMovement.xMovement * Time.deltaTime;
        else if (pMovement.xMovement == 0)
            seekDistance = 3 * Time.deltaTime;
        else
            seekDistance = 1f * Time.deltaTime;

        if (seekDistance < 0)
            seekDistance *= -1;
        middleDistance = this.transform.position.y - startPosition2.y;
        if (middleDistance < 0)
            middleDistance *= -1;

        // raycast
        RaycastHit2D[] rayHits1 = Physics2D.RaycastAll(startPosition1, mainDirection, seekDistance);
        Debug.DrawRay(startPosition1, mainDirection * seekDistance, Color.blue, 1);

        RaycastHit2D[] rayHits2 = Physics2D.RaycastAll(startPosition2, mainDirection, seekDistance);
        Debug.DrawRay(startPosition2, mainDirection * seekDistance, Color.red, 1);

        Vector2 ray3StartPos;
        if (isRight)
            ray3StartPos = new Vector2(startPosition1.x + seekDistance, startPosition1.y);
        else
            ray3StartPos = new Vector2(startPosition1.x - seekDistance, startPosition1.y);

        RaycastHit2D[] rayHits3 = Physics2D.RaycastAll(ray3StartPos, middleDirection, middleDistance);
        Debug.DrawRay(ray3StartPos, middleDirection * middleDistance, Color.green, 1);

        // Find which ray hit a wall, if any, and if one has, then stop player in proper direction.
        bool wallHit = false;
        RaycastHit2D rayHitWall = new RaycastHit2D();
        foreach (RaycastHit2D hit in rayHits1)
        {
            if (hit.collider != null && hit.collider.tag == "Platform")
            {
                rayHitWall = hit;
                wallHit = true;
                break;
            }
        }
        if (!wallHit)
        {
            foreach (RaycastHit2D hit in rayHits2)
            {
                if (hit.collider != null && hit.collider.tag == "Platform")
                {
                    rayHitWall = hit;
                    wallHit = true;
                }
            }
        }
        if (!wallHit)
        {
            foreach (RaycastHit2D hit in rayHits3)
            {
                if (hit.collider != null && hit.collider.tag == "Platform")
                {
                    rayHitWall = hit;
                    wallHit = true;
                }
            }
        }

        if (wallHit && ((isRight && pMovement.xMovement >= 0) || (!isRight && pMovement.xMovement <= 0)))
        {
            canMove = false;
            float newPos;
            if (isRight && pMovement.canMoveRight)
            {
                newPos = rayHitWall.point.x - (playerXDim / 2) + extraClosenessNum;
                Player.transform.position = new Vector3(newPos, Player.transform.position.y, Player.transform.position.z);
            }
            else if (!isRight && pMovement.canMoveLeft)
            {
                newPos = rayHitWall.point.x + (playerXDim / 2) - extraClosenessNum;
                Player.transform.position = new Vector3(newPos, Player.transform.position.y, Player.transform.position.z);
            }
            

        }
        else   // else, no wall hit, so let the player move.
        {
            canMove = true;
        }
    }
}