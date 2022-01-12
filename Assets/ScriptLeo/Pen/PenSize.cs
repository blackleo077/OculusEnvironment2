using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenSize : DrawToolsInteractable
{
    GameObject SliderController;
    Transform SliderStart;
    Transform SliderEnd;
    float SliderDistance;

    float PenSizeValue;
    [SerializeField]
    private int PenMaxSize;
    [SerializeField]
    private int PenMinSize;


    [SerializeField]
    private float SliderControllerMaxSize;
    [SerializeField]
    private float SliderControllerMinSize;

    // Start is called before the first frame update
    void Start()
    {
        SliderController = transform.GetChild(0).gameObject;
        SliderStart = transform.GetChild(1);
        SliderEnd = transform.GetChild(2);
        SliderDistance = Mathf.Abs(SliderStart.position.x - SliderEnd.position.x);
    }


    void PenPosToSliderPos()
    {
        Vector3 penpos = drawTools.GetHeadPosition();
        Vector3 SliderPos = SliderController.transform.position;

        if (isReachBorder(penpos.x))
        {
            penpos.x = MinMaxPosX(penpos.x);
        }
        SliderPos.x = penpos.x;
        SliderController.transform.position = SliderPos;

        PenSizeValue = NormalizedPenPos(SliderPos.x);
        SliderController.transform.localScale = UpdateSliderControllerSize(PenSizeValue);
        
    }

    bool isReachBorder(float posx)
    {
        return posx < SliderStart.position.x || posx > SliderEnd.position.x;
    }

    float MinMaxPosX(float posx)
    {
        return Mathf.Max( Mathf.Min(posx, SliderEnd.position.x), SliderStart.position.x);
    }

    float NormalizedPenPos(float PosX)
    {
        return Mathf.Abs(PosX - SliderEnd.position.x) / SliderDistance;
    }

    float SizeRemap(float value, float newMax, float newMin)
    {
        return value * (newMax - newMin) + newMin;
    }

    Vector3 UpdateSliderControllerSize(float value)
    {
        float newScale = SizeRemap(value, SliderControllerMinSize, SliderControllerMaxSize);
        return new Vector3(newScale, newScale, newScale);
    }

    protected override void OnTriggerExit(Collider other)
    {
        drawTools.SetSize((int)SizeRemap(PenSizeValue, PenMaxSize, PenMinSize));
        base.OnTriggerExit(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        PenPosToSliderPos();
    }

}
