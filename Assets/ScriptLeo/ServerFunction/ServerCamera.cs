using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;

public class ServerCamera : MonoBehaviour
{
    public static ServerCamera instance;

    
    [SerializeField]
    Camera CaptureCam, LiveCam;
    RenderTexture myrenderTexture;
    Texture2D myTexture2D;

    [SerializeField]
    Dictionary<string, Camera> playersCamera = new Dictionary<string, Camera>();
    public GameObject CamButtonPrefab;
    public Transform CamButtonParentTransform;
    UnityEvent SwitchCameraButton;

    bool Capturing;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        myrenderTexture = CaptureCam.targetTexture;
        LiveCam = this.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddServerCamButton(string pname, int camera_networkid)
    {

        GameObject cambtobj = Instantiate(CamButtonPrefab, CamButtonParentTransform);
        Text buttonText = cambtobj.GetComponentInChildren<Text>();
        buttonText.text = pname;
        Debug.Log(camera_networkid+":"+PhotonNetwork.GetPhotonView(camera_networkid).transform.GetChild(0).name);
        Camera refCamera =  PhotonNetwork.GetPhotonView(camera_networkid).transform.GetComponentInChildren<Camera>();
        Debug.Log(refCamera.name);
        playersCamera.Add(name, refCamera);

        Button cambt = cambtobj.GetComponentInChildren<Button>();
        cambt.onClick.AddListener(()=> ActiveCamea(pname) );

    }

    public void ActiveCamea(string pname)
    {
        if (playersCamera.ContainsKey(pname))
        {
            Camera nextcam = playersCamera[pname];
            Debug.Log(nextcam.name);
            if (nextcam != LiveCam)
            {
                LiveCam.enabled = false;
                LiveCam = nextcam;
                LiveCam.enabled = true;
            }
            else
            {
                Debug.LogError("Same camera, no change");
            }
        }
        else
        {
            Debug.LogError("No exist camera key.");
        }
    }

    public void CaptureImage()
    {
        if (Capturing) return;
        Capturing = true;
        Texture2D captureImage = SaveImage();
        SaveImageTo("",captureImage.EncodeToPNG());
    }

    private void debug_json()
    {

    }

    public void OnCaptureImageEnd()
    {

        Capturing = false;
    }

    private Texture2D SaveImage()
    {
        Texture2D rendertextureimage = new Texture2D(myrenderTexture.width, myrenderTexture.height,TextureFormat.ARGB32,false);
        RenderTexture mRt = new RenderTexture(myrenderTexture.width, myrenderTexture.height, myrenderTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

        CaptureCam.targetTexture = mRt;
        CaptureCam.Render();
        RenderTexture.active = myrenderTexture;

        rendertextureimage.ReadPixels(new Rect(0, 0, myrenderTexture.width, myrenderTexture.height), 0, 0);
        rendertextureimage.Apply();

        myTexture2D = rendertextureimage;
        myTexture2D.Apply();
        CaptureCam.targetTexture = myrenderTexture;
        CaptureCam.Render();
        RenderTexture.active = myrenderTexture;

        DestroyImmediate(rendertextureimage);
        DestroyImmediate(mRt);
        return myTexture2D;
    }

    private void SaveImageTo(string path, byte[] bytes)
    {
        string filename = GetFileName();
        path = Path.Combine(Application.persistentDataPath, "ServerCapture");
        path = Path.Combine(path, filename);
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        Debug.Log("Save file to:" +path);
        File.WriteAllBytes(path, bytes);
        OnCaptureImageEnd();
    }

    private string GetFileName()
    {
        return
            System.DateTime.Now.Year + "" +
            System.DateTime.Now.Month + "" +
            System.DateTime.Now.Day + "_" +
            System.DateTime.Now.Hour + "" +
            System.DateTime.Now.Minute + "" +
            System.DateTime.Now.Second+".png";

    }
}
