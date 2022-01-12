using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class DrawToolsInteractable : MonoBehaviour
{

    protected enum VibrationType
    {
        Singal,
        Continues,
        InOut,
        None
    }

    protected enum amplitudeType
    {
        Weak = 2,
        Medium = 5,
        Strong = 10
    }

    [SerializeField]
    protected VibrationType vibrationType;
    protected bool vibratable;
    protected DrawTools drawTools;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<DrawTools>(out  drawTools)) { return; }
        virbateHand(VibrationType.Singal, amplitudeType.Weak,0.1f);
        virbateHand(VibrationType.InOut,amplitudeType.Strong, 0.1f);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<DrawTools>(out  drawTools)) { return; }
        virbateHand(VibrationType.InOut, amplitudeType.Strong, 0.1f);
        drawTools = null;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<DrawTools>(out  drawTools)) { return; }
        virbateHand(VibrationType.Continues,amplitudeType.Medium,0.1f);
    }
    /*
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!other.transform.TryGetComponent<Pen>(out Pen p)) { return; }
        pen = p;
        virbateHand(VibrationType.Singal, amplitudeType.Weak, 0.1f);
        virbateHand(VibrationType.InOut, amplitudeType.Strong, 0.1f);
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        if (!other.transform.TryGetComponent<Pen>(out Pen p)) { return; }
        virbateHand(VibrationType.InOut, amplitudeType.Strong, 0.1f);
        pen = null;
    }

    protected virtual void OnCollisionStay(Collision other)
    {
        if (!other.transform.TryGetComponent<Pen>(out Pen p)) { return; }
        virbateHand(VibrationType.Continues, amplitudeType.Medium, 0.1f);
    }

    */



    protected virtual void virbateHand(VibrationType targetType, amplitudeType amplitude, float duration)
    {
        if (vibrationType != targetType) return;

        if (!drawTools.TryGetComponent<XRGrabObject>(out XRGrabObject grabbale)) return;
        XRGrabber targetHand = grabbale.HandInteracting;
        if (targetHand != null) { targetHand.VibrateController(((float)amplitude)/10, duration); }
    }
}
