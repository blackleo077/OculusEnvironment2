using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanel : DrawToolsInteractable
{
    Vector2 ColorReferencePoint;
    GameObject ColorPreview;

    bool interacting;
    Vector3 ColorPanelOffset = new Vector3(0f,0.01f,0.015f);

    LineRenderer linerend;

    private void Start()
    {
        interacting = true;
        linerend = GetComponent<LineRenderer>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        interacting = true;
    }
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        SimulateRaycast(drawTools.GetHeadPosition(), Vector3.up);
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        interacting = false;
    }

    private void SimulateRaycast(Vector3 simulatetarget, Vector3 direction)
    {
        RaycastHit hit;
        linerend.SetPosition(0, simulatetarget);
        linerend.SetPosition(0, direction*100);
        print("SimulateRaycast");
        if (Physics.Raycast(simulatetarget, direction,out hit,100))
        {
            //SetColorPreview(hit.point);
            print(hit.point);
        }
    }

    private void SetColorPreview(Vector3 PointOnCollider)
    {

        ColorPreview.transform.localPosition = new Vector3(
            ColorReferencePoint.x + ColorPanelOffset.x,
            ColorReferencePoint.y + ColorPanelOffset.y,
            0 + ColorPanelOffset.z);


    }
}
