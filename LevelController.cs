using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public AudioSource music;
    public AudioSource winMusic;
    public GameObject SpawnPoint;
    private GameObject Player;
    private PlayerMovement playerMovement;
    private GameObject Lava;
    private LavaMovement lavaMovement;
    private int checkpoint;
    public AudioClip Diamond_Got;
    public AudioClip EarthQuake;
    public AudioClip LavaHurt;
    private GameObject TimerObject;
    private ScoreScript scoreScript;
    private GameObject ShakeObject;
    private CameraShake shakeScript;
    private SpriteRenderer FaderRend;
    private bool hasWon = false;

    // Use this for initialization
    void Start ()
    {
        music.loop = true;
        music.Play(0);
        Lava = GameObject.FindGameObjectWithTag("Lava");
        lavaMovement = Lava.GetComponent<LavaMovement>();
        TimerObject = GameObject.FindGameObjectWithTag("LevelController");
        scoreScript = TimerObject.GetComponent<ScoreScript>();
        ShakeObject = GameObject.FindGameObjectWithTag("MainCamera");
        shakeScript = ShakeObject.GetComponent<CameraShake>();
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        

        LevelInit();
    }
	
    public void Update()
    {
        //keep lava near player
        if (hasWon)
        {
            Lava.transform.position = new Vector3(0, 0, 0);
            if (music.isPlaying)
            {
                music.Pause();
                winMusic.Play(0);
            }
            if (Player.transform.position.y < 72)
            {
                Player.transform.position = new Vector3(-0.005f, Player.transform.position.y + (Time.deltaTime * 5), Player.transform.position.z);
                if (Player.transform.position.y >= 72)
                {
                    Player.transform.position = new Vector3(Player.transform.position.x, 72, Player.transform.position.z);
                    scoreScript.hasWon = true;
                }
            }
        }
        if (Player.transform.position.y - Lava.transform.position.y > 30)
        {
            Lava.transform.position = new Vector3(0, Player.transform.position.y - 20, -3);
        }
    }

    public void LevelInit()
    {
        checkpoint = 0;
        scoreScript.sec = 0;
        scoreScript.min = 0;

        //level intro
        //AudioSource.PlayClipAtPoint(EarthQuake, Camera.main.transform.position);
        //shakeScript.ShakeOn(2);
        //shakeScript.ShakeOff();
    }

    public void DiamondGet()
    {
        //increase diamondcount
        scoreScript.diamondCount += 1;
        //play a sound
        AudioSource.PlayClipAtPoint(Diamond_Got, Camera.main.transform.position);
    }

    public void CheckPoint()
    {
        checkpoint = 1;
        //shake screen, increase lava speed.
        lavaMovement.lavaSpeed = 0.7f;
    }

    public void LavaContact()
    {
        if (checkpoint == 1)
        {
            //play sound
            AudioSource.PlayClipAtPoint(LavaHurt, Camera.main.transform.position);

            //Set Player to checkpoint location. 
            Player.transform.position = SpawnPoint.transform.position;

            //Set lava
            Lava.transform.position = new Vector3(0, Player.transform.position.y - 20, -3);
        }
        else
        {
            //play sound
            AudioSource.PlayClipAtPoint(LavaHurt, Camera.main.transform.position);

            //Set Player to checkpoint location. 
            Player.transform.position = new Vector3(-5, -2, 0); ;

            //Set lava
            Lava.transform.position = new Vector3(0, Player.transform.position.y - 20, -3);
        }
    }

    public void win()
    {
        hasWon = true;
    }
}
