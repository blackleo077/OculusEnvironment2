using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(Rigidbody))]
public class XRGrabObject : MonoBehaviour
{
    private bool Grabbing;
    private bool Colliding;
    private bool NetworkObject;
    private Rigidbody rb;
    private PhotonView pv;
    private XRGrabber Hand;
    private OwnerSign mySign;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out Rigidbody rigidbody);
        rb = rigidbody;
        NetworkObject = TryGetComponent<PhotonView>(out PhotonView pv);
        this.pv = pv;
    }

    private void Start()
    {
        if (isNetworkObject)
        {
            if (pv.IsMine)
            {
                rb.isKinematic = true;
                InitOwnerSign();
            }
        }
    }


    private void InitOwnerSign()
    {
        GameObject sign = Instantiate(Resources.Load("OwnerSign", typeof(GameObject))) as GameObject;
        sign.TryGetComponent(out mySign);
        mySign.Init(this.transform);
    }

    private void OnDestroy()
    {
        Destroy(mySign.gameObject);
    }

    private void SetSignActive(bool active) { mySign.gameObject.SetActive(active); }

    public bool isMyNetworkObject { get { return pv.IsMine; } }

    public Rigidbody m_rigidbody { get { return rb; } }

    public bool isGrabbing { get { return Grabbing; } }

    public bool isColliding { get { return Colliding; } }

    public bool isNetworkObject { get { return NetworkObject; } }

    public XRGrabber HandInteracting { get { return Hand; } set { Hand = value; } }

    public void Grab()
    {
        Grabbing = true;
        rb.isKinematic = false;
        SetSignActive(false);
    }

    public void Release()
    {
        Grabbing = false;
        rb.isKinematic = true;
        SetSignActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        Colliding = false;
    }

}
