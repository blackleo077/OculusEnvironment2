using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI;
using Custom.MaterialController;
using System.Linq;

public class Debug_CallFunction : MonoBehaviour
{
    public UnityEvent ToFunction;

    [SerializeField]
    bool isTrigger = true;
    [SerializeField]
    bool isUpdateColor = false;
    bool isActived ;

    Material ObjectMaterial;
    Color ObjectInitColor;

    // Start is called before the first frame update
    void Start()
    {
        isActived = false;
        ObjectMaterial = GetComponent<MeshRenderer>().material;
        ObjectInitColor = ObjectMaterial.color;
        if (isUpdateColor)
            C_MatControl.FadeMaterialColorTo(ObjectMaterial, Color.black, 1f);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!isTrigger) return;
        if (!other.TryGetComponent(out XRGrabber xRGrabber)) return;
        isActived = !isActived;
        ToFunction.Invoke();
        if(isUpdateColor)
            UpdateColor(isActived);
    }

    public void InvokeFunction()
    {
        ToFunction.Invoke();
    }

    private void UpdateColor(bool active)
    {
        if (active)
        {
            C_MatControl.FadeMaterialColorTo(ObjectMaterial, Color.green, 1f);

        }
        else
        {
            C_MatControl.FadeMaterialColorTo(ObjectMaterial, Color.black, 1f);
        }
    }

    
}
