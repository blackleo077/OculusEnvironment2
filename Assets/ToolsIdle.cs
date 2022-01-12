using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsIdle : MonoBehaviour
{
    
    public bool IsRotate, IsFloat;

    //float
    public float degreesPerSecond = 10f;
    public float amplitude = 0.03f;
    public float frequency = 0.5f;

    // Init Pos
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        if (IsRotate)
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
        }

        if (IsFloat)
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }
}
