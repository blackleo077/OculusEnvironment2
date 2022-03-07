using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using MLAPI;
//using MLAPI.Messaging;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class WhiteBoardNetwork : MonoBehaviourPun
{
    private WhiteBoard whiteBoard;
    public Texture2D WhiteBoardTexture;

    private int RPCDataSize = 500;
    byte[] textureData;

    private PhotonView PV;

    public enum networkType
    {
        MLAPI,
        Photon
    }

    public struct DrawData{
        public Vector2 LastPos;
        public Vector2 CurrentPos;
        public Color color;
        public int size;
        public int bid;
        public ulong cid;

        public DrawData(Vector2 LastPos, Vector2 CurrentPos, Color color, int size, int bid, ulong cid = default(ulong))
        {
            this.LastPos = LastPos;
            this.CurrentPos = CurrentPos;
            this.color = color;
            this.size = size;
            this.bid = bid;
            this.cid = cid;
        }
    }

    public void Init(Texture2D texture, networkType net)
    {

        whiteBoard = GetComponent<WhiteBoard>();
        WhiteBoardTexture = texture;
        switch (net)
        {
            case networkType.MLAPI:
             //   NetworkManager.Singleton.OnServerStarted += ServerInit;
              //  NetworkManager.Singleton.OnClientConnectedCallback += ClientInit;
                break;
            case networkType.Photon:
                PV = GetComponent<PhotonView>();
                break;
                   
        }
    }

    private void ServerInit()
    {
        Debug.LogError("Server Init");
        Application.targetFrameRate = 30;
    }

    private void ClientInit(ulong cid)
    {
       // if (!IsClient) return;
        Debug.LogError("Client GetServerWhiteBoardTextureServerRpc");
      //  GetServerWhiteBoardTextureServerRpc(NetworkManager.Singleton.LocalClientId);
    }


 //   [ServerRpc(RequireOwnership = false)]
    public void GetServerWhiteBoardTextureServerRpc(ulong cid)
    {
        Debug.LogError("GetServerWhiteBoardTextureServerRpc:" + cid);

        WhiteBoardTexture.Apply();
        byte[] textureColorArray = WhiteBoardTexture.EncodeToPNG();
        WhiteBoardTexture.LoadImage(textureColorArray);
        WhiteBoardTexture.Apply();
       // StartCoroutine( DelaySendTextureData(textureColorArray, cid));
        
    }
    /*
    IEnumerator DelaySendTextureData(byte[] textureColorArray, ulong cid)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { cid }
            }
        };

        IntiWhiteBoardTextureDataSizeClientRpc(textureColorArray.Length, clientRpcParams);
        yield return null;
        int totalpack = textureColorArray.Length / RPCDataSize;
        int remain = textureColorArray.Length % RPCDataSize;
        for (int PackageID = 0; PackageID <= textureColorArray.Length / RPCDataSize; PackageID++)
        {
            int startReadingIndex = PackageID * RPCDataSize;
            byte[] SentPackage;
            if (PackageID != totalpack)
            {
                SentPackage = new byte[RPCDataSize];
                System.Array.Copy(textureColorArray, startReadingIndex, SentPackage, 0, RPCDataSize);
            }
            else
            {
                SentPackage = new byte[remain];
                System.Array.Copy(textureColorArray, startReadingIndex, SentPackage, 0, remain);
            }
            SendWhiteBoardTextureDataClientRpc(PackageID*RPCDataSize, SentPackage, clientRpcParams);
            yield return null;
        }
        yield return null;
        ReformWhiteBoardTextureDataClientRpc(clientRpcParams);
        yield return null;
    }


    [ClientRpc]
    private void IntiWhiteBoardTextureDataSizeClientRpc(int texturedatasize, ClientRpcParams clientRpcParams = default)
    {
        Debug.LogError("IntiWhiteBoardTextureDataSizeClientRpc");
        textureData = new byte[texturedatasize];
    }

    [ClientRpc]
    private void SendWhiteBoardTextureDataClientRpc(int startPos, byte[] data, ClientRpcParams clientRpcParams = default)
    {
        System.Array.Copy(data, 0, textureData, startPos, data.Length);
    }

    [ClientRpc]
    private void ReformWhiteBoardTextureDataClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.LogError("ReformWhiteBoardTextureDataClientRpc");
        WhiteBoardTexture.LoadImage(textureData);
        WhiteBoardTexture.Apply();
    }


    */



    //Drawing network
    //[ServerRpc (RequireOwnership =false)]
    //public void DrawTextureToServerRpc(Vector2 LastPos, Vector2 CurrentPos, Color color, int size, int bid, ulong cid)
    public void DrawTextureToServerRpc(DrawData dd)
    {
        if (!PhotonNetwork.IsConnected)
            return;
        /*
        List<ulong> clientList = NetworkManager.Singleton.ConnectedClients.Keys.ToList();
        clientList.Remove(cid);
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = clientList.ToArray()
            }
        };
        if (clientRpcParams.Send.TargetClientIds.Length > 0)
        {
            string msg = "Send to Clientid :";
            foreach(ulong u in clientRpcParams.Send.TargetClientIds)
            {
                msg += u+",";
            }
            Debug.LogError(msg);
            
            DrawTextureToClientRpc(LastPos, CurrentPos, color, size, bid, clientRpcParams);
        }
        */
          
        PV.RPC("DrawTextureToLocal", RpcTarget.OthersBuffered, dd.LastPos,dd.CurrentPos,dd.color ,dd.size, dd.bid);
            /*
        if (cid != NetworkManager.Singleton.LocalClientId)
            DrawTextureToLocal(LastPos, CurrentPos, color, size, bid);
            */
    }

   // [ClientRpc]
    private void DrawTextureToClientRpc(DrawData dd)
    {
        Debug.LogError("Draw By Other Clients");
        //DrawTextureToLocal(dd);
    }

    [PunRPC]
    private void DrawTextureToLocal(Vector2 lastpos, Vector2 currentpos,Color color, int size, int bid)
    {
        whiteBoard.DrawOnBoard(lastpos, currentpos, color, size, bid); 
    }

    public void LoadTextureToWhiteBoardServerRpc(string texturename)
    {
        PV.RPC("SetTextureToLocal", RpcTarget.OthersBuffered, texturename);
    }

    [PunRPC]
    private void SetTextureToLocal(string texturename)
    {
        Debug.Log("SetTextureToLocal");
        whiteBoard.SetWhiteBoardTexture(texturename);
    }




  //  [ServerRpc]
    public void TestSendTimeServerRpc(int msg, float time)
    {
        DisplayTestSendTimeClientRpc(msg, time);
    }

  //  [ClientRpc]
    private void DisplayTestSendTimeClientRpc(int msg, float time)
    {

    }
}
