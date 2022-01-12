using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CustomButtonAction))]
public class XRGrabber : MonoBehaviour
{

    private delegate void SetGrabbleTarget();
    private event SetGrabbleTarget Grabble_Grab;

    private delegate void ResetGrabbleTarget();
    private event ResetGrabbleTarget Grabble_Release;

    public static bool isInteracting;

    [SerializeField]
    XRGrabObject GrabbleObject;
    Vector3 GrabPosOffset;
    Quaternion GrabRotOffset;

    [SerializeField]
    public XRController Device;
    [SerializeField]
    public InputControl controller;

    [SerializeField]
    public InputControl controller2;
    CustomButtonAction csVRAction;

    private void Start() {
        csVRAction = GetComponent<CustomButtonAction>();
    }

    private void FixedUpdate()
    {
        if (isInteracting)
        {
            Grabbing();
        }
    }

    public void GrabBegin()
    {
        if(GrabbleObject==null) { return; }
        GrabbleObject.transform.SetParent(null);

        Vector3 relPos = GrabbleObject.transform.position - transform.position;
        relPos = Quaternion.Inverse(transform.rotation) * relPos;
        GrabPosOffset = relPos;

        Quaternion relOri = Quaternion.Inverse(transform.rotation) * GrabbleObject.transform.rotation;
        GrabRotOffset = relOri;

        Grabble_Grab.Invoke();
        isInteracting = true;
    }

    private void Grabbing()
    {
        if (GrabbleObject == null) { return; }

        Rigidbody grabbedRigidbody = GrabbleObject.GetComponent<Rigidbody>();
        Vector3 grabbablePosition = transform.position + transform.rotation * GrabPosOffset;
        Quaternion grabbableRotation = transform.rotation * GrabRotOffset;

        grabbedRigidbody.MovePosition(grabbablePosition);
        grabbedRigidbody.MoveRotation(grabbableRotation);

    }

    public void GrabEnd()
    {
        if (GrabbleObject == null) { return; }
        Grabble_Release.Invoke();
        isInteracting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInteracting)
            return;
        if (other.TryGetComponent(out XRGrabObject obj) )
        {
            if (!obj.isMyNetworkObject) { return; }
            obj.HandInteracting = this;
            GrabbleObject = obj;

            Grabble_Grab += obj.Grab;
            Grabble_Release += obj.Release;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInteracting)
            return;
        if (other.TryGetComponent(out XRGrabObject obj) )
        {
            if (!obj.isMyNetworkObject) { return; }
            obj.HandInteracting = null;
            GrabbleObject = null;

            Grabble_Grab -= obj.Grab;
            Grabble_Release -= obj.Release;

        }
    }

    public void VibrateController(float amplitude, float duration)
    {
        controller = csVRAction.ControllerReference[0].action.activeControl;
        if (controller == null) return;
        if (controller.device is XRControllerWithRumble rumble)
            rumble.SendImpulse(amplitude, duration);

        Debug.LogError("Vibratre|" + controller.displayName+"|"+amplitude + "|" + duration);
    }

}
