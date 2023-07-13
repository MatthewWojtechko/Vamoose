using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    public float rate;
    SpriteRenderer Rend;
	// Use this for initialization
	void Start () {
        Rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Rend.color.a > 0f)
            Rend.color = new Color(Rend.color.r, Rend.color.g, Rend.color.b, Rend.color.a - (Time.deltaTime * rate));
	}
}
