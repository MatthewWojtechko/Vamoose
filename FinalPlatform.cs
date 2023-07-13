using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlatform : MonoBehaviour {
    public Color winColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collObj = collider.gameObject;
        if (collObj.tag == "Player")
        {
            PlayerMovement PlayerMove = collObj.GetComponent<PlayerMovement>();
            if (PlayerMove.isFalling || PlayerMove.isGrounded)
                GetComponent<SpriteRenderer>().color = winColor;
        }
    }
}
