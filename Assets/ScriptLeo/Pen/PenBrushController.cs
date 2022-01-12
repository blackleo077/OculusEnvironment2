using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PenBrushController : MonoBehaviour
{
    [SerializeField]
    List<PenBrush> PenBrushList;
    [SerializeField]
    List<Texture2D> BrushTextureList;

    public GameObject BrushPadPrefab;

    float PadOffset = -0.11f;

    public static PenBrushController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        InitBrushs();
    }

    private void InitBrushs()
    {
        for(int i = 0; i < BrushTextureList.Count; i++)
        {
            int id = i ;

            GameObject brushPad = GameObject.Instantiate(BrushPadPrefab, this.transform);
            float posValue = i * PadOffset;
            brushPad.transform.localPosition = new Vector3(posValue, 0, 0);
            PenBrush brush = brushPad.AddComponent<PenBrush>();
            brush.Init(id, BrushTextureList[i]);
            PenBrushList.Add(brush);
        }
    }

    public Color[] GetBrushColorArray(int id)
    {
        return PenBrushList[id].GetBrushColorArray();
    }

    public Color[] GetBrushColorArray(int id, Color c)
    {
        return PenBrushList[id].GetBrushColorArray(c);
    }


}
