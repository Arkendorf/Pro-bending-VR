using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedGrip : MonoBehaviourPun
{
    public OVRInput.Button leftGripButton;
    public OVRInput.Button rightGripButton;
    [Space(10)]
    public Animator animator;
    public Animator localAnimator;

    private PhotonView photonView;
    private bool leftGripping;
    private bool rightGripping;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(leftGripButton) != leftGripping)
            {
                leftGripping = OVRInput.Get(leftGripButton);
                photonView.RPC("SetAnimatorBool", RpcTarget.All, "LeftHandClosed", leftGripping);
            }

            if (OVRInput.Get(rightGripButton) != rightGripping)
            {
                rightGripping = OVRInput.Get(rightGripButton);
                photonView.RPC("SetAnimatorBool", RpcTarget.All, "RightHandClosed", rightGripping);
            }
        }      
    }

    [PunRPC]
    void SetAnimatorBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
        localAnimator.SetBool(boolName, value);
    }
}
