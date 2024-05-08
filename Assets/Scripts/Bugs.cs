using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bugs : MonoBehaviour {

    //20 MEANS OUT OF THE BOARD
    int OffBoard = 20;
    Vector2 OffBoardPos;
    //FOR AN ASPECT RATIO OF 16:10
    float refCols = 16, refRows = 10;

    public int[,] CreateGeneration;
    Vector2[,] NewGeneration;
    [SerializeField]private int columns = 10, rows = 16;
    private Vector2[,] board;
    Camera cam;
    float timer1 = 0;
    public GameObject[,] bugsArray;
    [SerializeField]private GameObject bug;
    SpriteRenderer bugSprite;

	// Use this for initialization
    private void Awake() {
        cam = Camera.main;
        bugSprite = bug.GetComponent<SpriteRenderer>();
    }

	void Start () {
        board = new Vector2[rows, columns];
        CreateBoard(rows, columns);

        bugsArray = new GameObject[rows, columns];
        CreateGeneration = new int[rows, columns];
        NewGeneration = new Vector2[rows, columns];
        OffBoardPos = new Vector2(OffBoard, OffBoard);
        CreateBugs(rows, columns);
        SetInitialBugs(rows, columns);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer1 += Time.deltaTime;
        if (timer1 >= 0.1f) {
            CheckDeathOrAlive(rows, columns);
            GenerationToAppear(rows, columns);
            timer1 = 0;
        }
    }


    void CreateBoard(int m, int n) {
        
        Vector2 initialPos;
        // If the number of rows is higher than 10 and columns are higher than 16 rescale the bug.
        // then initialize in the top left corner -8, 5.
        if(m >= refRows && n >= refCols) {
            ResizeBug(m, n);
            initialPos = new Vector2(-8, 5);
        } else {
            initialPos = new Vector2(-n / 2f, m / 2f);
        }
        Vector2 newPos = new Vector2(initialPos.x, initialPos.y);
        Vector2 delta = new Vector2(bugSprite.bounds.size.x, bugSprite.bounds.size.y);
        for (int i = 0; i < m ; i++) {
            for (int j = 0; j < n; j++) {
                board[i, j] = new Vector2(newPos.x, newPos.y);
                newPos.x += delta.x;
            }
            newPos.x = initialPos.x;
            newPos.y -=  delta.y;
        }
    }

    void ResizeBug(int m, int n) {
        bug.transform.localScale = new Vector2( ((float)refCols) / n, ((float)refRows) / m);
    }

    void CreateBugs(int m, int n) {
        int bugIndex = 0;
        for (int i = 0; i < m ; i++) {
            for (int j = 0; j < n; j++) {
                bugsArray[i, j] = Instantiate(bug, new Vector3(OffBoard, OffBoard, 0), Quaternion.identity);
                bugsArray[i, j].name = bugsArray[i, j].name.Replace("(Clone)", "") + (bugIndex++);
            }
        }
    }

    void SetInitialBugs(int m, int n) {
        float randomValue;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                randomValue = Random.Range(0f, 5f);
                if (randomValue < 1) {
                    bugsArray[i, j].transform.position = board[i, j];
                }

            }
        }
    }

    void CheckDeathOrAlive(int m, int n)
    {
        int counterBugs = 0;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (bugsArray[i, j].transform.position.x == OffBoard) //Detect if there is a space
                {
                    //CreateGeneration[i, j] = 0;
                    NewGeneration[i,j] = OffBoardPos;
                    counterBugs = CheckIfThereIsABug(i, j, counterBugs);
                    if (counterBugs == 3) /*CreateGeneration[i,j] = 2*/ NewGeneration[i, j] = board[i, j]; //creates a Bug
                    counterBugs = 0;
                    continue;
                }
                if (bugsArray[i, j].transform.position.x != OffBoard) //Detect if there is an entity
                {
                    //CreateGeneration[i, j] = 2;
                    NewGeneration[i,j] = board[i, j];
                    counterBugs = CheckIfThereIsABug(i, j, counterBugs);
                    if (counterBugs < 2 || counterBugs > 3) /*CreateGeneration[i, j] = 0*/ NewGeneration[i,j] = OffBoardPos; //kill the entity
                    counterBugs = 0;
                    continue;
                }
            }
        }
        return;
    }

    void GenerationToAppear(int m, int n) {
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                bugsArray[i, j].transform.position = NewGeneration[i, j];
            }
        }
    }

    int CheckIfThereIsABug(int i, int j, int counterBugs) {
        bool TopLimit = (i - 1) != -1;
        bool LeftLimit = (j - 1) != -1;
        bool RightLimit = (j + 1) != columns;
        bool BottomLimit = (i + 1) != rows;
        
        bool TopLeftLimit = TopLimit && LeftLimit;
        bool TopRightLimit = TopLimit && RightLimit;
        bool BottomLeftLimit = BottomLimit && LeftLimit;
        bool BottomRightLimit = BottomLimit && RightLimit;

        //TopLeft
        if (TopLeftLimit && bugsArray[i - 1, j - 1].transform.position.x != OffBoard) ++counterBugs;
        //Top
        if (TopLimit && bugsArray[i - 1, j].transform.position.x != OffBoard) ++counterBugs;
        //TopRight
        if (TopRightLimit && bugsArray[i - 1, j + 1].transform.position.x != OffBoard) ++counterBugs;
        //Left
        if (LeftLimit && bugsArray[i, j - 1].transform.position.x != OffBoard) ++counterBugs;
        //Right
        if (RightLimit && bugsArray[i, j + 1].transform.position.x != OffBoard) ++counterBugs;
        //BottomLeft
        if (BottomLeftLimit && bugsArray[i + 1, j - 1].transform.position.x != OffBoard) ++counterBugs;
        //Bottom
        if (BottomLimit && bugsArray[i + 1, j].transform.position.x != OffBoard) ++counterBugs;
        //BottomRight
        if (BottomRightLimit && bugsArray[i + 1, j + 1].transform.position.x != OffBoard) ++counterBugs;

        return counterBugs;
    }
}
