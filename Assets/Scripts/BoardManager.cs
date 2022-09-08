using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class BoardManager : MonoBehaviour
{

    #region Properties
    private GridSystemSO gridSO;
    private TextMeshProUGUI textField;
    private List<int> optionList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private List<int> openList;
    private List<int> closedList;
    private List<int> boardCounter;
    private System.Random random = new System.Random();

    private GridObjectSO previousCellClicked;
    private int backtrackCounter = 0;

    private Task autoFillBoard = null;

    private ScreenshotHandler screenShotHandler;

    public void SetGridSystem(GridSystemSO _gridSO)
    {
        this.gridSO = _gridSO;
    }

    public GridSystemSO GetGridSystem()
    {
        return gridSO;
    }

    #endregion

    #region OnEnable/Disable
    private void OnEnable()
    {
        EventManager.cellClicked += CellClicked;
        EventManager.boardIsReady += BoardIsReady;
    }

    private void OnDisable()
    {
        EventManager.cellClicked -= CellClicked;
        EventManager.boardIsReady -= BoardIsReady;
    }

    #endregion

    private void Awake()
    {


        screenShotHandler = this.GetComponent<ScreenshotHandler>();
    }
    private void Start()
    {
        autoFillBoard = new Task(AutoFillBoard(TakeScreenshot));
    }
    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("Assets/IconGenerator/Icons/soduku.png");
    }

    private void CellClicked(CellScript cellClicked)
    {
        //GridObjectSO gridObjectReference = cellClicked.GetGridObjectReference();
        //previousCellClicked = gridObjectReference;

        //StartCoroutine(AutoFillBoard());



    }

    public void BoardIsReady(GridSystemSO _gridSO)
    {
        gridSO = _gridSO;
        previousCellClicked = gridSO.gridReference.GetGridObject(0, 0);
    }

    public void BackTrack(GridObjectSO cellClicked)
    {
        //backtrack algorithm for the sudoku board

        backtrackCounter++;
        if (backtrackCounter > 100)
        {
            Debug.Log("Backtrack counter is over 100");
            if (backtrackCounter % 2 == 0)
            {
                if (cellClicked.EastNeighbour())
                {
                    cellClicked = cellClicked.EastNeighbour();
                }

            }
            else
            {
                if (cellClicked.WestNeighbour())
                {
                    cellClicked = cellClicked.WestNeighbour();

                }
            }
            /*if (cellClicked.EastNeighbour())
            {
                cellClicked = cellClicked.EastNeighbour();
            }
            else if (cellClicked.WestNeighbour())
            {
                cellClicked = cellClicked.WestNeighbour();

            }*/


        }

        int _x = cellClicked.x;
        int _z = cellClicked.z;
        int _y = cellClicked.y;
        int _cellSize = cellClicked.cellSize;
        int _cellValue = cellClicked.cellValue;
        int _gridWidth = gridSO.gridReference.GetWidth();
        int _gridHeight = gridSO.gridReference.GetHeight();


        for (int i = _x; i < _gridWidth; i++)
        {
            for (int j = _y; j < _gridHeight; j++)
            {
                gridSO.gridReference.GetGridObject(i, j).cellValue = 0;
                StartCoroutine(SetTextField(cellClicked, 0));

            }
        }

    }




    public IEnumerator AutoFillBoard(Action action, int counter = 0)
    {
        bool result = false;
        for (int i = 0; i < gridSO.gridReference.GetWidth(); i++)
        {
            for (int j = 0; j < gridSO.gridReference.GetWidth(); j++)
            {
                yield return new WaitForSeconds(0.1f);
                GridObjectSO cellClicked = gridSO.gridReference.GetGridObject(i, j);
                if (cellClicked.cellValue == 0)
                {
                    openList = new List<int>(optionList);
                    closedList = new List<int>();
                    result = GameLogic(cellClicked);
                    if (result == false)
                    {
                        BackTrack(cellClicked);
                        counter++;
                        if (counter > 1000)
                        {
                            break;
                        }
                        Task autoFillBoard = new Task(AutoFillBoard(action, counter));
                        //StartCoroutine(AutoFillBoard(counter));

                    }
                }

            }
        }
        action();


    }


    public bool IsValid(GridObjectSO cellClicked, int value)
    {
        bool northLogic = NorthLogic(cellClicked, value);
        bool southLogic = SouthLogic(cellClicked, value);
        bool eastLogic = EastLogic(cellClicked, value);
        bool westLogic = WestLogic(cellClicked, value);
        bool localArrayLogic = LocalArrayLogic(cellClicked, value);

        if (localArrayLogic && northLogic && southLogic && eastLogic && westLogic)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SetTextField(GridObjectSO cellClicked, int value)
    {
        yield return new WaitForSeconds(0.1f);

        if (value == 0)
        {
            cellClicked.cellValue = 0;
            cellClicked.GetCellScript().SetTextField("X");
            cellClicked.GetCellScript().SetTextColor(Color.red);
        }
        else
        {
            cellClicked.cellValue = value;
            cellClicked.GetCellScript().SetTextField(value.ToString());
            cellClicked.GetCellScript().SetTextColor(Color.red);
        }
        yield return new WaitForSeconds(0.1f);
        if (cellClicked != previousCellClicked)
        {
            previousCellClicked.GetCellScript().SetTextColor(Color.white);
            previousCellClicked = cellClicked;
        }

    }

    public bool GameLogic(GridObjectSO cellClicked)
    {
        int value = 0;
        bool isValid = false;
        int counter = 0;
        int index = 0;
        while (!isValid)
        {
            if (counter > 20)
            {
                return false;
            }
            index = random.Next(openList.Count);
            value = openList[index];
            isValid = IsValid(cellClicked, value);
            counter++;
        }
        if (isValid)
        {
            cellClicked.cellValue = value;
            StartCoroutine(SetTextField(cellClicked, value));
            closedList.Add(value);
            openList.Remove(value);
            return true;
        }
        return isValid;
    }

    public bool LocalArrayLogic(GridObjectSO _startingCell, int _value)
    {
        GridObjectSO gridObject;
        int _x = _startingCell.x;
        int _z = _startingCell.z;
        int xLoopStart = 0;
        int xLoopEnd = 0;
        int zLoopStart = 0;
        int zLoopEnd = 0;


        if (_x < 3)
        {
            xLoopStart = 0;
            xLoopEnd = 3;
            if (_z < 3)
            {
                //array 0
                zLoopStart = 0;
                zLoopEnd = 3;


            }
            else if (_z < 6)
            {
                //array 1
                zLoopStart = 3;
                zLoopEnd = 6;
            }
            else
            {
                //array 2
                zLoopStart = 6;
                zLoopEnd = 9;
            }
        }
        else if (_x < 6)
        {
            xLoopStart = 3;
            xLoopEnd = 6;
            if (_z < 3)
            {
                //array 3
                zLoopStart = 0;
                zLoopEnd = 3;
            }
            else if (_z < 6)
            {
                //array 4
                zLoopStart = 3;
                zLoopEnd = 6;
            }
            else
            {
                //array 5
                zLoopStart = 6;
                zLoopEnd = 9;
            }
        }
        else
        {
            xLoopStart = 6;
            xLoopEnd = 9;
            if (_z < 3)
            {
                //array 6
                zLoopStart = 0;
                zLoopEnd = 3;
            }
            else if (_z < 6)
            {
                //array 7
                zLoopStart = 3;
                zLoopEnd = 6;
            }
            else
            {
                //array 8
                zLoopStart = 6;
                zLoopEnd = 9;
            }
        }
        for (int i = xLoopStart; i < xLoopEnd; i++)
        {
            for (int j = zLoopStart; j < zLoopEnd; j++)
            {
                gridObject = gridSO.gridReference.GetGridObject(i, j);
                if (gridObject.cellValue != _value)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;

    }

    public bool NorthLogic(GridObjectSO _startingCell, int _value)
    {
        // return true if you can post the value false if not
        GridObjectSO _currentCell = _startingCell;
        GridObjectSO _nextCell = _currentCell.NorthNeighbour();
        while (_nextCell != null)
        {
            if (_nextCell.cellValue != _value)
            {
                //_nextCell.gameObject.GetComponent<CellScript>().SetTextField("N");
                _currentCell = _nextCell;
                _nextCell = _currentCell.NorthNeighbour();
            }
            else
            {
                //openList.RemoveAll(x => x == _value);
                return false;
            }

        }
        return true;
    }

    public bool SouthLogic(GridObjectSO _startingCell, int _value)
    {
        GridObjectSO _currentCell = _startingCell;
        GridObjectSO _nextCell = _currentCell.SouthNeighbour();
        while (_nextCell != null)
        {
            if (_nextCell.cellValue != _value)
            {
                //_nextCell.gameObject.GetComponent<CellScript>().SetTextField("S");
                _currentCell = _nextCell;
                _nextCell = _currentCell.SouthNeighbour();
            }
            else
            {
                //openList.RemoveAll(x => x == _value);
                return false;

            }
        }
        return true;

    }

    public bool EastLogic(GridObjectSO _startingCell, int _value)
    {
        GridObjectSO _currentCell = _startingCell;
        GridObjectSO _nextCell = _currentCell.EastNeighbour();
        while (_nextCell != null)
        {
            if (_nextCell.cellValue != _value)
            {
                //_nextCell.gameObject.GetComponent<CellScript>().SetTextField("E");
                _currentCell = _nextCell;
                _nextCell = _currentCell.EastNeighbour();
            }
            else
            {
                //openList.RemoveAll(x => x == _value);
                return false;
            }
        }
        return true;
    }

    public bool WestLogic(GridObjectSO _startingCell, int _value)
    {
        GridObjectSO _currentCell = _startingCell;
        GridObjectSO _nextCell = _currentCell.WestNeighbour();
        while (_nextCell != null)
        {
            if (_nextCell.cellValue != _value)
            {
                //_nextCell.gameObject.GetComponent<CellScript>().SetTextField("W");
                _currentCell = _nextCell;
                _nextCell = _currentCell.WestNeighbour();
            }
            else
            {
                //openList.RemoveAll(x => x == _value);
                return false;
            }
        }
        return true;
    }




}
