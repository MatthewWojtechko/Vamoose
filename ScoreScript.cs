using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text scoreTimer;
    public Text shadow;
    public Text DiamondCount;
    public Text SecondsFinal;
    public Text DiamondsFinal;
    public Text ScoreFinal;
    public int min;
    public int sec;
    public int diamondCount;
    public bool hasWon = false;
    //private int highMin;
    //private int highSec;

    void Start()
    {
        LoadScore();

        StartCoroutine("ScoreTimer");
            /*
            if (highSec < 10)
            {
                highScore.text = ("Best Time: " + highMin + ":0" + highSec);
            }
            else
            {
                highScore.text = ("Best Time: " + highMin + ":" + highSec);
            }
            */
    }

    void Update()
    {
        if (!hasWon)
        {
            DiamondCount.text = ("x " + diamondCount);

            if (sec < 10)
            {
                scoreTimer.text = (min + ":0" + sec);
                shadow.text = (min + ":0" + sec);
            }
            else
            {
                scoreTimer.text = (min + ":" + sec);
                shadow.text = (min + ":" + sec);
            }
        }
        else
        {
            scoreTimer.text = "";
            shadow.text = "";
            DiamondCount.text = "";

            

            int totalSeconds = (min * 60) + sec;
            int newScore = totalSeconds - diamondCount;
            SecondsFinal.text = totalSeconds + " seconds";
            DiamondsFinal.text = "- " + diamondCount + " diamonds";
            ScoreFinal.text = "= " + newScore;
        }
        
    }

    IEnumerator ScoreTimer()
    {
        while (!hasWon)
        {
            yield return new WaitForSeconds(1);
            sec++;

            if (sec >= 60)
            {
                int scoreCorrect;

                scoreCorrect = sec - 60;
                min += 1;
                sec = 0 + scoreCorrect;
            }
        }
    }

    public void LoadScore()
    {
        sec = 0;
        min = 0;
        diamondCount = 0;
        /*
        highSec = PlayerPrefs.GetInt("highSec");
        highMin = PlayerPrefs.GetInt("highMin");
        */
    }
}