using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ToolsGenerater : MonoBehaviourPun
{
    [SerializeField]
    private Transform SpawnPoint;
    [SerializeField]
    List<DrawTools> DrawToolsList;

    

    public void GenerateTool(DrawTools targettool)
    {
        if (targettool == null) return;

        ClearSpawePoint();
        GameObject tool;
        DrawTools drawTools;
        if (PhotonNetwork.IsConnected)
        {  tool = PhotonNetwork.Instantiate(targettool.name, SpawnPoint.position, targettool.transform.rotation);  }
        else
        {
            tool = Instantiate(Resources.Load(targettool.name, typeof(GameObject)), SpawnPoint.position, targettool.transform.rotation) as GameObject;
            tool.GetComponent<PhotonTransformView>().enabled = false;
        }

        tool.transform.SetParent(SpawnPoint);
        tool.transform.localPosition = Vector3.zero;

        drawTools = tool.GetComponent<DrawTools>();
        DestoryExistObject(drawTools);

        if (PhotonNetwork.IsConnected)
        { NetworkGame_Controller.instance.AddNetworkObject(drawTools); }
        else
        { DrawToolsList.Add(drawTools); }

    }
    private void ClearSpawePoint()
    {
        if (SpawnPoint.childCount > 0)
        {
            SpawnPoint.GetChild(0).TryGetComponent(out DrawTools d);
            DestoryExistObject(d);
        }
    }
    private void DestoryExistObject(DrawTools targettool)
    {
        if (PhotonNetwork.IsConnected)
        {
            NetworkGame_Controller.instance.DestoryNetworkObject(targettool);
        }
        else
        {
            DrawTools target = DrawToolsList.Find((obj) => obj.GetType() == targettool.GetType() );
            if (target != null)
            {
                DrawToolsList.Remove(target);
                Destroy(target.gameObject);
            }
        }
    }

    


}
