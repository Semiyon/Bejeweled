using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Matthew Schuessler Grid Script 
//Reference: Lynda.com/Unity-tutorials/Create-grid/440670/489014-4.html?autoplay=true
//Some of the code may look very similar to the video, but I have done my best to make sure that my syntax as well as understanding of the code is prevalent

public class Grid : MonoBehaviour {
    //Start off by creating public parameters in which helps create the grid space, viewable in the inspector
    public int xDim;
    public int yDim;
    public float fillTime;
    public Level level;
    private GamePiece pressedPiece;
    private GamePiece enteredPiece;

    private bool gameOver = false;

    //An enum is being used for the piecetype as to make sure that the different color of gems are distinguishable and will help us match them later
    public enum PieceType
    {
        EMPTY,
        NORMAL,
        COUNT,
    };

    //We have created a Serialized struct that allows us to edit the piecetype and the prefab in the inspector
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    }

    //We are creating a dictionary to make sure that we can insert prefabs for each of the piece types.
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    //This allows us to create an array of the pieceprefabs
    public PiecePrefab[] piecePrefabs;

    //This allows us to add a background prefab if we decide to use sprites afterall, if not then we can just use the GameObjects that were created.
    public GameObject backgroundPrefab;

    //This is creating a 2d array for the pieces in order to instantiate them
    private GamePiece[,] pieces;

	// Use this for initialization
	void Start () {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();
        //loop through all of the pieces in the array
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            // We are checking to see if the dictionary contains the key type, if it does not then it will add it to the dictionary so that we can call it later
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        //these three lines are generating the background prefab onto the grid, making sure that the vector3 is not rotated by using the Quaternion.identity
        for (int x = 0; x < xDim; x++ )
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x,y), Quaternion.identity);
                //now we make the background a child
                background.transform.parent = transform;
            }
        }
        //Storing the array of the game pieces
        pieces = new GamePiece[xDim, yDim];
        //Same thing as above, looping through the variables in the array and making sure to spawn the pieces to the grid, this will only spawn the NORMAL piece though.
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }

        StartCoroutine(Fill());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Fill()
    {
        bool needsRefill = true;
        while (needsRefill)
        {
            yield return new WaitForSeconds(fillTime);
            while (FillStep())
            {
                //inverse = !inverse;
                yield return new WaitForSeconds(fillTime);
            }

            needsRefill = ClearAllValidMatches ();
        }
    }

    public bool FillStep()
    {
        bool movedPiece = false;

        for (int y = yDim-2; y >=0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMoveable())
                {
                    GamePiece pieceBelow = pieces[x, y + 1];

                    if (pieceBelow.Type ==  PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MoveableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MoveableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
                movedPiece = true;
            }
        }
        return movedPiece;
    }


   public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x,
            transform.position.y + yDim / 2.0f - y);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }

    public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
    {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
            || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }

    public void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (gameOver)
        {
            return;
        }
        if (piece1.IsMoveable () && piece2.IsMoveable())
        {
            pieces [piece1.X, piece1.Y] = piece2;
            pieces[piece2.X, piece2.Y] = piece1;

            if (GetMatch(piece1, piece2.X, piece2.Y) != null || GetMatch(piece2, piece1.X, piece1.Y) != null)
            {

                int piece1X = piece1.X;
                int piece1Y = piece1.Y;

                piece1.MoveableComponent.Move(piece2.X, piece2.Y, fillTime);
                piece2.MoveableComponent.Move(piece1X, piece1Y, fillTime);

                ClearAllValidMatches ();

                StartCoroutine(Fill ());

                level.OnMove();
            }
            else
            {
                pieces[piece1.X, piece1.Y] = piece1;
                pieces[piece2.X, piece2.Y] = piece2;
            }
        }
    }

    public void PressPiece(GamePiece piece)
    {
        pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece)
    {
        enteredPiece = piece;
    }

    public void ReleasePiece()
    {
        if (IsAdjacent (pressedPiece, enteredPiece))
        {
            SwapPieces (pressedPiece,enteredPiece);
        }
    }

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
    {
        if (piece.IsColored())
        {
            ColorPiece.ColorType color = piece.ColorComponent.Color;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();
            List<GamePiece> matchingPieces = new List<GamePiece>();

            horizontalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int xOffset = 1; xOffset < xDim; xOffset++)
                {
                    int x;

                    if (dir == 0)
                    {
                        x = newX - xOffset;
                    }
                    else
                    {
                        x = newX + xOffset;
                    }
                    if (x < 0 || x >= xDim)
                    {
                        break;
                    }

                    if(pieces [x,newY].IsColored () && pieces[x, newY].ColorComponent.Color == color)
                    {
                        horizontalPieces.Add(pieces[x, newY]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3)
            {
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    matchingPieces.Add (horizontalPieces [i]);
                }
            }

            if (horizontalPieces.Count >= 3)
            {
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    for (int dir = 0; dir <= 1; dir++)
                    {
                        for (int yOffset = 1; yOffset < yDim; yOffset++)
                        {
                            int y;

                            if (dir == 0)
                            {
                                y = newY - yOffset;
                            }

                            else
                            {
                                y = newY + yOffset;
                            }

                            if (y < 0 || y >= yDim)
                            {
                                break;
                            }
                            if (pieces[horizontalPieces[i].X,y].IsColored() && pieces [horizontalPieces[i].X,y].ColorComponent.Color == color)
                            {
                                verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (verticalPieces.Count <2)
                    {
                        verticalPieces.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < verticalPieces.Count; j++)
                        {
                            matchingPieces.Add(verticalPieces[j]);
                        }
                        break;
                    }
                }
            }

        if (matchingPieces.Count >= 3)
        {
                return matchingPieces;
        }
            //look vertically now
            horizontalPieces.Clear();
            verticalPieces.Clear();
            verticalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int yOffset = 1; yOffset < yDim; yOffset++)
                {
                    int y;

                    if (dir == 0)
                    {
                        y = newY - yOffset;
                    }
                    else
                    {
                        y = newY +yOffset;
                    }
                    if (y < 0 || y >= yDim)
                    {
                        break;
                    }

                    if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
                    {
                        verticalPieces.Add(pieces[newX, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    matchingPieces.Add(verticalPieces[i]);
                }
            }

            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    for (int dir = 0; dir <= 1; dir++)
                    {
                        for (int xOffset = 1; xOffset < xDim; xOffset++)
                        {
                            int x;

                            if (dir == 0)
                            {
                                x = newX - xOffset;
                            }

                            else
                            {
                                x = newX + xOffset;
                            }

                            if (x < 0 || x >= xDim)
                            {
                                break;
                            }
                            if (pieces[x, verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
                            {
                                verticalPieces.Add(pieces[x, verticalPieces[i].Y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (horizontalPieces.Count < 2)
                    {
                        horizontalPieces.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < horizontalPieces.Count; j++)
                        {
                            matchingPieces.Add(horizontalPieces[j]);
                        }
                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3)
            {
                return matchingPieces;
            }

        }

        return null;
    }

    public bool ClearAllValidMatches()
    {
        bool needsRefill = false;

        for (int y = 0; y < yDim; y++)
        {
            for (int x = 0; x < xDim; x++)
            {
                if (pieces[x, y].IsClearable())
                {
                    List<GamePiece> match = GetMatch(pieces[x, y], x, y);

                    if (match != null)
                    {
                        for (int i = 0; i < match.Count; i++)
                        {
                            if (ClearPiece (match [i].X, match [i].Y))
                            {
                                needsRefill = true;
                            }
                        }
                    }
                }
            }
        }

        return needsRefill;
    }

    public bool ClearPiece(int x, int y)
    {
        if (pieces [x,y].IsClearable () && !pieces[x, y].ClearableComponent.IsBeingCleared)
        {
            pieces[x, y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);

            return true;
        }

        return false;
    }
    public void GameOver()
    {
        gameOver = true;
    }
}
