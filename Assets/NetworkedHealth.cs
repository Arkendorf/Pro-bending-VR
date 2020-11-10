using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedHealth : MonoBehaviourPun
{
    public int health =10;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
    if(photonView.IsMine){
     GetComponent<NetworkedPlayer>().playerHeadLocal.GetComponent<Hit>().networkedHealth = this;   

    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LocalPlayerHit(int health){
        this.health = health;
        // Everyone else gets this called
        GetComponent<PhotonView>().RPC("PlayerHit", RpcTarget.Others, health);
        Debug.Log(health);
        
        
    }
    [PunRPC]
    public void PlayerHit(int health){
        this.health = health;
        audio.Play();




    }
}
