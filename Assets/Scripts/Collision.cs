using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collision : MonoBehaviourPun
{
    public string collideEffect;

    private Missile missile;
    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Missile>();
    }


    

    [PunRPC]
    public void Collide(){
            // TODO: fix instantiation of puff effect
            //PhotonNetwork.Instantiate(collideEffect, transform.position, Quaternion.LookRotation(-missile.velocity));
            // Destroy the missile on all clients
        if(photonView.IsMine){
            PhotonNetwork.Destroy(gameObject);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
           // Don't collide with the player who instantiated this missile
           // Missile if we instantiated it will be tagged mine
           // Player controller on the ovr will be tagged playerController, and centerEyeAnchor where we currently have the collider box will be tagged MainCamera
        if (gameObject.tag!="mine" || (other.tag != "playerController" && other.tag != "MainCamera") ){
            GetComponent<PhotonView>().RPC("Collide", RpcTarget.All);

        }   
    }
}
