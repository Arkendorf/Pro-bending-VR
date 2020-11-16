using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // We want to change this to something because any call to PhotonNetwork.JoinOrCreateRoom will try to join this room name
    public string _room = "My_Custom_Room";
    private Vector3 EntryPoint;
    private Quaternion EntryRotation;
    public NetworkedHealth networkedHealth;
    
    // Define the red, blue, and spectator positions 
    static Vector3 redSpawn = new Vector3(8,-3.8f, 0);
    static Vector3 blueSpawn = new Vector3(-8,-3.8f,0);

    // Maybe we could add some randomness to the player spawn if it is a spectator just so everyone doesn't spawn on top of each other Not a priority
    static Vector3 spectatorSpawn = new Vector3(0,-3.8f,0);
    

    

    static Quaternion redRotation = Quaternion.Euler(0,-90,0);
    static Quaternion blueRotation = Quaternion.Euler(0,90,0);
    static Quaternion spectatorRotation = Quaternion.Euler(0,0,0);




    
    // Each spawn point index corresponds to the teams number
    Vector3[] spawnPoints = new Vector3[3] {redSpawn, blueSpawn, spectatorSpawn};
    Quaternion[] spawnRotations = new Quaternion[3]{redRotation, blueRotation, spectatorRotation};

    // Set the max on each team. We will start with 1 v 1
    int redMax = 1;
    int blueMax = 1;

        // Initialize the variables we need to setup teams

    Hashtable hashProperties = new Hashtable();
    int numRedPlayers = 0;
    int numBluePlayers = 0;


    int redScore = 10;
    int blueScore = 10;

    int ReadyToPlay = 0;

    

    GameObject MultiSetup;
    GameObject OVRplayer;
    GameObject Wall;
    //GameObject GameManager;
    Animator blackScreen;
     

    // Team number. Team number is 0 for red 1 for blue and 2 for spectator
    // Will be set and sent into instantation of player
    int teamNumber;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        this.blackScreen = GameObject.Find("MultiplayerSetup/OVRPlayerController/OVRCameraRig/TrackingSpace/CenterEyeAnchor/Blocker/Cube").GetComponent<Animator>();  
        this.Wall = GameObject.Find("StartWall");
    }

    void OnDestroy()
    {
        Debug.Log("Disconnecting");
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We're connected to the " + PhotonNetwork.CloudRegion + " server!");
        // Probably don't need to mess with RoomOptions but it can help set up timing to kick inactive players and can set a MaxPlayers. here's documentation if you really want
       //  https://doc-api.photonengine.com/en/pun/v1/class_room_options.html#a2d2471ba446949054e81362057d0d8ad 


        RoomOptions roomOptions = new RoomOptions();
        // This is how players join the same room. If a room has not been made it will be created with the two parameters RoomOptions and TypedLobby
        // If the room already exists (after the first person creates the room) any subsequent players will join the room and it will not recreate itself
        PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);


    }


