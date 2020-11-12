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


        // Update the scoreboard.. does not work here.. Not positive why... 
        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        //gameManager.RedScoreDisplay.SetText("Help");
        int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());
        
    }
    }

    // Update is called once per frame
    void Update()
    {
        //gameManager.RedScoreDisplay.SetText("Help");
     
        
    }

    public void LocalPlayerHit(int health){
        // player that just got hit updates their own health
        this.health = health;


        // This player should update the customProperties for everyone else. 
        Hashtable hashProperties = new Hashtable();


        int numRedPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
        int numBluePlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];

            // Not sure of the best way to do this but we need to setup the score. If this ends up being the first player to connect we need to setup new entries. Seems a little inefficient considering score will never actually change

        int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];

        if (this.teamNumber == 0){
            // The problem is that we can only use setCustomProperties I beleive, Cant just directly manipulate the hashtable so we have to do this funkyness
            // 
            redScore--;
           
            

        }else if (this.teamNumber ==1){
            blueScore--;
            
            

        }

        hashProperties.Add("numRedPlayers", numRedPlayers);
        hashProperties.Add("numBluePlayers", numBluePlayers);
        hashProperties.Add("redScore", redScore);
        hashProperties.Add("blueScore", blueScore);
        // Reset the room properities to update the score
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashProperties);


        //Update the scoreboard for yourself
    
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());



        // Everyone else gets this called
        GetComponent<PhotonView>().RPC("PlayerHit", RpcTarget.Others, health, redScore, blueScore);
        Debug.Log(health);
        
         
    }
    [PunRPC]
    public void PlayerHit(int health, int redScore, int blueScore){
        // Everyone else sets the health of their copy of this networked health to the new health
        this.health = health;
        // Everyone else hears this audio
        audio.Play();

        // Everyone else should update their scoreboards too
        //int redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        //int blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());


    }



    


    

    
}
