using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM_ButtonManager : MonoBehaviour{

	//Author: Shane Meitzner
	//Date: 10/30/2017
	
	public Transform canvas;

	public void StartGameButton (string loadLevel){
		SceneManager.LoadScene (loadLevel);
	}
	public void QuitGameButton (string quitGame){
		Application.Quit ();
	}
	public void CreditsGameButton (string showCredits){
		SceneManager.LoadScene (showCredits);
	}
	public void MainMenuButton (string mainMenu){
		SceneManager.LoadScene (mainMenu);
	}
    public void ControlsButton (string showControls) {
        SceneManager.LoadScene (showControls);
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

