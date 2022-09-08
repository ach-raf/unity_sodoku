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

    public GridObjectSO NorthNeighbour()
    {

        return gridReference.GetGridObject(x, z + 1);
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

    public GridObjectSO GetRandomCell()
    {
        return gridReference.GetGridObject(Random.Range(0, gridReference.GetWidth()), Random.Range(0, gridReference.GetHeight()));
    }

    public CellScript GetNeighbourCellScript(GridObjectSO _neighbour)
    {
        return _neighbour.gameObject.GetComponent<CellScript>();
    }

    public CellScript GetCellScript()
    {
        return gameObject.GetComponent<CellScript>();
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
        Cyan,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        White,
        Grey,
        Magenta,


    }

    public void SetColor(CellColors color)
    {
        switch (color)
        {
            case CellColors.Cyan:
                spriteRenderer.color = Color.cyan;
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
            case CellColors.Magenta:
                spriteRenderer.color = Color.magenta;
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
            case CellColors.Cyan:
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
                return CellColors.Magenta;
            case CellColors.Magenta:
                return CellColors.Cyan;
        }
        return CellColors.Cyan;
    }

}




