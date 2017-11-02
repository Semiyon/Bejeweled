using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SM_PauseMenu : MonoBehaviour {
	
	//Author: Shane Meitzner
	//Date: 10/30/2017

    public Transform canvas;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       // if (Input.GetKeyDown(KeyCode.Escape))
       // {
            //if (canvas.gameObject.activeInHierarchy == false)
           // {
         //       canvas.gameObject.SetActive(true);
         //       Time.timeScale = 0;
         //   }
         //   else
         //   {
         //       canvas.gameObject.SetActive(false);
         //       Time.timeScale = 1;
         //   }
       // }
	
	}
	public void PauseMenu (){
		if (canvas.gameObject.activeInHierarchy == false)
		{
			canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
		}
		else
		{
			canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}
}
