using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CellScript : MonoBehaviour, IClickable
{

    private GridObjectSO gridObjectReference;

    private TextMeshProUGUI textField;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        textField.outlineWidth = 0.2f;
        if (textField.color == Color.white)
        {
            textField.outlineColor = Color.black;
        }
        else
        {

            textField.outlineColor = Color.white;
        }
        textField.text = text;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }

    public void SetTextColor(Color color)
    {
        textField.color = color;
    }

}
