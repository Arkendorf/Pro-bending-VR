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

    // Get the lerp scripts for all moving objects
    public TransformLerp headTransformLerp;
    public TransformLerp leftHandTransformLerp;
    public TransformLerp rightHandTransformLerp;

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


            // This is where we set InputManager variable to my Player so that inputManager can affect things here all it does right now is change nickName text
            GameObject.Find("InputManager").GetComponent<InputManager>().myPlayer = this;
        }
        else
        {
            headTransformLerp = head.GetComponent<TransformLerp>();
            leftHandTransformLerp = leftHand.GetComponent<TransformLerp>();
            rightHandTransformLerp = rightHand.GetComponent<TransformLerp>();
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

            stream.SendNext(nickName.text);
        }
        else
        {
            if (headTransformLerp) {
                headTransformLerp.UpdateTransform((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            }
            else {
                head.transform.localPosition = (Vector3)stream.ReceiveNext();
                head.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }

            if (leftHandTransformLerp) {
                leftHandTransformLerp.UpdateTransform((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            }
            else {
                leftHand.transform.localPosition = (Vector3)stream.ReceiveNext();
                leftHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }

            if (rightHandTransformLerp) {
                rightHandTransformLerp.UpdateTransform((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            }
            else {
                rightHand.transform.localPosition = (Vector3)stream.ReceiveNext();
                rightHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }

            string tempname = (string)stream.ReceiveNext();

            if (!photonView.IsMine)
            {
                nickName.text = tempname;
            }
        }
    }
}
