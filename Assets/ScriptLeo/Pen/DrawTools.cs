using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.MaterialController;
using Photon.Pun;

public class DrawTools : MonoBehaviour
{
	protected Color Color;
	protected int Size;
	protected int BrushID;


	MeshRenderer Body;

	[SerializeField]
	protected Material[] ColorableMat_Ref;
	protected List<Material> Colorable = new List<Material>();

	private Transform tipStart, tipEnd;
	private BoardLaser mylaser;
	private PhotonView pv;
	protected enum laserDirection
    {
		up,
		down,
		left,
		right,
		forward,
		back
    }
	[SerializeField]
	protected laserDirection LaserDirection;
	[SerializeField]
	protected float laserOffset;

    protected void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
			TryGetComponent(out pv);
		}
    }

    protected virtual void Start()
    {
		Body = GetComponentInChildren<MeshRenderer>();
		InitDrawToolLaser();
	}

	private void InitDrawToolLaser()
    {
		GameObject laser = Instantiate(Resources.Load("Laser", typeof(GameObject)), Body.transform) as GameObject;
		tipStart = laser.transform.GetChild(0).transform;
		tipEnd = laser.transform.GetChild(1).transform;
		SetLaserDirection();
		laser.TryGetComponent(out mylaser);
		mylaser.SetLaser(tipStart, tipEnd);
	}

    protected virtual void SetColorableMaterial(int[] index)
    {
		foreach(int i in index)
        {
			Colorable.Add( Body.materials[i]);
        }
    }

	private void Update()
	{
		if (PhotonNetwork.IsConnected )
		{
			if(pv.IsMine)
				mylaser.LaserToBoard(Size, Color, BrushID);
		}
		else
		{
			mylaser.LaserToBoard(Size, Color, BrushID);
		}
		
	}

	public virtual void SetColor(Color c)
	{
		Color = c;
		foreach(Material m in Colorable)
		{
			C_MatControl.FadeMaterialColorTo(m, Color, 0.15f);
		}
	}

	public virtual void SetSize(int s) { Size = s; }

	public virtual void SetBrush(int brushid) { BrushID = brushid; }

	public Vector3 GetHeadPosition()
    {
		return tipEnd.transform.position;
    }

	private void SetLaserDirection()
    {
		Vector3 Center = Body.bounds.center;
		Vector3 Min = Body.bounds.min;
		Vector3 Max = Body.bounds.max;
		Vector3 enddir = Vector3.zero;
		Vector3 startdir = Center;
		switch (LaserDirection)
		{
			case laserDirection.right:
				enddir = new Vector3(Max.x, Center.y, Center.z);
				enddir.x += laserOffset;
				//startdir = new Vector3(Min.x, Center.y, Center.z);
				break;
			case laserDirection.left:
				enddir = new Vector3(Min.x, Center.y, Center.z);
				enddir.x -= laserOffset;
				//startdir = new Vector3(Max.x, Center.y, Center.z);
				break;
			case laserDirection.up:
				enddir = new Vector3(Center.x, Max.y, Center.z);
				enddir.y += laserOffset;
				//startdir = new Vector3(Center.x, Min.y, Center.z);
				break;
			case laserDirection.down:
				enddir = new Vector3(Center.x, Min.y, Center.z);
				enddir.y -= laserOffset;
				//startdir = new Vector3(Center.x, Max.y, Center.z);
				break;
			case laserDirection.forward:
				enddir = new Vector3(Center.x, Center.y, Max.z);
				enddir.z += laserOffset;
				//startdir = new Vector3(Center.x, Center.y, Min.z);
				break;
			case laserDirection.back:
				enddir = new Vector3(Center.x, Center.y, Min.z);
				enddir.z -= laserOffset;
				//startdir = new Vector3(Center.x, Center.y, Max.z);
				break;
		}

		tipEnd.position = enddir;
		tipStart.position = startdir;

	}
}
