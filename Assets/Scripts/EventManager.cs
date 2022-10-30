using System;


public static class EventManager
{
    public static event Action<CellScript> cellClicked;
    public static void OnCellClicked(CellScript _cellClicked) => cellClicked?.Invoke(_cellClicked);

    public static event Action<GridSystemSO> boardIsReady;
    public static void OnBoardIsReady(GridSystemSO _gridSO) => boardIsReady?.Invoke(_gridSO);


    public static event Action startProgram;
    public static void OnStartProgram() => startProgram?.Invoke();


}
