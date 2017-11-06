using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public enum LevelType
    {
        TIMER,
        MOVES
    };

    public Grid grid;



    protected LevelType type;

    public LevelType Type
    {
        get { return type; }
    }

    protected int currentScore;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void GameWin()
    {
        Debug.Log("You win!");
        grid.GameOver();
    }

    public virtual void GameLose()
    {
        Debug.Log("You lose.");
        grid.GameOver();
    }

    public virtual void OnMove()
    {
        Debug.Log("You moved.");
    }

    public virtual void OnPieceCleared(GamePiece piece)
    {
        currentScore += piece.score;
        Debug.Log("Score: " + currentScore);
    }
}
