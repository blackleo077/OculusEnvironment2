using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.MaterialController;

public class UIButtonControl : MonoBehaviour
{
    Outline buttonOutline;
    bool StandBy;

    // Start is called before the first frame update
    void Start()
    {
        buttonOutline = GetComponentInChildren<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void ButtonOccupied()
    {
        StandBy = false;
        SetOutlineColor(Color.red, 0.25f);
    }

    protected virtual void ButtonRelease()
    {
        StandBy = true;
        SetOutlineColor( Color.green, 0.25f);
    }

    private void SetOutlineColor(Color to_c, float duration)
    {
        C_MatControl.FadeOutlineColorTo(buttonOutline, to_c, duration);
    }
}
