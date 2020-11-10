using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class NetworkedHealth : MonoBehaviourPun
{
    public int health =10;
    public AudioSource audio;


    


    // Grab the GameManager
    public GameManager gameManager;

    // 
    public int BlueScore;

    // Start is called before the first frame update
    void Start()
    {
    if(photonView.IsMine){
        GetComponent<NetworkedPlayer>().playerHeadLocal.GetComponent<Hit>().networkedHealth = this;   

        gameManager = GameObject.Find("MultiplayerSetup/GameManager").GetComponent<GameManager>();
        GameObject.Find("MultiplayerSetup/NetworkController").GetComponent<NetworkController>().networkedHealth = this;
        GameObject.Find("MultiplayerSetup/GameManager").GetComponent<GameManager>().networkedHealth = this;
        
    }
    }

    // Update is called once per frame
    void Update()
    {
        int RedCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedCount"];
        gameManager.BlueScoreDisplay.SetText(RedCount.ToString());

        
        //gameManager.BlueScoreDisplay.SetText("Test");
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
