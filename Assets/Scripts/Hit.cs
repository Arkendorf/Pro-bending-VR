using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Hit : MonoBehaviourPun
{
    
    [HideInInspector]
    public NetworkedHealth networkedHealth;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        // The local OVR Controller has been hit. We should do something for just the player and then call a RPC
        if(other.gameObject.tag != "mine" && other.gameObject.layer == LayerMask.NameToLayer("Missile")){
            // Do something. 
            audio.Play();
            networkedHealth.LocalPlayerHit(networkedHealth.health - 1);
            other.gameObject.GetComponent<PhotonView>().RPC("Collide", RpcTarget.All);
            // More local Stuff 

        }
    }
}
