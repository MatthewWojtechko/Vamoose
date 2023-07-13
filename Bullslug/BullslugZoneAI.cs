using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullslugZoneAI : MonoBehaviour {
    public GameObject bullslug;
    private BullslugAI bullAI;
	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        bullAI = bullslug.GetComponent<BullslugAI>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            bullAI.isPlayerInZone = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            bullAI.isPlayerInZone = true;
    }
}
