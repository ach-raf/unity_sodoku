using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public GridSystemSO gridSO;

    public GridObjectSO tileSO;

    public Canvas worldSpaceCanvas;
    private TextMeshProUGUI textField;

    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        EventManager.startProgram += InstantiateGridObjects;
    }

    private void OnDisable()
    {
        EventManager.startProgram -= InstantiateGridObjects;
    }
    void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnStartProgram();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private GridObjectSO CreateGridObject(GridSys<GridObjectSO> _gridReference, int x, int y, int z)
    {
        GridObjectSO tile = ScriptableObject.Instantiate(tileSO);
        return tile.InitializeGridObject(gridSO.gridReference, x, y, z);
    }
    void InstantiateGridObjects()
    {
        DestroyObjects();
        gridSO.gridReference = new GridSys<GridObjectSO>(gridSO.x, gridSO.z, gridSO.cellSize, gridSO.originPosition,
                                                    (GridSys<GridObjectSO> grid, int x, int y, int z) => CreateGridObject(grid, x, y, z));
        //tileSO.EmptyGridObject();

        for (int x = 0; x < gridSO.x; x++)
        {
            for (int z = 0; z < gridSO.z; z++)
            {
                GridObjectSO gridObject = gridSO.gridReference.GetGridObject(x, z);
                SpawnGridObject(ref gridObject, x, z, ChooseColor(x, z));


                //currentColor = gridObject.NextColor(currentColor);


            }
        }
        EventManager.OnBoardIsReady(gridSO);

    }

    public void SpawnGridObject(ref GridObjectSO gridObject, int x, int z, GridObjectSO.CellColors color)
    {
        gridObject.gameObject = Instantiate(tileSO.gameObject);
        spriteRenderer = gridObject.gameObject.GetComponent<SpriteRenderer>();
        CellScript cellScript = gridObject.gameObject.GetComponent<CellScript>();

        gridObject.gameObject.transform.position = gridSO.gridReference.GetWorldPosition2D(x, z);
        gridObject.gameObject.transform.SetParent(worldSpaceCanvas.transform);
        gridObject.gameObject.name = $"{x}, {z}";
        SetColor(color);

        cellScript.GetSpriteRenderer().gameObject.name = $"{x}, {z}";

        gridObject = gridSO.gridReference.GetGridObject(x, z);
        gridObject.gridReference = gridSO.gridReference;
        gridObject.x = x;
        gridObject.z = z;
        gridObject.position = new Vector2Int(x, z);
        //gridObject.SetColor(color);

        gridObject.gameObject.GetComponent<CellScript>().SetGridObjectReference(gridObject);


        //cellScript.SetTextField(spawnObject.gameObject.name);



        gridSO.gridReference.SetValue(x, z, gridObject);
    }
    public void SetColor(GridObjectSO.CellColors color)
    {
        switch (color)
        {
            case GridObjectSO.CellColors.Cyan:
                spriteRenderer.color = Color.cyan;
                break;
            case GridObjectSO.CellColors.Green:
                spriteRenderer.color = Color.green;
                break;
            case GridObjectSO.CellColors.Blue:
                spriteRenderer.color = Color.blue;
                break;
            case GridObjectSO.CellColors.Yellow:
                spriteRenderer.color = Color.yellow;
                break;
            case GridObjectSO.CellColors.Orange:
                spriteRenderer.color = new Color(1, 0.5f, 0);
                break;
            case GridObjectSO.CellColors.Purple:
                spriteRenderer.color = new Color(0.5f, 0, 0.5f);
                break;
            case GridObjectSO.CellColors.White:
                spriteRenderer.color = Color.white;
                break;
            case GridObjectSO.CellColors.Grey:
                spriteRenderer.color = Color.grey;
                break;
            case GridObjectSO.CellColors.Magenta:
                spriteRenderer.color = Color.magenta;
                break;
        }
    }

    public GridObjectSO.CellColors ChooseColor(int _x, int _z)
    {
        if (_x < 3)
        {

            if (_z < 3)
            {
                //array 0
                return GridObjectSO.CellColors.Cyan;

            }
            else if (_z < 6)
            {
                //array 1
                return GridObjectSO.CellColors.Green;
            }
            else
            {
                //array 2
                return GridObjectSO.CellColors.Blue;
            }
        }
        else if (_x < 6)
        {

            if (_z < 3)
            {
                //array 3
                return GridObjectSO.CellColors.Yellow;
            }
            else if (_z < 6)
            {
                //array 4
                return GridObjectSO.CellColors.Orange;
            }
            else
            {
                //array 5
                return GridObjectSO.CellColors.Purple;
            }
        }
        else
        {

            if (_z < 3)
            {
                //array 6
                return GridObjectSO.CellColors.White;
            }
            else if (_z < 6)
            {
                //array 7
                return GridObjectSO.CellColors.Grey;
            }
            else
            {
                //array 8
                return GridObjectSO.CellColors.Magenta;
            }
        }
    }

    public void DestroyObjects()
    {
        if (gridSO.gridReference != null)
        {
            for (int x = 0; x < gridSO.x; x++)
            {
                for (int z = 0; z < gridSO.z; z++)
                {
                    GridObjectSO gridObject = gridSO.gridReference.GetGridObject(x, z);
                    if (gridObject.gameObject != null)
                    {
                        gridObject.EmptyGridObject();
                    }

                }
            }
        }

    }


}