// Is a MonoBehaviourPunCallBack called each time "LoadBalancingClient" Entered a room, no matter if this client created it or simply joined. 
// A similar method is OnPlayerEnteredRoom(Player newPlayer). Which works similarily but is called any time a remote player enters the room (so not the host)
// This is where we instantiate each player Prefab, currently the prefab is called "NetworkedPlayer" and is a prefab located in Assets/Resources/NetworkedPlayer.prefab
// Loads from resources folder. 

    public override void OnJoinedRoom()
    {
       // Each prefab that we instantiate must have a Photon View component. The photon view component will then have Observed Components
       // In the example of the player it has a photon view component with an observed component that is just the NetworkedPlayer Script 

       // Documentation is found here https://doc.photonengine.com/en-us/pun/current/gameplay/instantiation#photonnetwork_instantiate
       // Position and rotation of where to create the object must be set. Will be originally instatiated here even if moved already


       // Instantiate can take another parameter for custom instantiation Data. It takes an object[]
       // object[] myCustomInitData = GetInitData();
        //PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0, myCustomInitData);

        // Then to recieve this custom data on the prefab's script side use the OnPhotonInstantiate callback 
        /**
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
        object[] instantiationData = info.photonView.InstantiationData;
            // ...
        }
        **/



        this.OVRplayer = GameObject.Find("MultiplayerSetup/OVRPlayerController");
        this.MultiSetup = GameObject.Find("MultiplayerSetup");
        //GameObject GameManager = GameObject.Find("Multiplayer/GameManager");
        // Pull Current values from the room properties
        // My intuition is that we only need to check if one property is null because whoever joined first will set everything to not null

        if(PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"] != null){
            this.numRedPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
            this.numBluePlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];

            // Not sure of the best way to do this but we need to setup the score. If this ends up being the first player to connect we need to setup new entries. Seems a little inefficient considering score will never actually change

            this.redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
            this.blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
            this.ReadyToPlay = (int)PhotonNetwork.CurrentRoom.CustomProperties["readyToPlay"];
            
            
        }


        // Update properities and set this new player's team int;
        // Probably an unecessary amount of logic here but just keeping it reallly broad in case we ever want to move to 3v3 
        // This will alternate players
        if (this.numRedPlayers  > this.numBluePlayers){
            if (this.numBluePlayers < this.blueMax){
                // Assign this player team number 1
                this.teamNumber = 1;
                this.numBluePlayers++;
            }else{
                // Make this player a spectator
                this.teamNumber = 2;
            }

        } // If we make it here there are less or equal redPlayers
        else{
            if(this.numRedPlayers < this.redMax){
                this.teamNumber = 0;
                this.numRedPlayers++;
            }else{
                this.teamNumber = 2;
            }


        }

        
        if (this.ReadyToPlay >= 2){
            this.Wall.SetActive(false);
        }
        

       


       // rebuild or build for the first time
        this.hashProperties.Add("numRedPlayers", this.numRedPlayers);
        this.hashProperties.Add("numBluePlayers", this.numBluePlayers);
        this.hashProperties.Add("redScore", this.redScore);
        this.hashProperties.Add("blueScore", this.blueScore);
        this.hashProperties.Add("readyToPlay", this.ReadyToPlay);
        PhotonNetwork.CurrentRoom.SetCustomProperties(this.hashProperties);
       /// player 

       Hashtable playerHash = new Hashtable();
        playerHash.Add("TeamNumber", this.teamNumber);
       PhotonNetwork.LocalPlayer.SetCustomProperties(playerHash);

        // Here is where we set up the position of where the player will spawn based on their team
        
    
        this.MultiSetup.transform.position = Vector3.zero;
        this.MultiSetup.transform.localPosition = Vector3.zero;

        EntryPoint = this.spawnPoints[this.teamNumber];
        this.OVRplayer.transform.position = this.EntryPoint;
        this.OVRplayer.transform.localPosition = this.EntryPoint;

        
        EntryRotation = this.spawnRotations[this.teamNumber];
        this.OVRplayer.transform.rotation = this.EntryRotation;
        this.OVRplayer.transform.localRotation = this.EntryRotation;
        
       
    

        // Setup data initialization here

        object[] myCustomInitData = new object[1];
        myCustomInitData[0]=this.teamNumber; 
        //PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0, myCustomInitData);

        PhotonNetwork.Instantiate("NetworkedPlayer", EntryPoint, EntryRotation, 0, myCustomInitData);
       this.blackScreen.SetBool("ShouldFade", true);
      

        

    }



    public override void OnPlayerLeftRoom(Player otherPlayer){

        // Only the master should update when a player leaves. 
        
        Hashtable hash = new Hashtable();
        int numRed = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
        int numBlue = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];

        int teamNum = (int)otherPlayer.CustomProperties["TeamNumber"];

        if (teamNum == 0){
            numRed--;
            hash.Add("numRedPlayers", numRed);

        }else if (teamNum == 1){
            numBlue--;
            hash.Add("numBluePlayers", numBlue);

        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        



        





    }

    // Need to implement somesort of playerLeftRoom to keep track of who gets to be red team and who gets to be blue team. 
    // Can't change jain the room properties if you've left
    /*
    public override void OnLeftRoom(){
        // No way we leave a room without the properities being instantiated already so dont need to make any checks for null 
        // Create a clone 
        this.hashProperties = new Hashtable();


        this.numRedPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numRedPlayers"];
        this.numBluePlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["numBluePlayers"];

            // Not sure of the best way to do this but we need to setup the score. If this ends up being the first player to connect we need to setup new entries. Seems a little inefficient considering score will never actually change

        this.redScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["redScore"];
        this.blueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["blueScore"];
        this.ReadyToPlay = (int)PhotonNetwork.CurrentRoom.CustomProperties["readyToPlay"];

        if (this.teamNumber == 0){
            // The problem is that we can only use setCustomProperties I beleive, Cant just directly manipulate the hashtable so we have to do this funkyness
            // 
            this.numRedPlayers--;

            

        }else if (this.teamNumber ==1){
            this.numBluePlayers--;

        }
        // Reset the room properities before we leave 
        this.hashProperties.Add("numRedPlayers", this.numRedPlayers);
        this.hashProperties.Add("numBluePlayers", this.numBluePlayers);
        this.hashProperties.Add("redScore", this.redScore);
        this.hashProperties.Add("blueScore", this.blueScore);
        this.hashProperties.Add("readyToPlay", this.ReadyToPlay);
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(this.hashProperties);
        


    }
    */

    


}

