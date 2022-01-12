using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.MaterialController;

public class PenColor : DrawToolsInteractable
{
    Material myMaterial;
    Color myColor;


    void Start()
    {
        myMaterial = GetComponent<MeshRenderer>().material;
        myColor = myMaterial.color;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (drawTools is Pen)
        {
            (drawTools as Pen).SetColor(myColor);
        }
    }


}
