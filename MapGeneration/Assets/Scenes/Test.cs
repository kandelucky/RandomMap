using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool firstMap;
    public enum Positions { minLeft, minRight, left, right, maxLeft, maxRight };
    public Positions position;
    enum Roads { Left = 0, Right = 1, LeftUp = 2, RightUp = 3, Up = 4, UpLeft = 5, UpRight = 6};
    Roads road;
    public enum Directions { NoDirection, LeftDirection, RightDirection};
    Directions direction;
    


    private void Start()
    {
        firstMap = true;
    }
    public void RandomUpRoads(Directions dir) // Roads = UP, UpLeft, UpRight
    {
        int[] upRoads;
        switch (dir)
        {
            case Directions.NoDirection:
                upRoads = new int[] { 4, 4, 5, 6 }; // 4 = Up, 5 = UpLeft, etc.
                road = (Roads)upRoads[Random.Range(0, upRoads.Length)];
                break;
            case Directions.LeftDirection:
                upRoads = new int[] { 4, 4, 5 };
                road = (Roads)upRoads[Random.Range(0, upRoads.Length)];
                break;
            case Directions.RightDirection:
                upRoads = new int[] { 4, 4, 6 };
                road = (Roads)upRoads[Random.Range(0, upRoads.Length)];
                break;
        }
        
    }
    public void RandomLeftRoads()
    {
        int[] leftRoads = new int[] { 0, 2, 2 };
        road = (Roads)leftRoads[Random.Range(0, leftRoads.Length)];
    }
    public void RandomRightRoads()
    {
        int[] rightRoads = new int[] { 1, 3, 3};
        road = (Roads)rightRoads[Random.Range(0, rightRoads.Length)];
    }
    public void MapGeneration()
    {
        if (firstMap) // if map created first time
        {
            bool leftPos = Random.value > 0.5f;
            position = leftPos ? Positions.minLeft : Positions.minRight; // if (leftPos = true) position = MinLeft; else MinRight
            RandomUpRoads(Directions.NoDirection);
            CreateRoad();
            firstMap  = false; // next road will created at new map and this will not be the first road
        }
        else
        {
            CreateRoad();
        }
    }
    void CreateRoad()
    {
        Debug.Log("Random Position is <" + position + ">, Road is <" + road + ">");
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
                    position = Positions.minLeft;
                    direction = Directions.NoDirection; // next map in the UP; no directions check needed
                    RandomUpRoads(direction);
                    break;
                case Positions.maxRight: // end of the map
                    position = Positions.minRight;
                    direction = Directions.NoDirection; // next map in the UP; no directions check needed
                    RandomUpRoads(direction);
                    break;
            }
        }
        else if (road == Roads.Left || road == Roads.UpLeft) // road ends at left
        {
            switch (position)
            {
                case Positions.minLeft: // end of the map
                    position = Positions.minRight;
                    direction = Directions.LeftDirection;
                    break;
                case Positions.minRight:
                    position = Positions.minLeft;
                    break;
                case Positions.left: // end of the map
                    position = Positions.right;
                    direction = Directions.LeftDirection;
                    break;
                case Positions.right:
                    position = Positions.left;
                    break;
                case Positions.maxLeft: // end of the map
                    position = Positions.maxRight;
                    direction = Directions.NoDirection;
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
                    position = Positions.minLeft;
                    direction = Directions.RightDirection;
                    break;
                case Positions.left:
                    position = Positions.right;
                    break;
                case Positions.right: // end of the map
                    position = Positions.left;
                    direction = Directions.RightDirection;
                    break;
                case Positions.maxLeft:
                    position = Positions.maxRight;
                    break;
                case Positions.maxRight: // end of the map
                    position = Positions.maxLeft;
                    direction = Directions.NoDirection;
                    break;
            }
            RandomRightRoads();
        }
        Debug.Log("NEXT Random Position is <" + position + ">, Road is <" + road + ">");
    }
}
