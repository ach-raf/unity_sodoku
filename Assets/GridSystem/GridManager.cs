using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public GridSystemSO gridSO;

    public GridObjectSO tileSO;

    public Canvas worldSpaceCanvas;
    private GridObjectSO.CellColors currentColor = GridObjectSO.CellColors.Red;
    private TextMeshProUGUI textField;


    void Awake()
    {
        gridSO.gridReference = new GridSys<GridObjectSO>(gridSO.x, gridSO.z, gridSO.cellSize, gridSO.originPosition,
                                                    (GridSys<GridObjectSO> grid, int x, int y, int z) => CreateGridObject(grid, x, y, z));

    }
    // Start is called before the first frame update
    void Start()
    {
        InstantiateGridObjects();
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
        for (int x = 0; x < gridSO.x; x++)
        {
            for (int z = 0; z < gridSO.z; z++)
            {
                GridObjectSO gridObject = gridSO.gridReference.GetGridObject(x, z);
                SpawnGridObject(ref gridObject, x, z, currentColor);
                currentColor = gridObject.NextColor(currentColor);


            }
        }
        EventManager.OnBoardIsReady(gridSO);

    }

    public void SpawnGridObject(ref GridObjectSO gridObject, int x, int z, GridObjectSO.CellColors currentColor)
    {
        GameObject spawnObject = Instantiate(tileSO.gameObject);
        spawnObject.transform.position = gridSO.gridReference.GetWorldPosition2D(x, z);
        spawnObject.transform.SetParent(worldSpaceCanvas.transform);
        spawnObject.name = $"{x}, {z}";
        spawnObject.GetComponent<SpriteRenderer>().gameObject.name = $"{x}, {z}";
        gridObject = gridSO.gridReference.GetGridObject(x, z);
        gridObject.gameObject = spawnObject;
        gridObject.gridReference = gridSO.gridReference;
        gridObject.x = x;
        gridObject.z = z;
        gridObject.position = new Vector2Int(x, z);
        gridObject.SetColor(currentColor);

        textField = spawnObject.GetComponentInChildren<TextMeshProUGUI>();
        textField.text = spawnObject.gameObject.name;

        spawnObject.GetComponent<CellScript>().SetGridObjectReference(gridObject);


        gridSO.gridReference.SetValue(x, z, gridObject);
    }


}