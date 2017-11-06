﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public int score;
    private int x;
    private int y;

    public int X
    {
        get { return x; }
        set {
            if (IsMoveable())
            {
                x = value;
            }
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            if (IsMoveable())
            {
                y = value;
            }
        }
    }

    private Grid.PieceType type;

    public Grid.PieceType Type
    {
        get { return type; }
    }

    private Grid grid;

    public Grid GridRef
    {
        get { return grid; }
    }

    private MoveablePiece moveableComponent;

    public MoveablePiece MoveableComponent
    {
        get { return moveableComponent; }
    }

    private ColorPiece colorComponent;

    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }

    private ClearablePiece clearableComponent;

    public ClearablePiece ClearableComponent
    {
        get { return clearableComponent; }
    }


    private void Awake()
    {
        moveableComponent = GetComponent<MoveablePiece>();
        colorComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int _x, int _y,Grid _grid, Grid.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    void OnMouseEnter()
    {
        grid.EnterPiece(this);
    }

     void OnMouseDown()
    {
        grid.PressPiece(this);
    }

     void OnMouseUp()
    {
        grid.ReleasePiece();
    }

    public bool IsMoveable()
    {
        return moveableComponent != null;
    }

    public bool IsColored()
    {
        return colorComponent != null;
    }

    public bool IsClearable()
    {
        return clearableComponent != null;
    }

}
