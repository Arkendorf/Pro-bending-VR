using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
// Welcome to GameManager where the events of the game will Occur.
// The game Manager needs to have a photonView. Everyone in the game will have a copy of this script but certain values WILL NEVER be changed unless through a RPC to all players so that copies of this stay consisten

public class GameManager : MonoBehaviourPun
{

    // We will have a local variable that no one else needs to know called team. Team 0 is Blue, Team 1 is Red, Team 2 is spectator
    [HideInInspector]
    public int Team;


    // We will have networked variables that hold track of how many people are currently on Team Blue and Team Red
    [HideInInspector] public int RedCount;
    [HideInInspector] public int BlueCount;

    // Keep track of the score
    [HideInInspector] public int RedScore;
    //[HideInInspector] public int BlueScore;

    // We need reference to this versions networkedHealth. We want to be able to get the health from here so we can update our scoreboard. 
    [HideInInspector] public NetworkedHealth networkedHealth;
    // Speaking of scoreboard lets grab two references to text to update the red and blue score
    public TextMeshProUGUI RedScoreDisplay;
    public TextMeshProUGUI BlueScoreDisplay;
    public TextMeshProUGUI RedScoreDisplay2;
    public TextMeshProUGUI BlueScoreDisplay2;

    public TextMeshProUGUI RedText1;
    public TextMeshProUGUI RedText2;
    public TextMeshProUGUI BlueText1;
    public TextMeshProUGUI BlueText2;


    public TextMeshProUGUI win1Display;
    public TextMeshProUGUI win2Display;
    


    private bool button1Hit = false;
    private bool button2Hit = false;

    

    public Animator wallDown;

    public AudioSource playersReadyAudio;
    public AudioSource blueReadyAudio;
    public AudioSource redReadyAudio;
    public AudioSource wallRumble;

    public AudioSource buttonClick;

    


    // Start is called before the first frame update
    void Start()
    {
        win1Display.enabled = false;
        win2Display.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       

        
        
    }

    public void StartButtonHit(int buttonId){
        
        
        if (!this.button1Hit && buttonId ==1){
            this.button1Hit = true;
            buttonClick.Play();
            bool allReady = PreemptiveCheck();
            if (!allReady){
                GetComponent<PhotonView>().RPC("RedReady", RpcTarget.All);
            }
            networkedHealth.changeProperties("readyToPlay", 1);
            
            
         } 
         
         
        if (!this.button2Hit && buttonId ==2){
            this.button2Hit = true;
            buttonClick.Play();
            bool allReady = PreemptiveCheck();
            if (!allReady){
            GetComponent<PhotonView>().RPC("BlueReady", RpcTarget.All);
            }
            networkedHealth.changeProperties("readyToPlay", 1);
            
            


        }
        
        


        // Takes too long to call changeProperties.. Will Thread...  by the time it gets here the value will not have been updated...
        // Need to make preemptive checks



    }

    public bool PreemptiveCheck(){
        int ready = networkedHealth.GetValue("readyToPlay");
        
          if (ready >= 1 ){
            // start the animation here. Drop the Wall down. 
            
           GetComponent<PhotonView>().RPC("WallDown", RpcTarget.All);
           return true;
           
        }

        return false;

    }

    // adds the value to the value associated with key in Properties
    

    [PunRPC]
    public void WallDown(){
        wallDown.SetBool("PlayersReady", true);
        playersReadyAudio.Play();
        wallRumble.Play();
        
        StartCoroutine(timeout());

    }

    IEnumerator timeout()
    {
        float t = 6;
        
        while (t>0){
            t-=Time.deltaTime;
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
            yield return null; 
        }
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        
    }

    [PunRPC]
    public void BlueReady(){
        blueReadyAudio.Play();
    }

    [PunRPC]
    public void RedReady(){
        redReadyAudio.Play();
    }

    
    
    
}
