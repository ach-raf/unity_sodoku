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
    private List<int> optionList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

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
    private void Update()
    {

    }

    private void CellClicked(CellScript cellClicked)
    {
        Debug.Log(string.Format("Cell Clicked: {0}", cellClicked.gameObject.name));
        cellClicked.SetTextField("X");
        cellClicked.GetGridObjectReference().NorthNeighbour().SetTextField("N");
        int _x = cellClicked.GetGridObjectReference().x;
        int _z = cellClicked.GetGridObjectReference().z;
        //Debug.Log(string.Format("Cell: {0}", gridSO.gridReference.GetGridObject(_x, _z).gameObject.name));

        //array of size 3x3, x, z is the notation if we have a list of arrays of size 3x3
        //and we want to create the soduku board, if we wanna know in which array we are, we can use the x and z values of the cell clicked
        // array 0 if x - 3 < 0, array 1 if x -3 = 0, array 2 if x - 3 > 0
        // array 0 if z - 3 < 0, array 1 if z -3 = 0, array 2 if z - 3 > 0
    }

    public void BoardIsReady(GridSystemSO _gridSO)
    {
        gridSO = _gridSO;
    }


}
