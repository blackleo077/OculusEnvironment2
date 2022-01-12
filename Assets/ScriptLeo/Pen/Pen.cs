using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : DrawTools
{
    [SerializeField]
    private int[] ColorableMaterialIndex;
    protected override void Start()
    {
        base.Start();
        SetColorableMaterial(ColorableMaterialIndex);
        Size = 5;
        Color = Color.black;
        BrushID = 1;
    }

    public override void SetColor(Color c) { base.SetColor(c); }
    public override void SetSize(int s) { base.SetSize( s); }
    public override void SetBrush(int brushid) { base.SetBrush( brushid ); }
    protected override void SetColorableMaterial(int[] index)
    {
        base.SetColorableMaterial(index);
    }
}
