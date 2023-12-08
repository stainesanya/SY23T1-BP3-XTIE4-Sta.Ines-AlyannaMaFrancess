using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Game : MonoBehaviour
    {
        [SerializeField] GameObject square;

        Square[,] grid = new Square[17, 15];
        Vector2Int snakeHeadPos = new Vector2Int(10, 8);

        List<Square> snakeObj = new List<Square>();
        Direction snakeDir = Direction.Up;

        Transform foodObj;
        Vector2Int foodPos = Vector2Int.zero;

        bool canChangeDir;

        void Start()
        {
            //Playing field 
            Vector2 pos = new Vector2(0.5f, 0.5f);
            bool alternateColor = true;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    GameObject _s = Instantiate(square, pos + new Vector2((1f * x), -(1f * y)), Quaternion.identity);
                    grid[x, y] = _s.GetComponent<Square>();

                    grid[x, y].color = alternateColor ? Color.cyan : Color.white;
                    grid[x, y].gridPos = new Vector2Int(x, y);
                    alternateColor = !alternateColor;
                }
            }

            //Snake head 
            snakeObj.Add(Instantiate(square, grid[snakeHeadPos.x, snakeHeadPos.y].transform.position, Quaternion.identity).GetComponent<Square>());
            snakeObj[0].GetComponent<Square>().gridPos = snakeHeadPos;

            //Snake bodies 
            for (int i = 1; i < 2; i++) //2 because (3 - 1) 
            {
                Vector2Int _pos = Vector2Int.zero;
                switch (snakeDir)
                {
                    case Direction.Left:
                        _pos.x += i;
                        break;
                    case Direction.Right:
                        _pos.x -= i;
                        break;
                    case Direction.Up:
                        _pos.y += i;
                        break;
                    case Direction.Down:
                        _pos.y -= i;
                        break;
                }

                Vector2Int _givePos = new Vector2Int(snakeHeadPos.x + _pos.x, snakeHeadPos.y + _pos.y);
                snakeObj.Add(Instantiate(square, grid[_givePos.x, _givePos.y].transform.position, Quaternion.identity).GetComponent<Square>());
                snakeObj[i].GetComponent<Square>().gridPos = _givePos;
            }

            //Food 
            foodObj = Instantiate(square, grid[2, 2].transform.position, Quaternion.identity).transform;
            foodObj.GetComponent<Square>().color = Color.yellow;
            foodPos = new Vector2Int(2, 2);

            StartCoroutine(MovingSequence());
        }

        IEnumerator MovingSequence()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                Move(); //at the same time, eating the food 

                //if you hit a snake body part 
                List<Square> _snakeBodyParts = new List<Square>(snakeObj);
                _snakeBodyParts.RemoveAt(0);

                if (_snakeBodyParts.FindIndex(_b => _b.gridPos == snakeHeadPos) != -1)
                {
                    Debug.LogError("You died!");
                    break;
                }
            }
        }

        void Move()
        {
            canChangeDir = true;
            grid[snakeHeadPos.x, snakeHeadPos.y].isOccupied = false;
            switch (snakeDir)
            {
                case Direction.Left:
                    snakeHeadPos.x--;
                    break;
                case Direction.Right:
                    snakeHeadPos.x++;
                    break;
                case Direction.Up:
                    snakeHeadPos.y--;
                    break;
                case Direction.Down:
                    snakeHeadPos.y++;
                    break;
            }

            Vector2Int _tipPos = -Vector2Int.one;
            for (int i = snakeObj.Count - 1; i > -1; i--)
            {
                if (i > 0) // not the head
                {
                    if (_tipPos == -Vector2Int.one)
                        _tipPos = snakeObj[i].GetComponent<Square>().gridPos;

                    Vector2Int _nextPos = snakeObj[i - 1].GetComponent<Square>().gridPos;
                    snakeObj[i].transform.position = snakeObj[i - 1].transform.position;
                    snakeObj[i].GetComponent<Square>().gridPos = snakeObj[i - 1].GetComponent<Square>().gridPos;
                    grid[_nextPos.x, _nextPos.y].isOccupied = true;
                }
                else
                {
                    snakeObj[i].transform.position = grid[snakeHeadPos.x, snakeHeadPos.y].transform.position;
                    snakeObj[i].GetComponent<Square>().gridPos = snakeHeadPos;
                    grid[snakeHeadPos.x, snakeHeadPos.y].isOccupied = true;
                }
            }

            // Vacate last tailtip cell 
            grid[_tipPos.x, _tipPos.y].isOccupied = false;

            //we've hit something..... 
            if (snakeHeadPos == foodPos)
            {
                //foodObj.gameObject.SetActive(false);
                snakeObj.Add(Instantiate(square, snakeObj[snakeObj.Count - 1].transform.position, Quaternion.identity).GetComponent<Square>());
                snakeObj[snakeObj.Count - 1].GetComponent<Square>().gridPos = snakeObj[snakeObj.Count - 2].GetComponent<Square>().gridPos;

                Vector2Int _newPos = snakeObj[snakeObj.Count - 2].GetComponent<Square>().gridPos;
                grid[_newPos.x, _newPos.y].isOccupied = true;

                //Relocate food randomly... 
                foodPos = SpawnFoodAwayFromSnake;
                foodObj.transform.position = grid[foodPos.x, foodPos.y].transform.position;
            }
        }

        Vector2Int SpawnFoodAwayFromSnake
        {
            get
            {
                List<Square> _VacantSquares = new List<Square>();
                for (int _y = 0; _y < grid.GetLength(1); _y++)
                {
                    for (int _x = 0; _x < grid.GetLength(0); _x++)
                    {
                        if (!grid[_x, _y].isOccupied)
                            _VacantSquares.Add(grid[_x, _y]);
                    }
                }

                return _VacantSquares[Random.Range(0, _VacantSquares.Count)].gridPos;
            }
        }

        void Update()
        {
            if (canChangeDir)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (snakeDir != Direction.Left)
                    {
                        canChangeDir = false;
                        snakeDir = Direction.Right;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (snakeDir != Direction.Right)
                    {
                        canChangeDir = false;
                        snakeDir = Direction.Left;
                    }
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (snakeDir != Direction.Down)
                    {
                        canChangeDir = false;
                        snakeDir = Direction.Up;
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (snakeDir != Direction.Up)
                    {
                        canChangeDir = false;
                        snakeDir = Direction.Down;
                    }
                }
            }
        }
    }
}
