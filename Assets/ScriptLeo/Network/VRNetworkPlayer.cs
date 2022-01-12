using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;
using UnityEngine.InputSystem.XR;

public class VRNetworkPlayer : MonoBehaviourPun
{
    
    Camera cam;
    AudioListener myaudio;
    [SerializeField]
    ServerCamera server;

    
    private void Awake()
    {

        cam = GetComponentInChildren<Camera>();
        myaudio = GetComponentInChildren<AudioListener>();
        if (!photonView.IsMine)
        {
            disableAllComponents();

        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Set Server");
                server = GetComponent<ServerCamera>();
            }
            registerPlayerCameraToServer();
        }
    }

    private void registerPlayerCameraToServer()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        if (PhotonNetwork.IsMasterClient )
        {
            Debug.Log("MC local register");
            SetPlayerCamera(PhotonNetwork.LocalPlayer.NickName, photonView.ViewID);
        }
        else
        {
            Debug.Log("Local player server register");
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("SetPlayerCamera", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, photonView.ViewID);
            }
        }
    }

    [PunRPC]
    private void SetPlayerCamera(string pname, int camera_networkid)
    {
        Debug.Log("Players call set cam");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("SetPlayerCamera");
            Debug.Log("RPC Player Name" + pname);
            Debug.Log("RPC Player camera_networkid" + camera_networkid);
            Debug.Log("RPC Player on"+this.gameObject.name);
            //ServerCamera server = transform.GetComponent<ServerCamera>();
            Debug.Log("RPC Player on" + server);
            server.AddServerCamButton(pname, camera_networkid);
        }
        else
        {
            Debug.LogError("Cant SetPlayerCamera");
        }
    }


    private void disableAllComponents()
    {
        cam.enabled = false;
        myaudio.enabled = false;
        MonoBehaviour[] components = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour c in components)
        {
            if (c is PhotonTransformView)
            {
                c.enabled = true;
            }
            else
            {
                c.enabled = false;
            }
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
    }
}
