using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellBasicInfo
{
    public int x;
    public int z;
    public int value;
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
