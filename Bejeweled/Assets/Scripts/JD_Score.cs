using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JD_Score : MonoBehaviour
{

    //Author: Jacob Dunbar
    //Date: 10/31/17

    int score;
    int highScore;
    public Text text;
    public Text highscoreText;


    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        score = 0;
        highScore = PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score++;
        }
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
            highscoreText.text = "Highscore:" + highScore;
        }
        text.text = "Score:" + score;
        // displays highscore 
        highscoreText.text = "HighScore:" + highScore;
    }
}
