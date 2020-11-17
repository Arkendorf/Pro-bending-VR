using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class NetworkedHealth : MonoBehaviourPun
{
    public int health =10;
    public AudioSource hitOtherSound;

    public AudioSource blueWinsAudio;
    public GameObject blueFireWorks;

    public AudioSource redWinsAudio;
    public GameObject redFireWorks;


    public AudioSource redLosing;
    public AudioSource blueLosing;
    


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
           redScore--;
        }else if (this.teamNumber ==1){
            blueScore--;
            
        }
        if (!(redScore < 0 || blueScore < 0)){

            if (redScore == 2 && this.teamNumber == 0 && blueScore > 0) {
                GetComponent<PhotonView>().RPC("RedLosing", RpcTarget.All);
            }

            if (blueScore == 2 && this.teamNumber == 1 && redScore > 0) {
                GetComponent<PhotonView>().RPC("BlueLosing", RpcTarget.All);
            }
        hash.Add("redScore", redScore);
        hash.Add("blueScore", blueScore);
    
        

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);


        //Update the scoreboard for yourself
        
        UpdateScoreBoard(redScore, blueScore);
        


        // Everyone else gets this called
        GetComponent<PhotonView>().RPC("PlayerHit", RpcTarget.Others, health, redScore, blueScore);

        if (blueScore == 0 && this.teamNumber == 1 && redScore > 0){
            GetComponent<PhotonView>().RPC("RedWins", RpcTarget.All);

        }

        if (redScore == 0 && this.teamNumber == 0 && blueScore > 0){
            GetComponent<PhotonView>().RPC("BlueWins", RpcTarget.All);
        }
        
        }
        
        
         
    }

    public void UpdateScoreBoard(int redScore, int blueScore){    
        gameManager.RedScoreDisplay.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay.SetText(blueScore.ToString());
        gameManager.RedScoreDisplay2.SetText(redScore.ToString());
        gameManager.BlueScoreDisplay2.SetText(blueScore.ToString());
        

    }
    [PunRPC]
    public void PlayerHit(int health, int redScore, int blueScore){
        // Everyone else sets the health of their copy of this networked health to the new health
        this.health = health;
        // Everyone else hears this hitOtherSound
        hitOtherSound.Play();

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

    
    public void winDisplays(){
        gameManager.win1Display.enabled = true;
        gameManager.win2Display.enabled = true;
        gameManager.RedScoreDisplay.enabled = false;
        gameManager.BlueScoreDisplay.enabled = false;
        gameManager.RedScoreDisplay2.enabled = false;
        gameManager.BlueScoreDisplay2.enabled = false;
        gameManager.RedText1.enabled = false;
        gameManager.BlueText1.enabled = false;
        gameManager.RedText2.enabled = false;
        gameManager.BlueText2.enabled = false;


    }

    [PunRPC]
    public void RedWins(){
        redWinsAudio.Play();
        winDisplays();
        gameManager.win1Display.SetText("Red Team Wins!");
        gameManager.win2Display.SetText("Red Team Wins!");
        Instantiate(redFireWorks, new Vector3(0, -6.5f,0), Quaternion.identity);
    }

    [PunRPC]
    public void BlueWins(){
        blueWinsAudio.Play();
        winDisplays();
        gameManager.win1Display.SetText("Blue Team Wins!");
        gameManager.win2Display.SetText("Blue Team Wins!");
        Instantiate(blueFireWorks, new Vector3(0, -6.5f, 0), Quaternion.identity);
    }


    [PunRPC]
    public void RedLosing(){
        redLosing.Play();
    }

    [PunRPC]
    public void BlueLosing(){
        blueLosing.Play();
    }
}



    
