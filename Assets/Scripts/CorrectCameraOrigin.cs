using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectCameraOrigin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CorrectCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CorrectCameraPosition()
    {
        //0.5 because unity tiles, 0 for example goes between -0.5 and 0.5
        this.transform.position = new Vector3((float)9 / 2 - 0.5f, (float)9 / 2 - 0.5f, -10);
    }
}
