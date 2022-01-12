using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoardLaser : MonoBehaviourPun
{
	private Transform _laserstart, _laserend;
	private float _laserLength;

	//layer
	int PenInteractable = 1 << 6;

	[SerializeField]
	private WhiteBoard whiteboard;
	private RaycastHit touch;

	LineRenderer laserSimulate;

	//var
	private bool IsTouchingBoard = false;
	private Vector2 LastPenTouchCoordinate, CurrenPenTouchCoordinate;

    private void Start()
    {
		laserSimulate = GetComponent<LineRenderer>();

	}

    public void LaserToBoard(int PenSize, Color PenColor, int PenBrushID)
	{
		if (Physics.Raycast(_laserstart.position, _laserend.position- _laserstart.position, out touch, _laserLength, PenInteractable))
		{
			if (!touch.collider.TryGetComponent(out WhiteBoard db))
                return;

            OnPenTouchBoard(db);
            //laserSimulate.SetPosition(0, _laserstart.position);
            //laserSimulate.SetPosition(1, touch.point);
			CurrenPenTouchCoordinate = touch.textureCoord;
			whiteboard.DrawPath(LastPenTouchCoordinate, CurrenPenTouchCoordinate, PenSize, PenColor, PenBrushID);
			LastPenTouchCoordinate = CurrenPenTouchCoordinate;
        }
		else
		{
			OnPenLeaveBoard();
		}
	}

	public void SetLaser(Transform start, Transform end)
	{
		_laserstart = start;
		_laserend = end;
		_laserLength = Vector3.Distance(_laserstart.position, _laserend.position);
	}


	void OnPenTouchBoard(WhiteBoard db)
	{
		if (!IsTouchingBoard)
		{
			LastPenTouchCoordinate = touch.textureCoord;
			whiteboard = db;
			IsTouchingBoard = true;
			print("touch" + touch.textureCoord);
		}
	}

	void OnPenLeaveBoard()
	{
		if (IsTouchingBoard)
		{
			IsTouchingBoard = false;
			print("off touch");
		}
	}



}
