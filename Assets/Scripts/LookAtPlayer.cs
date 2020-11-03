using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform playerHead;
    public GameObject avatarHead;
    // Start is called before the first frame update
    void Start()
    {
        Transform playerGlobal = GameObject.Find("OVRPlayerController").transform;
        playerHead = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = avatarHead.transform.position + 0.4f * Vector3.up;
        this.transform.LookAt(playerHead);
    }
}
