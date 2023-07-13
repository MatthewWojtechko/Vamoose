using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionExits : MonoBehaviour {
    private PlayerMovement pMovement;
    // Use this for initialization
    void Start()
    {
        pMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //// If escape wall, can move in that direction again.
    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    GameObject colObj = collision.gameObject;
    //    //if (colObj.tag == "LeftWall")
    //    //    pMovement.canMoveLeft = true;
    //    if (colObj.tag == "RightWall")
    //        pMovement.canMoveRight = true;
    //}

    //void OnTriggerEnter2d(Collider2D collision)
    //{
    //}
}
