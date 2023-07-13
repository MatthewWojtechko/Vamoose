using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    public Pauser pauseScript;
    private GameObject Player;
    private PlayerMovement playerMovement;
    SpriteRenderer playerSprite;
    Animator playerAnimations;

    //anim controlls

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimations = GetComponent<Animator>();
    }

    public void Update()
    {
        if (playerMovement.isFalling)
        {
            playerAnimations.SetBool("running", false);
            playerAnimations.SetBool("standing", false);
            playerAnimations.SetBool("jumping", true);
        }
        else if (playerMovement.isGrounded)
        {
            playerAnimations.SetBool("running", true);
            playerAnimations.SetBool("standing", false);
            playerAnimations.SetBool("jumping", false);
        }
        else if (!Input.anyKey && (playerMovement.isGrounded == true))
        {
            playerAnimations.SetBool("running", false);
            playerAnimations.SetBool("standing", true);
            playerAnimations.SetBool("jumping", false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && (playerMovement.isGrounded == true))
        {
            playerSprite.flipX = true;
            playerAnimations.SetBool("running", true);
            playerAnimations.SetBool("standing", false);
            playerAnimations.SetBool("jumping", false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && (playerMovement.isGrounded == true))
        {
            playerSprite.flipX = false;
            playerAnimations.SetBool("running", true);
            playerAnimations.SetBool("standing", false);
            playerAnimations.SetBool("jumping", false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || (playerMovement.isFalling == true))
        {
            playerAnimations.SetBool("running", false);
            playerAnimations.SetBool("standing", false);
            playerAnimations.SetBool("jumping", true);
        }
    }

    public void LateUpdate()
    {
        if (!pauseScript.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerSprite.flipX = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerSprite.flipX = false;
            }
        }
    }
}