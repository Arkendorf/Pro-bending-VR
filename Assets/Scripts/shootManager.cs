using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class shootManager : MonoBehaviourPun
{
    public GameObject ball;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot(){

        photonView.RPC("RpcShoot", RpcTarget.All, transform.forward*20);

    }

    [PunRPC]
    void RpcShoot(Vector3 vel)
    {
        // Create a bullet and set its velocity. Initial position is based of the transform of the player
        GameObject bullet = Instantiate(ball, transform.position, transform.rotation);
        Rigidbody br = bullet.GetComponent<Rigidbody>();
        br.velocity = vel;


        // after 20 seconds we can destroy the bullet
        Destroy (bullet, 20);

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // if left trigger is clicked call shoot
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.Touch))
                {
                    Shoot();
                }
        }
    }
}
