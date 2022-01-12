using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class CustomButtonAction : MonoBehaviour
{
    [SerializeField]
    public List<InputActionReference> ControllerReference;

    public UnityEvent InputPressDown, InputPress, InputRelease;

#if UNITY_EDITOR
    bool isPressing;
#endif

    private void Start()
    {
        foreach(InputActionReference input in ControllerReference)
        {
            input.action.started += OnPress;
            input.action.performed += OnPressRelease;
            input.action.canceled += OnRelease;
        }
    }

    private void OnDestroy()
    {
        foreach (InputActionReference input in ControllerReference)
        {
            input.action.started -= OnPress;
            input.action.performed -= OnPressRelease;
            input.action.canceled -= OnRelease;
        }
    }

    // private void OnEnable() { ControllerReference.asset.Enable(); }
    // private void OnDisable() { ControllerReference.asset.Disable(); }

    private void OnPress(InputAction.CallbackContext callback)
    {
        InputPressDown.Invoke();
        Debug.Log("OnPress");
    }

    private void OnPressRelease(InputAction.CallbackContext callback)
    {
        print(callback.ReadValue<float>());
        InputPress.Invoke();
        Debug.Log("Pressing");
    }

    private void OnRelease(InputAction.CallbackContext callback)
    {
        InputRelease.Invoke();
        Debug.Log("OnRelease");
    }

   

    


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isPressing == false)
            {
                isPressing = true;
                InputPressDown.Invoke();
                return;
            }
            else
            {
                isPressing = false;
                InputRelease.Invoke();
                return;
            }
        }
        if (isPressing)
        {
            InputPress.Invoke();
        }
#endif
        
    }

    

}
