using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    protected string myText;
    public virtual void text()
    {
        print("This is ColorManager");

    }

    public virtual string GetText()
    {
        return myText;
    }

    public virtual void SetText(string newtext)
    {
        myText = newtext;
    }
}
