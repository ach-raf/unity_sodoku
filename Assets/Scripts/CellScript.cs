using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CellScript : MonoBehaviour, IClickable
{

    private GridObjectSO gridObjectReference;

    private TextMeshProUGUI textField;

    private void Awake()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void click()
    {
        EventManager.OnCellClicked(this);
    }

    public void SetGridObjectReference(GridObjectSO gridObjectReference)
    {
        this.gridObjectReference = gridObjectReference;
    }

    public GridObjectSO GetGridObjectReference()
    {
        return gridObjectReference;
    }

    public void SetTextField(string text)
    {
        textField.text = text;
    }
}
