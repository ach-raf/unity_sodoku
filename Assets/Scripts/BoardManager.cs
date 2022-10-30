using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;


public class BoardManager : MonoBehaviour
{

    #region Properties
    private GridSystemSO gridSO;
    private TextMeshProUGUI textField;
    private List<int> optionList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private List<int> openList;
    private List<int> closedList;
    private System.Random random = new System.Random();

    private int backtrackCounter = 0;

    private ScreenshotHandler screenShotHandler;

    private int screenshotCounter = 0;

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

    }
    public void TakeScreenshot(int _screenshotCounter)

    {
        ScreenCapture.CaptureScreenshot(string.Format("Assets/IconGenerator/Icons/soduku{0}.png", _screenshotCounter));
    }

    private void CellClicked(CellScript cellClicked)
    {
        //AutoFillBoard();
        Solve();
    }

    public IEnumerator WaitForSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        screenshotCounter++;
        if (screenshotCounter < 5)
        {
            EventManager.OnStartProgram();
        }
    }

    public void BoardIsReady(GridSystemSO _gridSO)
    {
        gridSO = _gridSO;
        //CreateGameGridFromJson(1);
        //ReadJson(1);

        /*Solve();
        TakeScreenshot(screenshotCounter);
        SaveListToFile();
        StartCoroutine(WaitForSeconds(0.1f));*/
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
                GridObjectSO cellReference = gridSO.gridReference.GetGridObject(i, j);
                if (cellReference.modifiable)
                {
                    cellReference.cellValue = 0;
                    SetTextField(cellClicked, 0, Color.white);
                }

            }
        }

    }




    public void AutoFillBoard(int counter = 0)
    {
        bool result = false;
        for (int i = 0; i < gridSO.gridReference.GetWidth(); i++)
        {
            for (int j = 0; j < gridSO.gridReference.GetWidth(); j++)
            {

                GridObjectSO cellClicked = gridSO.gridReference.GetGridObject(i, j);
                if (cellClicked.modifiable == true && cellClicked.cellValue == 0)
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
                        AutoFillBoard(counter);

                    }
                }

            }
        }

    }


    public bool IsValidMove(GridObjectSO cellClicked, int value)
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

    public void SetTextField(GridObjectSO cellClicked, int value, Color color)
    {

        if (value == 0)
        {
            cellClicked.cellValue = 0;
            cellClicked.GetCellScript().SetTextField("");
        }
        else
        {
            cellClicked.cellValue = value;
            cellClicked.GetCellScript().SetTextField(value.ToString());
            cellClicked.GetCellScript().SetTextColor(color);
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
            isValid = IsValidMove(cellClicked, value);
            counter++;
        }
        if (isValid)
        {
            cellClicked.cellValue = value;
            SetTextField(cellClicked, value, Color.white);
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

    public void SaveListToFile(int _screenshotCounter)
    {
        string path = string.Format("{0}/IconGenerator/Icons/soduku{1}.json", Application.dataPath, _screenshotCounter);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        CellBasicInfo cellBasicInfo = new CellBasicInfo();
        List<string> lines = new List<string>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GridObjectSO gridObject = gridSO.gridReference.GetGridObject(i, j);
                if (gridObject)
                {
                    cellBasicInfo.x = i;
                    cellBasicInfo.z = j;
                    cellBasicInfo.value = gridObject.cellValue;
                    lines.Add(cellBasicInfo.ToString());
                }
            }
        }
        File.AppendAllLines(path, lines);

    }

    public void ReadJson(int _screenshotNumber)
    {
        string path = string.Format("{0}/IconGenerator/Icons/soduku{1}.json", Application.dataPath, _screenshotNumber);
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                CellBasicInfo cellBasicInfo = JsonUtility.FromJson<CellBasicInfo>(line);
                GridObjectSO gridObject = gridSO.gridReference.GetGridObject(cellBasicInfo.x, cellBasicInfo.z);
                if (gridObject)
                {
                    gridObject.x = cellBasicInfo.x;
                    gridObject.z = cellBasicInfo.z;
                    gridObject.cellValue = cellBasicInfo.value;
                    gridObject.modifiable = false;
                    SetTextField(gridObject, cellBasicInfo.value, Color.red);
                }
            }
        }


    }

    public GridObjectSO FindEmpty()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GridObjectSO gridObject = gridSO.gridReference.GetGridObject(i, j);
                if (gridObject && gridObject.modifiable && gridObject.cellValue == 0)
                {
                    return gridObject;
                }
            }
        }
        return null;
    }

    public bool Solve()
    {
        List<int> _choises = new List<int>(optionList);


        /*
        '''Solves the Sudoku board via the backtracking algorithm'''
        */
        GridObjectSO emptyGridObject = FindEmpty();
        if (emptyGridObject == null)
        {
            return true;
        }

        while (_choises.Count != 0)
        {
            int _index = random.Next(_choises.Count);
            int _value = _choises[_index];
            if (IsValidMove(emptyGridObject, _value))
            {
                emptyGridObject.cellValue = _value;
                _choises.Remove(_value);
                SetTextField(emptyGridObject, _value, Color.white);
                //backtracking here, leave the cell emptyGridObject empty for now if solve is false. 
                if (Solve())
                {
                    return true;
                }

                emptyGridObject.cellValue = 0;
                SetTextField(emptyGridObject, 0, Color.white);
            }
            _choises.Remove(_value);
        }
        return false;
    }

    public void PrintList(List<int> _list)
    {
        string _string = "";
        foreach (int i in _list)
        {
            _string += i.ToString() + " ";
        }
        Debug.Log(_string);
    }

    public void CreateGameGridFromJson(int _screenshotNumber)
    {
        string path = string.Format("{0}/IconGenerator/Icons/soduku{1}.json", Application.dataPath, _screenshotNumber);
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            List<string> tempList = new List<string>(lines);
            int _length = tempList.Count;
            for (int i = 0; i < _length - 26; i++)
            {
                int _index = random.Next(tempList.Count);
                tempList.RemoveAt(_index);
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.AppendAllLines(path, tempList.ToArray());
        }
    }


}



