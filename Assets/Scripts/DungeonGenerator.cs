using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour {

    public class Cell{

        public bool visited = false;
        public bool[] status = new bool[4];
        public bool voidSpace = false;
        public bool hasPortal = false;
        public bool hasShop = false;
        public bool spawnersActive = false;
    }

    public Vector2 size;
    [Header("Check if you want to start in the middle of the grid, else it will start at the given position")]
    public bool startAtCentre = false;
    public int startPos = 0;
    [Header("Void Spaces are based on the size of the grid, but if multiplied by this factor can\nbe altered to create more or less. By default 20% of cells will be blank.")]
    public float blankCellFactor = 1.0f;

    public GameObject[] rooms;
    public Vector2 offset;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {      
        MapGenerator();
    }


    public void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j <size.y; j++)
            {
                if (!board[i + Mathf.FloorToInt(j * size.y)].voidSpace)
                {
                    var newRoom = Instantiate(rooms[GetRoomType(board[Mathf.FloorToInt(i + j * size.x)].status)], new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    if (board[i + Mathf.FloorToInt(j * size.y)].hasShop)
                    {
                        newRoom.CreateShop();
                    }
                    if (board[i + Mathf.FloorToInt(j * size.y)].hasPortal)
                    {
                        newRoom.CreatePortal();
                    }
                    if(Random.Range(0, 100) >= 50 && !board[i + Mathf.FloorToInt(j * size.y)].hasPortal && !board[i + Mathf.FloorToInt(j * size.y)].hasShop)
                    {
                        for(int k = 0; k < newRoom.spawners.Length; k++)
                        {
                            newRoom.spawners[k].GetComponent<Spawner>().spawningAllowed = true;
                        }
                    } else
                    {
                        for (int k = 0; k < newRoom.spawners.Length; k++)
                        {
                            newRoom.spawners[k].GetComponent<Spawner>().spawningAllowed = false;
                        }
                    }
                    newRoom.UpdateRotation(board[Mathf.FloorToInt(i + j * size.x)].status);
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    int GetRoomType(bool[] direction)
    {
        int openings = 0;
        // get the amount of openings the current cell has
        for(int i = 0; i < direction.Length; i++)
        {
            if (direction[i])
            {
                openings++;
            }
        }
        // use the openings to narrow it down
        // only 1 room has 4 openings
        if (openings == 4)
        {
            return 1;
        }
        // only 1 room has 3 openings
        else if (openings == 3)
        {
            return 5;
        }
        // only 1 room has 1 opening
        else if (openings == 1)
        {
            return 2;
        } 
        // 2 rooms have 2 openings
        else if(openings == 2)
        {
            // if the openings are up and down and not right and left or they are right and left and not up and down
            if ((direction[0] && direction[1] && !direction[2] && !direction[3]) || (!direction[0] && !direction[1] && direction[2] && direction[3]))
            {
                return 3;
            } else
            {
                return 4;
            }
        } else
        {
            Debug.Log("No room exists within these parameters");
            return 0;
        }

    }

    void PlantBlanks()
    {
        int currentBlank;
        var blanksToGenerate = Mathf.FloorToInt(((size.x * size.y) * 0.2f) * blankCellFactor);
        for(int i = 0; i < blanksToGenerate; i++)
        {
            
            currentBlank = Random.Range(0, Mathf.FloorToInt(size.x)) + (Random.Range(0, Mathf.FloorToInt(size.y)) * Mathf.FloorToInt(size.x));
            if (currentBlank != startPos && currentBlank != Mathf.FloorToInt(size.x / 2) + Mathf.FloorToInt(size.y / 2) * Mathf.FloorToInt(size.x))
            {
                //Debug.Log("Void Placed");
                board[currentBlank].visited = true;
                board[currentBlank].voidSpace = true;
                for (int j = 0; j < 3; j++)
                {
                    board[currentBlank].status[j] = false;
                }
            }
        }
    }

    void MapGenerator()
    {
        board = new List<Cell>();
        board.Capacity = Mathf.FloorToInt(size.x * size.y);

        for( int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)

            {
                board.Add(new Cell());
            }
        }
        PlantBlanks();
        // if startAtCentre is selected, find the center of the grid, if not start at startPos
        int currentCell;
        if (startAtCentre)
        {
            currentCell = Mathf.FloorToInt(size.x / 2) + Mathf.FloorToInt(size.y / 2) * Mathf.FloorToInt(size.x);
        } else
        {
            currentCell = startPos;
        }

        Stack<int> path = new Stack<int>();

        int k = 0;

        while(k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            // CheckNeighbors(board[currentCell]);
            
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if(newCell > currentCell)
                {
                    if(newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        if(GetOpenings(board[0].status) == 0)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        PlacePortal();
        PlaceShop();
        GenerateDungeon();
    }

    public List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        //check up
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell-size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        //check down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        //check right
        if ((cell+1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }
        //check left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }

    int GetOpenings(bool[] direction)
    {
        int openings = 0;
        // get the amount of openings the current cell has
        for (int i = 0; i < direction.Length; i++)
        {
            if (direction[i])
            {
                openings++;
            }
        }
        return openings;
    }
    void PlaceShop()
    {
        Stack<int> endRooms = new Stack<int>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (GetOpenings(board[Mathf.FloorToInt(i + j * size.x)].status) == 1 && !board[Mathf.FloorToInt(i + j * size.x)].hasPortal)
                {
                    endRooms.Push(Mathf.FloorToInt(i + j * size.x));
                }
            }
        }

        int popUntil = Random.Range(1, endRooms.Count);
        for(int k = endRooms.Count; k == popUntil; k--)
        {
            endRooms.Pop();
        }
        board[endRooms.Pop()].hasShop = true;
    }

    void PlacePortal()
    {
        Stack<int> endRooms = new Stack<int>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (GetOpenings(board[Mathf.FloorToInt(i + j * size.x)].status) == 1)
                {
                    endRooms.Push(Mathf.FloorToInt(i + j * size.x));
                }
            }
        }

        int popUntil = Random.Range(1, Mathf.FloorToInt(endRooms.Count * 0.8f));
        for (int k = endRooms.Count; k == popUntil; k--)
        {
            endRooms.Pop();
        }
        board[endRooms.Pop()].hasPortal = true;
    }
}
