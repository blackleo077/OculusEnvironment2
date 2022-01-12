using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenBrush:  DrawToolsInteractable
{
    private int BrushID;
    [SerializeField]
    private Texture2D src_brushtexture;
    private Texture2D brushtexture;

    Image mySprite;

    public void Init(int brushid, Texture2D texture)
    {
        BrushID = brushid;
        src_brushtexture = texture;

        brushtexture = new Texture2D(src_brushtexture.width, src_brushtexture.height);

        mySprite = GetComponentInChildren<Image>();
        SetSprite(src_brushtexture);
    }

    public void SetBrushID(int id)
    {
        BrushID = id;
    }

    public Color[] GetBrushColorArray()
    {
        return TextureToColorArray();
    }

    public Color[] GetBrushColorArray(Color color)
    {
        SetBrushColor(color);
        return TextureToColorArray();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (drawTools is Pen)
        {
            (drawTools as Pen).SetBrush(BrushID);
        }
    }

    private void SetSprite(Texture2D tex)
    {
        mySprite.sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height),Vector2.zero);
    }


    private void SetBrushColor(Color c)
    {
        for (int y = 0; y < src_brushtexture.height; y++)
        {
            for (int x = 0; x < src_brushtexture.width; x++)
            {
                Color tempC = src_brushtexture.GetPixel(x, y);
                float CAlpha = tempC.a;
                Color targetC = c;
                targetC.a = CAlpha;
                brushtexture.SetPixel(x, y, targetC);
            }
        }
        brushtexture.Apply();
    }

    private Color[] TextureToColorArray()
    {
        return brushtexture.GetPixels(0, 0, brushtexture.width, brushtexture.height);
    }
}
