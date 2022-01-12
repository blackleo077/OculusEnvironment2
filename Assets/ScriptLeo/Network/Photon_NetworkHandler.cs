

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Photon_NetworkHandler : MonoBehaviourPunCallbacks
{
    public static Photon_NetworkHandler instance;

    string gameVersion = "1";
    bool isConnecting;

    public UnityEvent HideMasterServerBt, HideJoinRoomBt;
    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    #endregion


    private void Start()
    {
//#if UNITY_EDITOR
        Connect();
//#endif
    }
    #region Public Methods
    public void Connect()
    {

        PhotonNetwork.NickName = "Tester"+System.DateTime.Now.Minute + System.DateTime.Now.Second;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("join room");

        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            Debug.Log("connect with setting");
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        //PhotonNetwork.JoinRandomRoom();
        isConnecting = false;
        HideMasterServerBt.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        isConnecting = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4});
    }

    public override void OnJoinedRoom()
    {
        HideJoinRoomBt.Invoke();
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {

            PhotonNetwork.NickName = "Server";
            Debug.Log("We load the 'WB_photon' ");
            PhotonNetwork.LoadLevel("WB_photon");
        }
    }

    #endregion


    

}
