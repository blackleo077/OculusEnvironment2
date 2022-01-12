using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class NetworkGame_Controller : MonoBehaviourPunCallbacks
{
    
    public GameObject PlayerVRPrefab;
    public GameObject ServerCameraPrefab;
    private GameObject myCamera;
    public static NetworkGame_Controller instance;
    [SerializeField]
    List<DrawTools> NetworkTools;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /*
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            Debug.Log("Master Client Enter Room");
            ServerCameraPrefab = PhotonNetwork.Instantiate(this.ServerCameraPrefab.name, ServerCameraPrefab.transform.position, ServerCameraPrefab.transform.rotation, 0);
            myCamera = ServerCameraPrefab;
        }
        else
        {
            Debug.Log("Normal Player Enter Room");
            PlayerVRPrefab = PhotonNetwork.Instantiate(this.PlayerVRPrefab.name, PlayerVRPrefab.transform.position, PlayerVRPrefab.transform.rotation, 0);
            myCamera = PlayerVRPrefab;
        }
    }
    */

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {

            Debug.Log("Master Client Enter Room");
            ServerCameraPrefab = PhotonNetwork.Instantiate(this.ServerCameraPrefab.name, ServerCameraPrefab.transform.position, ServerCameraPrefab.transform.rotation, 0);
            myCamera = ServerCameraPrefab;
        }
        else
        {
            Debug.Log("Normal Player Enter Room");
            PlayerVRPrefab = PhotonNetwork.Instantiate(this.PlayerVRPrefab.name, PlayerVRPrefab.transform.position, PlayerVRPrefab.transform.rotation, 0);
            myCamera = PlayerVRPrefab;
        }
    }

    public void AddNetworkObject(DrawTools g)
    {
        NetworkTools.Add(g);
    }

    public void DestoryNetworkObject(DrawTools g)
    {

        DrawTools RepeatedDrawTool = null;
        foreach(DrawTools tool in NetworkTools)
        {
            if(tool.GetType() == g.GetType() )
            {
                Debug.Log("Repeated Object");
                if (tool.GetComponent<PhotonView>().Owner == g.GetComponent<PhotonView>().Owner)
                {
                    RepeatedDrawTool = tool;
                }
                else
                {
                    Debug.Log("Repeated Object but not owner");
                }
            }
        }
        if (RepeatedDrawTool != null)
        {
            NetworkTools.Remove(RepeatedDrawTool);
            PhotonNetwork.Destroy(RepeatedDrawTool.gameObject);
            Debug.LogError("Delete network object");
        }

    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        foreach(DrawTools g in NetworkTools)
        {
            PhotonNetwork.Destroy(g.gameObject);
        }
        PhotonNetwork.Destroy(myCamera);
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
