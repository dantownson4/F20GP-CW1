using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static int score;
    public int targetScore;
    public int maxScore;
    public static bool playerOnFinish;
    public Text scoreDisplay;
    public Text finishText;
    public Text finishText2;
    public Text targetText;
    public Text ammoDisplay;
    public Text gameOverText;

    private AudioSource winSound;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        maxScore += (GameObject.FindGameObjectsWithTag("ShootTarget").Length * 20);
        maxScore += (GameObject.FindGameObjectsWithTag("BasketballHoop").Length * 25);
        maxScore += (GameObject.FindGameObjectsWithTag("Trophy").Length * 50);

        targetText.text = "Target: " + targetScore;

        gameOverText.text = " ";

        winSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMove.alive)
        {
            GameOver();
        }

        scoreDisplay.text = score.ToString() + " / " + maxScore;
        ammoDisplay.text = "Ammo: " + PlayerMove.projAmmo;

        if (playerOnFinish)
        {
            FinishMessage();
        }
        else
        {
            clearMessage();
        }

        
    }

    public void FinishMessage()
    {
        if (score == maxScore)
        {
            finishText.text = "You have earned all possible points!";
            finishText2.text = "Press enter to finish.";

            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameWon();
            }

        }
        else if (score >= targetScore)
        {
            finishText.text = "You have earned enough points!";
            finishText2.text = "Press enter to finish.";

            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameWon();
            }
        }
        else
        {
            finishText.text = "You do not have enough points.";
            finishText2.text = "Earn " + (targetScore - score) + " more points to finish";
        }
    }

    public void clearMessage()
    {
        finishText.text = "";
        finishText2.text = "";
    }

    void GameOver()
    {
        gameOverText.text = "GAME OVER";
        gameOverText.color = Color.red;
    }

    void GameWon()
    {
        gameOverText.text = "YOU WON!";
        gameOverText.color = Color.green;
        winSound.Play();
    }
}
