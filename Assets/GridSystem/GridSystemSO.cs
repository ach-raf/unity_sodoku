using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GridSystem SO", menuName = "Grid/GridSystem SO", order = 0)]
public class GridSystemSO : ScriptableObject
{
    public GridSys<GridObjectSO> gridReference;
    public new string name;
    public int x; //width
    public int y = 0;
    public int z; //height or depth

    public float cellSize = 5;

    public Vector3 originPosition = new Vector3(0, 0, 0);
    //public BuildingScriptableObject[,] buildings_list;

    private void Awake()
    {
        //originPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
    }


}

