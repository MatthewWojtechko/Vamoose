using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    // References to the player and its movement script
    private GameObject Player;
    private PlayerMovement PlayerMovementObj;
    private float playerXDim, playerYDim;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovementObj = Player.GetComponent<PlayerMovement>();
        SpriteRenderer rend = PlayerMovementObj.GetComponent<SpriteRenderer>();
        playerXDim = rend.bounds.size.x;
        playerYDim = rend.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //// Finds what this is - the player's head, body, or feet.
    //// Then, finds what the collision was, and produces the effect.
    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    GameObject collisionObj = collision.gameObject;
    //    float newPos;

    //    if (collisionObj.tag == "LevelController")
    //    {
    //        PlayerMovementObj.enabled = false;
    //        LevelController control = collisionObj.GetComponent<LevelController>();
    //        control.win();
    //    }
    //    if (this.tag == "PlayerHead")   // if their head hits a ceiling...
    //    {
    //        if (collisionObj.tag == "HorizontalWall" || collisionObj.tag == "LeftWall" || collisionObj.tag == "RightWall")
    //        {
    //            //PlayerMovementObj.hitWall(collisionObj.tag);
    //            //newPos = collisionObj.transform.position.y - (playerYDim / 2);
    //            //Player.transform.position = new Vector3(Player.transform.position.x, newPos, Player.transform.position.z);
    //            PlayerMovementObj.hitCeiling();
    //        }
    //    }
    //}
    //void OnTriggerStay2D(Collider2D collider)
    //{
    //    GameObject collisionObj = collider.gameObject;
    //    if (this.tag == "PlayerFeet")
    //    {
    //        if (collisionObj.tag == "Ground")   // if their feet hit the ground...
    //        {
    //            if (PlayerMovementObj.yMovement <= 0f)   // if the player isn't going up, ground them
    //            {
    //                float newPos = collisionObj.transform.position.y + (playerYDim / 2);
    //                Player.transform.position = new Vector3(Player.transform.position.x, newPos, Player.transform.position.z);
    //                PlayerMovementObj.setGrounded();
    //            }
    //        }
    //    }
    //    else if (this.tag == "UpperbodyCollider" || this.tag == "LowerbodyCollider")
    //    {
    //        float bumpOffset = 0f;
    //        //if (this.tag == "UpperbodyCollider")
    //        bumpOffset = 0.16f;
    //        //else // LowerBodyCollider
    //        //  bumpOffset = 0.1f;

    //        if (collisionObj.tag == "LeftWall")
    //        {
    //            PlayerMovementObj.hitWall(collisionObj.tag);
    //            //newPos = collisionObj.transform.position.x + bumpOffset;
    //            //Player.transform.position = new Vector3(newPos, Player.transform.position.y, Player.transform.position.z);
    //        }
    //        else if (collisionObj.tag == "RightWall")
    //        {
    //            PlayerMovementObj.hitWall(collisionObj.tag);
    //            //newPos = collisionObj.transform.position.x - bumpOffset;
    //            //Player.transform.position = new Vector3(newPos, Player.transform.position.y, Player.transform.position.z);
    //        }
    //    }
    //}

    //// Finds what this is - the player's head, body, or feet.
    //// Then finds what it was exiting, and produces the effect.
    //void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (this.tag == "PlayerHead")
    //    {
    //        if (collider.tag == "HorizontalWall")   // If head is exiting a ceiling...
    //            PlayerMovementObj.canMoveUp = true;
    //    }
    //    else if (this.tag == "UpperbodyCollider" || this.tag == "MiddlebodyCollider" ||
    //             this.tag == "LowerbodyCollider")
    //    {
    //        // if body is moving off of vertical wall
    //        if (collider.tag == "LeftWall")
    //            PlayerMovementObj.canMoveLeft = true;
    //        else if (collider.tag == "RightWall")
    //            PlayerMovementObj.canMoveRight = true;
    //    }
    //    else if (this.tag == "PlayerFeet")
    //    {
    //        // if feet no longer touching ground
    //        if (collider.tag == "Ground")
    //        {
    //            PlayerMovementObj.isGrounded = false;
    //        }
    //    }
    //}
}
