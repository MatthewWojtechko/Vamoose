using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAction : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject collisionObj = collision.gameObject;
        //if (this.tag == "Ground")
        //{
        //    // if it's a non-jumping player, ground them
        //    if (collisionObj.tag == "Player")
        //    {
        //        PlayerMovement pMove = collisionObj.GetComponent<PlayerMovement>();
        //        if (!pMove.isJumping)
        //        {
        //            collisionObj.transform.position = new Vector3(collisionObj.transform.position.x, transform.position.y, collisionObj.transform.position.z);
        //            pMove.setGrounded();
        //        }
        //    }
        //}
        //else if (this.tag == "LeftWall" || this.tag == "RightWall")
        //{
        //    PlayerMovement pMove = collisionObj.GetComponent<PlayerMovement>();
        //    pMove.hitWall(this.tag);
        //    collisionObj.transform.position = new Vector3(this.transform.position.x, collisionObj.transform.position.y, collisionObj.transform.position.z);
        //}
        //else if (this.tag == "HorizontalWall")
        //{
        //    PlayerMovement pMove = collisionObj.GetComponent<PlayerMovement>();
        //    pMove.hitWall(this.tag);
        //    collisionObj.transform.position = new Vector3(collisionObj.transform.position.x, this.transform.position.y, collisionObj.transform.position.z);
        //}

    }
}
