using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > 0.2f)
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * speed), transform.position.z);
        else if (transform.position.y < 0.2f)
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
    }
}
