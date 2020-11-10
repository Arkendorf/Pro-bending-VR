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
    public NetworkedHealth networkedHealth;
    public int blue  = 0;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
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
// We can access existinging players by accessing Room.Players or the number by accessing Room.playerCount which could be helpful for assigning people who join to a locker room 
// or the stands

// A similar method is OnPlayerEnteredRoom(Player newPlayer). Which works similarily but is called any time a remote player enters the room (so not the host)

// This is where we instantiate each player Prefab, currently the prefab is called "NetworkedPlayer" and is a prefab located in Assets/Resources/NetworkedPlayer.prefab

// Loads from resources folder. 
/*
    public override void OnCreatedRoom(){
        // This will only be called when the master creates the room
        // Lets
        Hashtable hash = new Hashtable();
        hash.Add("RedCount", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);



    }
    */
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

        Vector3 pos1 = new Vector3(-15,-2, 0);
        Vector3 pos2 = new Vector3(15,-2,0);
        GameObject MultiSetup = GameObject.Find("MultiplayerSetup/OVRPlayerController");
        if(PhotonNetwork.CurrentRoom.PlayerCount ==1){
        MultiSetup.transform.position = pos1;
        MultiSetup.transform.localPosition = pos1;
        EntryPoint = pos1;
        blue = 50;

        }else{
        MultiSetup.transform.position = pos2;
        MultiSetup.transform.localPosition = pos2;
        EntryPoint = pos2;
        blue = 1;
        }

        //if (PhotonNetwork.CurrentRoom.PlayerCount< 3){
         
        // GAME OBJECTS ARE NOT SERIALIZABLE
        /**
        if (((bool)PhotonNetwork.CurrentRoom.CustomProperties["RedCount"] != true)){
            Hashtable hash = new Hashtable();
            hash.Add("RedCount", 20);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        }else{
            Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
            hash["RedCount"] = (int)hash["RedCount"] + 10;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        }
        **/
        Hashtable hash = new Hashtable();
        int RedCount = 0;
        if(PhotonNetwork.CurrentRoom.CustomProperties["RedCount"] != null){
            RedCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedCount"];
            


        }
        RedCount++;
        hash.Add("RedCount", RedCount);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        PhotonNetwork.Instantiate("NetworkedPlayer", EntryPoint, Quaternion.identity, 0);


        // Destroying an Object (this could get a little tricky)
        /**
        If you want to use PhotonNetwork.Destroy(Gameobject) but are not the master client (server) you need request an ownership transfer so that you now own the object and then can destroy
        See documentation here 
        https://doc.photonengine.com/en-us/pun/v1/demos-and-tutorials/package-demos/ownership-transfer

        see forum answer here 
        https://answers.unity.com/questions/1451821/how-to-use-photonnetworkdestroy-if-i-am-not-the-ma.html 

        Not sure if we will run into this. I guess hypothetically a ball or element or whatever can just know when it needs to destroy itself and we should be good

        **/

        // If you want to call method on other clients. Like one player wants to call a method on another you must use a remote Procedure call
        // [PunRPC] see documentation here. https://doc.photonengine.com/en-us/pun/v2/gameplay/rpcsandraiseevent 
        // Might be useful if a player needs to access method on Element prefabs... 





        // After a new player has added we must set everyone up with the correct information.
        // Using the GameManager of the master client, update that and then broadcast this information so that everyone gets it

        
        //networkedHealth.GetComponent<PhotonView>().RPC("MasterSendsData", RpcTarget.MasterClient, networkedHealth.BlueScore);


        

    }


    // Need to implement somesort of playerLeftRoom to keep track of who gets to be red team and who gets to be blue team. 
    // There is OnPlayerLeftRoom() and OnLeftRoom(), i think keeping with style we have started I will do OnLeftRoom()
    public override void OnLeftRoom(){
        // CleanUp here

    }


}

