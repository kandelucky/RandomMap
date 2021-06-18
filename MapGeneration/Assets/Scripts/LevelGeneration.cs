using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    #region Comentarebi
    //enum to int
    // AmmoType theEnum = AmmoType.End;
    // int toInteger = (int)theEnum;
    // AmmoType andBackAgain = (AmmoType)toInteger;
    #endregion 
    bool firstMap;
    bool savedGameIsStarted;
    public bool stopGeneration = true;

    public enum Directions { NoDirection, LeftDirection, RightDirection };
    Directions direction;
    // Positions
    enum Positions { minLeft, minRight, left, right, maxLeft, maxRight };
    Positions position;
    public Transform[] roadsPositions;
    // Roads
    enum Roads { Left = 0, Right = 1, LeftUp = 2, RightUp = 3, Up = 4, UpLeft = 5, UpRight = 6 };
    Roads road;
    bool upDirRoad; // for detect more than 1 up direction
    bool HorDirRoad; // for detect more than 1 horisontal direction
    public GameObject[] roadsObj;
    

    //---------------
    public GameObject[] freeSpaces;
    //-----------

    #region Mini Map Values

    
    string arrayToSaveStr;
    Directions startDir = Directions.NoDirection;
    int rows = 14;
    int cols = 15;
    public int[,] miniMapArray = new int[14, 15];
    int arrayRowValue = 0;
    int arrayColValue = 6;
    public bool newMiniMap = false;
    public int day = 0;

    #endregion


    private void Start()
    {
        MapAuthorization();
    }

    void MapAuthorization()
    {

        LoadMapValues();
        SetVariables();
        if (savedGameIsStarted)
        {
            //ToDo Load Saved Map
        }
        else
        {
            CreateMap();
        }
    }
    void LoadMapValues()
    {
        
    }
    void SetVariables()
    {
        firstMap = stopGeneration = newMiniMap = true;
        upDirRoad = HorDirRoad = false;
    }

    void RandomBottomUpRoads( Positions pos)
    {
        int[] upRoads;
        switch (pos)
        {
            case Positions.minLeft:
                upRoads = new int[] { 4, 4, 4, 5, 6, 6, 6 }; // 4 = Up, 5 = UpLeft, 6 = UpRight. // Most likely Up or UpRight for long road in the map
                upRoads.Shuffle(7);
                road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                break;
            case Positions.minRight:
                upRoads = new int[] { 4, 4, 4, 5, 5, 5, 6 }; // 4 = Up, 5 = UpLeft, 6 = UpRight.
                upRoads.Shuffle(7);
                road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                break;
        }
        if (road == Roads.Up) upDirRoad = true;
    }

    public void RandomUpRoads(Directions dir) // Roads = UP, UpLeft, UpRight
    {
        int[] upRoads;
        switch (dir)
        {
            case Directions.NoDirection:
                if (!upDirRoad)
                {
                    upRoads = new int[] { 4, 5, 6 }; // 4 = Up, 5 = UpLeft, etc.
                    upRoads.Shuffle(3);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                else
                {
                    upRoads = new int[] { 4, 5, 5, 5, 6, 6, 6 }; // 4 = Up, 5 = UpLeft, etc.
                    upRoads.Shuffle(7);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                break;
            case Directions.LeftDirection:
                if (!upDirRoad)
                {
                    upRoads = new int[] { 4, 5 };
                    upRoads.Shuffle(2);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                else
                {
                    upRoads = new int[] { 4, 5, 5, 5 };
                    upRoads.Shuffle(4);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                
                break;
            case Directions.RightDirection:
                if (!upDirRoad)
                {
                    upRoads = new int[] { 4, 6 };
                    upRoads.Shuffle(2);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                else
                {
                    upRoads = new int[] { 4, 6, 6, 6 };
                    upRoads.Shuffle(4);
                    road = (Roads)upRoads[UnityEngine.Random.Range(0, upRoads.Length)];
                }
                break;
        }
        if (road != Roads.Up) upDirRoad = false;
    }
    public void RandomLeftRoads()
    {
        int[] leftRoads = new int[] { 0, 2 };
        leftRoads.Shuffle(2);
        road = (Roads)leftRoads[UnityEngine.Random.Range(0, leftRoads.Length)];
        if (upDirRoad) upDirRoad = false;
    }
    public void RandomRightRoads()
    {
        int[] rightRoads = new int[] { 1, 3 };
        rightRoads.Shuffle(2);
        road = (Roads)rightRoads[UnityEngine.Random.Range(0, rightRoads.Length)];
        if (upDirRoad) upDirRoad = false;
    }
    public void CreateMap()
    {
        for (int i = 0; i < roadsPositions.Length; i++)
        {
            if (roadsPositions[i].childCount > 0) Destroy(roadsPositions[i].GetChild(0).gameObject);

        }
        stopGeneration = false;
        MapGeneration();
        day++;
    }
    public void MapGeneration()
    {
        if (firstMap) // if map created first time
        {
            bool leftPos = UnityEngine.Random.value > 0.5f;
            position = leftPos ? Positions.minLeft : Positions.minRight; // if (leftPos = true) position = MinLeft; else MinRight
            RandomBottomUpRoads(position);
            CreateRoad();
        }
        else
        {
            CreateRoad();
        }
    }
    void CreateRoad()
    {
        GameObject instance = Instantiate(roadsObj[(int)road], roadsPositions[(int)position].transform.position, Quaternion.identity);
        instance.transform.parent = roadsPositions[(int)position].transform;
        NextPositionAndRoad();// check to next position, next road
    }
    void NextPositionAndRoad()
    {
        if (road == Roads.LeftUp || road == Roads.RightUp || road == Roads.Up) // road ends at top
        {
            switch (position) // set next position
            {
                case Positions.minLeft:
                    position = Positions.left;
                    RandomUpRoads(direction);
                    break;
                case Positions.minRight:
                    position = Positions.right;
                    RandomUpRoads(direction);
                    break;
                case Positions.left:
                    position = Positions.maxLeft;
                    RandomUpRoads(direction);
                    break;
                case Positions.right:
                    position = Positions.maxRight;
                    RandomUpRoads(direction);
                    break;
                case Positions.maxLeft: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.minLeft;
                    direction = Directions.NoDirection; // next map in the UP; no directions check needed
                    RandomBottomUpRoads(position);
                    //RandomUpRoads(direction);
                    break;
                case Positions.maxRight: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.minRight;
                    direction = Directions.NoDirection; // next map in the UP; no directions check needed
                    RandomBottomUpRoads(position);
                    //RandomUpRoads(direction);
                    break;
            }
        }
        else if (road == Roads.Left || road == Roads.UpLeft) // road ends at left
        {
            switch (position)
            {
                case Positions.minLeft: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.minRight;
                    direction = Directions.LeftDirection;
                    break;
                case Positions.minRight:
                    position = Positions.minLeft;
                    break;
                case Positions.left: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.right;
                    direction = Directions.LeftDirection;
                    break;
                case Positions.right:
                    position = Positions.left;
                    break;
                case Positions.maxLeft: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.maxRight;
                    direction = Directions.LeftDirection;
                    break;
                case Positions.maxRight:
                    position = Positions.maxLeft;
                    break;
            }
            RandomLeftRoads();
        }
        else if (road == Roads.Right || road == Roads.UpRight) // road ends at right
        {
            switch (position)
            {
                case Positions.minLeft:
                    position = Positions.minRight;
                    break;
                case Positions.minRight: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.minLeft;
                    direction = Directions.RightDirection;
                    break;
                case Positions.left:
                    position = Positions.right;
                    break;
                case Positions.right: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.left;
                    direction = Directions.RightDirection;
                    break;
                case Positions.maxLeft:
                    position = Positions.maxRight;
                    break;
                case Positions.maxRight: // end of the map
                    stopGeneration = true;
                    if (firstMap) firstMap = false;
                    position = Positions.maxLeft;
                    direction = Directions.RightDirection;
                    break;
            }
            RandomRightRoads();
        }
        if (firstMap)
        {
            if (!stopGeneration)
            {
                firstMap = false; 
                MapGeneration();
            }
        }
        else
        {
            if (!stopGeneration) MapGeneration();
            else
            {
                FallEmptyes();
                CheckMiniMapDirection(direction);
            }
        }
    }
    void FallEmptyes()
    {
        StartCoroutine (FallEmptySpaces());
    }
    IEnumerator FallEmptySpaces()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < roadsPositions.Length; i++)
        {
            if (roadsPositions[i].childCount == 0)
            {
                GameObject instance = Instantiate(freeSpaces[UnityEngine.Random.Range(0, freeSpaces.Length)], roadsPositions[i].transform.position, Quaternion.identity);
                instance.transform.parent = roadsPositions[i].transform;
            }
        }

    }


    #region Mini Map
    enum MiniMapDirections { Down_Up, Up_Left, Up_Right, Left_Up, Left_Left, Right_Up, Right_Right };
    MiniMapDirections miniMapDirection;

    void CheckMiniMapDirection(Directions newDirection)
    {
        switch (startDir)
        {
            case Directions.NoDirection:
                switch (newDirection)
                {
                    case Directions.NoDirection:
                        miniMapDirection = MiniMapDirections.Down_Up;
                        break;
                    case Directions.LeftDirection:
                        miniMapDirection = MiniMapDirections.Up_Left;
                        break;
                    case Directions.RightDirection:
                        miniMapDirection = MiniMapDirections.Up_Right;
                        break;
                }
                break;
            case Directions.LeftDirection:
                switch (newDirection)
                {
                    case Directions.NoDirection:
                        miniMapDirection = MiniMapDirections.Left_Up;
                        break;
                    case Directions.LeftDirection:
                        miniMapDirection = MiniMapDirections.Left_Left;
                        break;
                }
                break;
            case Directions.RightDirection:
                switch (newDirection)
                {
                    case Directions.NoDirection:
                        miniMapDirection = MiniMapDirections.Right_Up;
                        break;
                    case Directions.RightDirection:
                        miniMapDirection = MiniMapDirections.Right_Right;
                        break;
                }
                break;
        }
        startDir = newDirection;
        SetMiniMapArrayAndCheckNextIndex(miniMapDirection);
    }
    void SetMiniMapArrayAndCheckNextIndex(MiniMapDirections miniMapDir)
    {
        miniMapArray[arrayRowValue, arrayColValue] = (int)miniMapDir + 1; // + 1 becouse if the array value not changed, in mini map will by created emtry sprite
        CheckNextIndexInArray(miniMapDir);
    }

    private void CheckNextIndexInArray(MiniMapDirections miniMapDir)
    {
        switch (miniMapDir)
        {
            case MiniMapDirections.Down_Up:
                arrayRowValue++;
                if (arrayRowValue == rows) AddArrayRow();
                break;
            case MiniMapDirections.Up_Left:
                arrayColValue--;
                if (arrayColValue == 0) AddArrayFirstCols();
                // ToDo if is less of 0 - add in first of array columns
                break;
            case MiniMapDirections.Up_Right:
                arrayColValue++;
                if (arrayColValue == cols) AddArrayEndCols();
                // ToDo if is grater of 13 - add in end of array columns
                break;
            case MiniMapDirections.Left_Up:
                arrayRowValue++;
                if (arrayRowValue == rows) AddArrayRow();
                break;
            case MiniMapDirections.Left_Left:
                arrayColValue--;
                if (arrayColValue == 0) AddArrayFirstCols();
                // ToDo if is less of 0 - add in first of array columns
                break;
            case MiniMapDirections.Right_Up:
                arrayRowValue++;
                if (arrayRowValue == rows) AddArrayRow();
                break;
            case MiniMapDirections.Right_Right:
                arrayColValue++;
                if (arrayColValue == cols) AddArrayEndCols();
                // ToDo if is grater of 13 - add in end of array columns
                break;
        }
        debugArray("@@@@@@");
    }

    void AddArrayRow()
    {
        newMiniMap = true;
        int[,] tempArray = (int[,])miniMapArray.Clone();
        miniMapArray = new int[arrayRowValue+1, cols];
        for (int row = 0; row < arrayRowValue; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                miniMapArray[row, col] = tempArray[row, col];
            }
        }
        rows++;
        debugArray("axali ^^^^^^^^ ");
    }
    void AddArrayFirstCols()
    {
        newMiniMap = true;
        int[,] tempArray = (int[,])miniMapArray.Clone();
        miniMapArray = new int[rows, cols+1];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                miniMapArray[row, col+1] = tempArray[row, col];
            }
        }
        cols++;
        arrayColValue++;
        debugArray("axali ------- ");
    }
    void AddArrayEndCols()
    {
        newMiniMap = true;
        int[,] tempArray = (int[,])miniMapArray.Clone();
        miniMapArray = new int[rows, cols + 1];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                miniMapArray[row, col] = tempArray[row, col];
            }
        }
        cols++;
        debugArray("axali ++++++++ ");
    }

    void debugArray(string tex)
    {
        string msg = "";
        Debug.Log("["+ miniMapArray.GetLength(0)+", "+ miniMapArray.GetLength(1)+"]");
        for (int j = 0; j < miniMapArray.GetLength(0); j++)
        {
            for (int i = 0; i < miniMapArray.GetLength(1); i++)
            {
                msg = msg + miniMapArray[j, i].ToString()+" ,";
                
            }
            Debug.Log(j+" - "+msg);
            
            msg = "";
        }
        msg = tex;
        Debug.Log(msg);
    }
        #endregion Mini Map
}
