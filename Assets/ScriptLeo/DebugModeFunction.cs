using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeFunction : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
       
    }
}
