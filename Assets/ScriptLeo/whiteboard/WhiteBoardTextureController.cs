using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoardTextureController : MonoBehaviour
{
    [SerializeField]
    Texture2D wbtexture;
    float UpdatePerSecond;

    public void Init(Texture2D targetTexture)
    {
        wbtexture = targetTexture;
        SetUpdateRate(30);
        StartUpdate();
    }

    private void StartUpdate()
    {
        StartCoroutine(ApplyTexture());
    }

    IEnumerator ApplyTexture()
    {
        wbtexture.Apply();
        yield return new WaitForSeconds(UpdatePerSecond);
        StartCoroutine(ApplyTexture());
    }

    public void SetUpdateRate(int frameRate)
    {
        UpdatePerSecond = (1f / frameRate);
    }


    public void LoadTextureAsBackground(string texturename)
    {
        ClearWhiteBoard();
        StartCoroutine(LoadTextureFromResource(texturename));
    }

    IEnumerator LoadTextureFromResource(string texturename)
    {
        Sprite bgsprite = Resources.Load<Sprite>("WhiteBoardBg/"+ texturename);
        print("Load complete");
        Texture2D bgtexture = bgsprite.texture;
        Color[] colorpixels = bgtexture.GetPixels(0, 0, bgtexture.width, bgtexture.height);
        //System.Array.Reverse(colorpixels, 0, colorpixels.Length);
        yield return new WaitForEndOfFrame();
        int nWidth = (wbtexture.width - bgtexture.width) / 2;
        int nHeight = (wbtexture.height - bgtexture.height) / 2;
        wbtexture.SetPixels(nWidth, nHeight, bgtexture.width, bgtexture.height, colorpixels);
        wbtexture.Apply();
    }

    public void ClearWhiteBoard()
    {

        Color[] pixels = wbtexture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        wbtexture.SetPixels(pixels);
        wbtexture.Apply();
    }

    
}
