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
        // just here for testing
        //UpdateScoreBoard();
     
        
    }

    public void LocalPlayerHit(int health){
        // player that just got hit updates their own health
        
        this.health = health;

        Hashtable hash = new Hashtable();

        int numRedPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
        int numBluePlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];  
        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        int ReadyToPlay = (int)PhotonNetwork.CurrentRoom.CustomProperties["readyToPlay"];
        




        
      

        if (this.teamNumber == 0){
           //changeProperties("redScore", -1);
           redScore--;
        }else if (this.teamNumber ==1){
            //changeProperties("blueScore", -1);
            blueScore--;
            
        }



        hash.Add("numRedPlayers", numRedPlayers);
        hash.Add("numBluePlayers", numBluePlayers);
        hash.Add("redScore", redScore);
        hash.Add("blueScore", blueScore);
        hash.Add("readyToPlay", ReadyToPlay);
        

        
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);


        //Update the scoreboard for yourself
        
        //UpdateScoreBoard();
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());


        // Everyone else gets this called
        GetComponent<PhotonView>().RPC("PlayerHit", RpcTarget.Others, health, redScore, blueScore);
        Debug.Log(health);
        
        
         
    }

    public void UpdateScoreBoard(){
        
        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
       int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        
        
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());
        
       

    }
    [PunRPC]
    public void PlayerHit(int health, int redScore, int blueScore){
        // Everyone else sets the health of their copy of this networked health to the new health
        this.health = health;
        // Everyone else hears this audio
        audio.Play();

        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());
        // Everyone else should update their scoreboards too
        //UpdateScoreBoard();



    }
    
    public void changeProperties(string key, int value){
        // is this necessary? not sure but i know every person has a local copy of gamemanager
        //if (photonView.IsMine){
        Hashtable hash = new Hashtable();
        // Have to rebuild our hash table unfortunately 
        int numRedPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
        int numBluePlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];  
        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        int ReadyToPlay = (int)PhotonNetwork.CurrentRoom.CustomProperties["readyToPlay"];
        


        hash.Add("numRedPlayers", numRedPlayers);
        hash.Add("numBluePlayers", numBluePlayers);
        hash.Add("redScore", redScore);
        hash.Add("blueScore", blueScore);
        hash.Add("readyToPlay", ReadyToPlay);
        

        
        hash[key] = (int)hash[key] + value;
        


        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);



    }
    

    public int GetValue(string key){
        return (int)PhotonNetwork.CurrentRoom.CustomProperties[key];
    }
}



    
