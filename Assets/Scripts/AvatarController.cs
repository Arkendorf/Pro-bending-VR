using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Transform controller;
    // Update is called once per frame
    void Update()
    {

        transform.position = controller.position + Vector3.down;
        transform.rotation = controller.rotation;
    }
}
