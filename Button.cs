using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {
    public string transitionScene;
    public AudioSource hoverSFX;
    public AudioSource clickSFX;

    SpriteRenderer Rend;
    private bool canPlayHoverSound = true;
    public bool clicked = false;

	// Use this for initialization
	void Start () {
        Rend = GetComponent<SpriteRenderer>();
        Rend.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        Rend.color = new Color(255f / 255f, 239f / 255f, 163f / 255f, 1f);
        if (canPlayHoverSound)
            hoverSFX.Play(0);
        canPlayHoverSound = false;

        if (Input.GetMouseButton(0) && !clicked)
        {
            clicked = true;
            StartCoroutine(transition());
        }
    }

    void OnMouseExit()
    {
        Rend.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
        canPlayHoverSound = true;
    }

    IEnumerator transition()
    {
        clickSFX.Play(0);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(transitionScene);
    }
}
