using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : DrawTools
{
    protected override void Start()
    {
        base.Start();
        Size = 30;
        Color = Color.white;
        BrushID = 1;
    }

    public override void SetSize(int s) { base.SetSize(s); }
}
