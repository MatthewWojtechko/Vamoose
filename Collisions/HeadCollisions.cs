using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisions : MonoBehaviour
{
    public GameObject Player;
    public GameObject bodypartPartner;

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
        Vector2 startPosition1, startPosition2, mainDirection, middleDirection;
        startPosition1 = this.transform.position;
        startPosition2 = bodypartPartner.transform.position;
        mainDirection = new Vector2(0, 1);
        middleDirection = new Vector2(-1, 0);

        // initialize proper directions and distances

        if (pMovement.yMovement > 0)
            seekDistance = pMovement.yMovement * Time.deltaTime; // distance is how far will move in a frame
        else
            seekDistance = 0.1f * Time.deltaTime;

        middleDistance = this.transform.position.x - startPosition2.x;
        if (middleDistance < 0)
            middleDistance *= -1;

        // raycast
        RaycastHit2D[] rayHits1 = Physics2D.RaycastAll(startPosition1, mainDirection, seekDistance);
        Debug.DrawRay(startPosition1, mainDirection * seekDistance, Color.blue, 1);

        RaycastHit2D[] rayHits2 = Physics2D.RaycastAll(startPosition2, mainDirection, seekDistance);
        Debug.DrawRay(startPosition2, mainDirection * seekDistance, Color.red, 1);

        RaycastHit2D[] rayHits3 = Physics2D.RaycastAll(new Vector2(startPosition1.x, startPosition1.y + seekDistance), middleDirection, middleDistance);
        Debug.DrawRay(new Vector2(startPosition1.x, startPosition1.y + seekDistance), middleDirection * middleDistance, Color.green, 1);


        // Find which ray hit the ceiling, if any, and make the player fall if so
        RaycastHit2D rayThatHitCeiling = new RaycastHit2D();
        bool ceilingHit = false;
        foreach (RaycastHit2D hit in rayHits1)
        {
            if (hit.collider != null && hit.collider.tag == "Platform")
            {
                ceilingHit = true;
                rayThatHitCeiling = hit;
                break;
            }
        }
        if (!ceilingHit)
        {
            foreach (RaycastHit2D hit in rayHits2)
            {
                if (hit.collider != null && hit.collider.tag == "Platform")
                {
                    ceilingHit = true;
                    rayThatHitCeiling = hit;
                    break;
                }
            }
        }
        if (!ceilingHit)
        {
            foreach (RaycastHit2D hit in rayHits3)
            {
                if (hit.collider != null && hit.collider.tag == "Platform")
                {
                    ceilingHit = true;
                    rayThatHitCeiling = hit;
                    break;
                }
            }
        }

        if (ceilingHit && pMovement.yMovement > 0)
        {
            pMovement.isJumping = false;
            pMovement.isFalling = true;
            pMovement.yMovement = 0;
            float newPos = rayThatHitCeiling.point.y - (playerYDim / 2) + 0.11f;
            Player.transform.position = new Vector3(Player.transform.position.x, newPos, Player.transform.position.z);
        }
    }
}
