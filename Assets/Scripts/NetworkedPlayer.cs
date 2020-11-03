using System.Collections;
using UnityEngine;
using Photon.Pun;
  

public class NetworkedPlayer : MonoBehaviourPun, Photon.Pun.IPunObservable
{
    // test
    // avatar parts
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject speaker;
    public TMPro.TMP_Text nickName;
    public GameObject nameCanvas;

    // avatar models (TO COLOR)
    public GameObject headModel;
    public GameObject leftHandModel; // right hand and left hand using same material right now
    public GameObject rightHandModel; // right hand and left hand using same material right now

    // self hands (TO COLOR)
    public Material handsMat;

    // tracking transforms
    public Transform playerHeadLocal;
    public Transform playerLeftHandLocal;
    public Transform playerRightHandLocal;

    //public Color color;

    public GameObject bulletPrefab;
    //public BulletScript bulletScript;
    void Start ()
    {
        Debug.Log("i'm instantiated");

        if (photonView.IsMine)
        {
            Debug.Log("player is mine");

            Transform playerGlobal = GameObject.Find("MultiplayerSetup/OVRPlayerController").transform;
            playerHeadLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            playerLeftHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
            playerRightHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");


            this.transform.position = Vector3.zero;
            this.transform.localPosition = Vector3.zero;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            this.transform.localRotation = Quaternion.Euler(Vector3.zero);

            // hide my own avatar to myself
        
            head.SetActive(false);
            leftHand.SetActive(false);
            rightHand.SetActive(false);
            nameCanvas.SetActive(false);
            //speaker.SetActive(false);

            //color = Random.ColorHSV(0,1f,0.5f,1f, 0.5f, 1f, 1f, 1f);
            //handsMat.SetColor("_BaseColor", color);


            // This is where we set InputManager variable to my Player so that inputManager can affect things here all it does right now is change nickName text
            GameObject.Find("InputManager").GetComponent<InputManager>().myPlayer = this;
        }
    }

    public void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Bullet", playerLeftHandLocal.position, playerLeftHandLocal.rotation, 0);
        bullet.GetComponent<PhotonView>().RPC("setVelocityPUN", RpcTarget.All, playerLeftHandLocal.forward * 10);
    }


    public void Update(){
        if (photonView.IsMine){ // is this line necessary? i think so right?
             if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.Touch))
                {
                    Shoot();
                }
        }
    }

    

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHeadLocal.position);
            stream.SendNext(playerHeadLocal.rotation);

            stream.SendNext(playerLeftHandLocal.position);
            stream.SendNext(playerLeftHandLocal.rotation);

            stream.SendNext(playerRightHandLocal.position);
            stream.SendNext(playerRightHandLocal.rotation);

            //stream.SendNext(new Vector3(color.r, color.g, color.b));
            stream.SendNext(nickName.text);
        }
        else
        {
            
            head.transform.localPosition = (Vector3)stream.ReceiveNext();
            head.transform.localRotation = (Quaternion)stream.ReceiveNext();

            leftHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            leftHand.transform.localRotation = (Quaternion)stream.ReceiveNext();

            rightHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            rightHand.transform.localRotation = (Quaternion)stream.ReceiveNext();

            //Vector3 temp = (Vector3)stream.ReceiveNext();
            string tempname = (string)stream.ReceiveNext();

            if (!photonView.IsMine)
            {
                //color = new Color(temp.x, temp.y, temp.z, 1);
                //UpdateModelColors();

                nickName.text = tempname;
            }
        }
    }

    /**
    private void UpdateModelColors()
    {
        Material newMat = new Material(headModel.GetComponent<Renderer>().material);
        newMat.SetColor("_BaseColor", color);

        headModel.GetComponent<Renderer>().material = newMat;

        leftHandModel.GetComponent<Renderer>().material = newMat;
        rightHandModel.GetComponent<Renderer>().material = newMat;
    }
    **/
}
