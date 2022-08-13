using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell SO", menuName = "Cell/Cell SO", order = 0)]
public class GridObjectSO : ScriptableObject
{
    public GridSys<GridObjectSO> gridReference;

    public GameObject gameObject;

    public Vector2Int position;
    public int x;
    public int y;
    public int z;

    public int cellSize;
    public int cellValue;

    public CellScript NorthNeighbour()
    {

        //gridReference.GetGridObject(x, z + 1)
        return gameObject.GetComponent<CellScript>();
    }
    public GridObjectSO SouthNeighbour()
    {
        return gridReference.GetGridObject(x, z - 1);
    }
    public GridObjectSO EastNeighbour()
    {
        return gridReference.GetGridObject(x + 1, z);
    }
    public GridObjectSO WestNeighbour()
    {
        return gridReference.GetGridObject(x - 1, z);
    }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetPosition(int x, int z)
    {
        position = new Vector2Int(x, z);
        this.x = x;
        this.z = z;
    }

    public void CopyGridObject(GridObjectSO gridObject)
    {
        gridReference = gridObject.gridReference;
        position = gridObject.position;
        x = gridObject.x;
        z = gridObject.z;
        cellValue = gridObject.cellValue;
    }

    public GridObjectSO InitializeGridObject(GridSys<GridObjectSO> grid, int x, int y, int z)
    {
        gridReference = grid;
        position = new Vector2Int(x, z);
        this.x = x;
        this.y = y;
        this.z = z;
        cellValue = 0;
        return this;
    }

    public void EmptyGridObject()
    {
        gridReference = null;
        position = new Vector2Int(-1, -1);
        x = -1;
        z = -1;
        cellValue = 0;
    }

    public enum CellColors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        White,
        Grey,
        Black,
    }

    public void SetColor(CellColors color)
    {
        switch (color)
        {
            case CellColors.Red:
                spriteRenderer.color = Color.red;
                break;
            case CellColors.Green:
                spriteRenderer.color = Color.green;
                break;
            case CellColors.Blue:
                spriteRenderer.color = Color.blue;
                break;
            case CellColors.Yellow:
                spriteRenderer.color = Color.yellow;
                break;
            case CellColors.Orange:
                spriteRenderer.color = new Color(1, 0.5f, 0);
                break;
            case CellColors.Purple:
                spriteRenderer.color = new Color(0.5f, 0, 0.5f);
                break;
            case CellColors.White:
                spriteRenderer.color = Color.white;
                break;
            case CellColors.Grey:
                spriteRenderer.color = Color.grey;
                break;
            case CellColors.Black:
                spriteRenderer.color = Color.black;
                break;
        }
    }

    public Color GetColor()
    {
        return spriteRenderer.color;
    }

    public CellColors NextColor(CellColors currentColor)
    {
        switch (currentColor)
        {
            case CellColors.Red:
                return CellColors.Green;
            case CellColors.Green:
                return CellColors.Blue;
            case CellColors.Blue:
                return CellColors.Yellow;
            case CellColors.Yellow:
                return CellColors.Orange;
            case CellColors.Orange:
                return CellColors.Purple;
            case CellColors.Purple:
                return CellColors.White;
            case CellColors.White:
                return CellColors.Grey;
            case CellColors.Grey:
                return CellColors.Black;
            case CellColors.Black:
                return CellColors.Red;
        }
        return CellColors.Red;
    }

}




