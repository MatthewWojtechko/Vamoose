using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamAI : MonoBehaviour {
    public float xSpeed;
    public GameObject Bullslug;

    public float helplessYFactorStart;
    public float helplessXFactorStart;
    public float helplessDuration;
    public float chargeImpactSpeed;

    private BullslugAI bullScript;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        bullScript = Bullslug.GetComponent<BullslugAI>();

        helplessYFactorStart = bullScript.helplessYFactorStart;
        helplessXFactorStart = bullScript.helplessXFactorStart;
        helplessDuration = bullScript.helplessDuration;
}
	
	// Update is called once per frame
	void Update () {
        
	}

    // updates the speed and returns true if charging currently
    // to be called by player upon collisions, to determine if collision
    // results in a toss.
    public bool isCharging()
    {
        xSpeed = bullScript.currentChargeSpeed;
        if (bullScript.currentState == 5 && xSpeed > bullScript.minChargeSpeed)
            return true;
        else
            return false;
    }

    // called by player when it makes an impactful collision
    public void playerHit()
    {
        bullScript.isHittingPlayer = true;
    }
}
