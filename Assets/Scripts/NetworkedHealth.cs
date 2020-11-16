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
    
    int teamNumber;
    // Start is called before the first frame update
    void Start()
    {
    gameManager = GameObject.Find("MultiplayerSetup/GameManager").GetComponent<GameManager>();
    
    
    if(photonView.IsMine){
        
        GetComponent<NetworkedPlayer>().playerHeadLocal.GetComponent<Hit>().networkedHealth = this;   
        
        
        GameObject.Find("MultiplayerSetup/NetworkController").GetComponent<NetworkController>().networkedHealth = this;
        GameObject.Find("MultiplayerSetup/GameManager").GetComponent<GameManager>().networkedHealth = this;
        


        //Grab my team number
        this.teamNumber = (int)photonView.InstantiationData[0];

        // Doesn't run for master client for some reason. 
        
        
        
    }
    }

    // Update is called once per frame
    void Update()
    {
        
     
        
    }

    public void LocalPlayerHit(int health){
        // player that just got hit updates their own health
        
        this.health = health;

        Hashtable hash = new Hashtable();
 
        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
       

        if (this.teamNumber == 0){
           //changeProperties("redScore", -1);
           redScore--;
        }else if (this.teamNumber ==1){
            //changeProperties("blueScore", -1);
            blueScore--;
            
        }

        hash.Add("redScore", redScore);
        hash.Add("blueScore", blueScore);
    
        

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);


        //Update the scoreboard for yourself
        
        UpdateScoreBoard(redScore, blueScore);
        


        // Everyone else gets this called
        GetComponent<PhotonView>().RPC("PlayerHit", RpcTarget.Others, health, redScore, blueScore);
        Debug.Log(health);
        
        
         
    }

    public void UpdateScoreBoard(int redScore, int blueScore){    
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());
        

    }
    [PunRPC]
    public void PlayerHit(int health, int redScore, int blueScore){
        // Everyone else sets the health of their copy of this networked health to the new health
        this.health = health;
        // Everyone else hears this audio
        audio.Play();

        UpdateScoreBoard(redScore, blueScore);
        



    }
    
    public void changeProperties(string key, int value){
        // is this necessary? not sure but i know every person has a local copy of gamemanager
        //if (photonView.IsMine){
        Hashtable hash = new Hashtable();
    
        hash[key] = (int)PhotonNetwork.CurrentRoom.CustomProperties[key] + value;

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);



    }
    

    public int GetValue(string key){
        return (int)PhotonNetwork.CurrentRoom.CustomProperties[key];
    }
}



    
