using System.Collections;
using UnityEngine;
using Photon.Pun;
  

public class NetworkedPlayer : MonoBehaviourPun, Photon.Pun.IPunObservable
{
    // test
    // avatar parts
    public GameObject avatar;
    public GameObject localAvatar;
    [Space(10)]
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject controller;
    [Space(10)]
    public GameObject speaker;
    [Space(10)]
    public TMPro.TMP_Text nickName;
    public GameObject nameCanvas;
    [Space(10)]
    public Material blueTeamMaterial;
    public string blueTeamElement = "BlueFire";
    [Space(10)]
    public Material redTeamMaterial;
    public string redTeamElement = "Fire";
    [Space(10)]
    public Material spectatorMaterial;

    // avatar models (TO COLOR)
    //public GameObject headModel;
    //public GameObject leftHandModel; // right hand and left hand using same material right now
    //public GameObject rightHandModel; // right hand and left hand using same material right now

    // self hands (TO COLOR)
    //public Material handsMat;

    // Get the lerp scripts for all moving objects
    [Space(10)]
    public TransformLerp headTransformLerp;
    public TransformLerp leftHandTransformLerp;
    public TransformLerp rightHandTransformLerp;
    public TransformLerp controllerTransformLerp;

    // tracking transforms
    [HideInInspector] public Transform playerControllerLocal;
    [HideInInspector] public Transform playerHeadLocal;
    [HideInInspector] public Transform playerLeftHandLocal;
    [HideInInspector] public Transform playerRightHandLocal;

    

    void Start ()
    {
        Debug.Log("i'm instantiated");
        // Grab my team number!
        int teamNumber = (int)photonView.InstantiationData[0];
        // Decide on avatar model based on color
        if (photonView.IsMine)
        {
            Debug.Log("player is mine");

            // Get necessary parts of the player controller
            playerControllerLocal = GameObject.Find("MultiplayerSetup/OVRPlayerController").transform;

            if (teamNumber == 1 || teamNumber == 0) {
                // TODO: Set this to false
                GameObject.Find("MultiplayerSetup/OVRPlayerController").GetComponent<OVRPlayerController>().EnableLinearMovement = false;
                GameObject.Find("MultiplayerSetup/OVRPlayerController").GetComponent<OVRPlayerController>().EnableRotation = false;
            }

            playerHeadLocal = playerControllerLocal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            playerLeftHandLocal = playerControllerLocal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
            playerRightHandLocal = playerControllerLocal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");

            // Set up IK
            IKControl ikControl = localAvatar.GetComponentInChildren<IKControl>();
            ikControl.headTarget = playerHeadLocal;
            ikControl.leftHandTarget = playerLeftHandLocal;
            ikControl.rightHandTarget = playerRightHandLocal;
            localAvatar.GetComponent<AvatarController>().controller = playerControllerLocal;

            // Reset position
            transform.position = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            // hide my own avatar to myself            
            localAvatar.SetActive(true);
            avatar.SetActive(false);

            // Deactivate name canvas
            nameCanvas.SetActive(false);

            // This is where we set InputManager variable to my Player so that inputManager can affect things here all it does right now is change nickName text
            GameObject.Find("InputManager").GetComponent<InputManager>().myPlayer = this;               
        }
        else
        {
            localAvatar.SetActive(false);
            avatar.SetActive(true);

            headTransformLerp = head.GetComponent<TransformLerp>();
            leftHandTransformLerp = leftHand.GetComponent<TransformLerp>();
            rightHandTransformLerp = rightHand.GetComponent<TransformLerp>();
            controllerTransformLerp = controller.GetComponent<TransformLerp>();
        }


        // Set element and material
        NetworkedBender[] networkedBenders = GetComponents<NetworkedBender>();
        if (teamNumber == 0)
        {
            GetComponentInChildren<Renderer>().material = redTeamMaterial;
            foreach (NetworkedBender networkedBender in networkedBenders)
            {
                networkedBender.element = redTeamElement;
            }
        }
        else if (teamNumber == 1)
        {
            GetComponentInChildren<Renderer>().material = blueTeamMaterial;
            foreach (NetworkedBender networkedBender in networkedBenders)
            {
                networkedBender.element = blueTeamElement;
            }
        }
        else
        {
            GetComponentInChildren<Renderer>().material = spectatorMaterial;
            // Disable bending
            foreach (NetworkedBender networkedBender in networkedBenders)
            {
                networkedBender.enabled = false;
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

            stream.SendNext(playerControllerLocal.position);
            stream.SendNext(playerControllerLocal.rotation);

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

            if (controllerTransformLerp)
            {
                controllerTransformLerp.UpdateTransform((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext());
            }
            else
            {
                controller.transform.localPosition = (Vector3)stream.ReceiveNext();
                controller.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }

            string tempname = (string)stream.ReceiveNext();

            if (!photonView.IsMine)
            {
                nickName.text = tempname;
            }
        }
    }
}
