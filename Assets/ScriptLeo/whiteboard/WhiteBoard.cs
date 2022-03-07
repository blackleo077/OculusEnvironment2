using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using MLAPI;
using System;

//[RequireComponent (typeof( WhiteBoardNetwork))]
//[RequireComponent(typeof(WhiteBoardTextureController))]
public class WhiteBoard : MonoBehaviour
{
	[Range(0, 120)]
	public Vector2 WhiteBoardTextureSize;
	public int WhiteBoardMargine;

	
	 
	private WhiteBoardNetwork WBNetwork;
	private WhiteBoardTextureController WBTextureController;

	public int WBResolution_W, WBResolution_H;
	public Sprite WhiteBoardSprite;
	[SerializeField]
	protected Texture2D WhiteBoardTexture;

	private void Start()
	{
		WhiteBoardTexture = new Texture2D(1920, 1080, TextureFormat.RGB24, true);
		WhiteBoardTexture.Apply();
		GetComponent<MeshRenderer>().material.mainTexture = WhiteBoardTexture;
		WBTextureController = GetComponent<WhiteBoardTextureController>();
		WBTextureController.Init(WhiteBoardTexture);
		WBTextureController.ClearWhiteBoard();

		WBNetwork = GetComponent<WhiteBoardNetwork>();
		WBNetwork.Init(WhiteBoardTexture, WhiteBoardNetwork.networkType.Photon);


		//WBNetwork = GetComponent<WhiteBoardNetwork>();
		//WBNetwork.Init(WhiteBoardTexture, WhiteBoardNetwork.networkType.MLAPI);
	}


	//network method 2
	public void DrawOnBoard(Vector2 lastpoint, Vector2 currentpoint, Color pencolor, int pensize, int PenBrushID)
	{
		float step = 1 / Vector2.Distance(lastpoint, currentpoint);

		//Color[] colorArray = PenBrushController.instance.GetBrushColorArray(PenBrushID, pencolor);
		Color[] colorArray = colorToArray(pencolor, pensize);
		for (float t = 0.01f; t < 1.00f; t += step)
		{
			Vector2 lerpPos = Vector2.Lerp(lastpoint, currentpoint, t);
			WhiteBoardTexture.SetPixels((int)lerpPos.x, (int)lerpPos.y, pensize, pensize, colorArray);
		}
		
	}


	public void DrawPath(Vector2 lastpoint, Vector2 currentpoint, int pensize, Color pencolor, int PenBrushID)
    {
		Vector2 t_lastpoint = PenToTextureCoordinate(lastpoint, pensize);
		Vector2 t_currentpoint = PenToTextureCoordinate(currentpoint, pensize);
		if (CheckPenWithinArea(t_currentpoint))
		{
			float t_texturedistance = Vector2.Distance(t_lastpoint, t_currentpoint);
			Debug.Log("Texture distance :"+ Vector2.Distance(t_lastpoint, t_currentpoint));
			if (t_texturedistance > 1f)
			{
				WhiteBoardNetwork.DrawData dd = new WhiteBoardNetwork.DrawData(t_lastpoint, t_currentpoint, pencolor, pensize, PenBrushID, 0);
				//WBNetwork.DrawTextureToServerRpc(t_lastpoint, t_currentpoint, pencolor, pensize, PenBrushID, NetworkManager.Singleton.LocalClientId);
				
				WBNetwork.DrawTextureToServerRpc(dd);
				Debug.Log("LaserOnBoard : DrawPath");
				DrawOnBoard(t_lastpoint, t_currentpoint, pencolor, pensize, PenBrushID);
			}
		}
	}


	bool CheckPenWithinArea(Vector2 point)
	{
		if (point.x < 0 + WhiteBoardMargine || point.x > WhiteBoardTexture.width - WhiteBoardMargine ||
			point.y < 0 + WhiteBoardMargine || point.y > WhiteBoardTexture.height - WhiteBoardMargine)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	Vector2 PenToTextureCoordinate(Vector2 worldcoordinate, int pensize)
	{
		float x = (worldcoordinate.x * WhiteBoardTexture.width - (pensize / 2));
		float y = (worldcoordinate.y * WhiteBoardTexture.height - (pensize / 2));
		return new Vector2(x, y);
	}

	Color[] colorToArray(Color c, int size)
    {
		return Enumerable.Repeat<Color>(c, size * size).ToArray<Color>();
	}

	public void LoadWhiteBoardBackground(string texturename)
	{
		WBNetwork.LoadTextureToWhiteBoardServerRpc(texturename);
		SetWhiteBoardTexture(texturename);
	}

	public void SetWhiteBoardTexture(string texturename)
    {
		WBTextureController.LoadTextureAsBackground(texturename);
	}
	public void ClearWhiteBoard()
	{
		WBTextureController.ClearWhiteBoard();
	}


}
