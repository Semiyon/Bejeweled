﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JD_Timer : MonoBehaviour {

    float timeLeft = 60.0f;

    public Text text;


    void Update()
    {
        if (GameObject.Find("Timer").GetComponent<JD_Score>().scored) ;
        {

        }
        timeLeft -= Time.deltaTime;
        text.text = "" + Mathf.Round(timeLeft);
        if (timeLeft <= 0)
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
