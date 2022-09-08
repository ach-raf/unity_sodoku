using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataSO : ScriptableObject
{
    public GridSys<GameObject> ObjectsGrid;
    public int x;
    public int y;
    public int value;

    public int cellSize;

}
