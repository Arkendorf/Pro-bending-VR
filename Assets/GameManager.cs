using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

// Welcome to GameManager where the events of the game will Occur.
// The game Manager needs to have a photonView. Everyone in the game will have a copy of this script but certain values WILL NEVER be changed unless through a RPC to all players so that copies of this stay consisten

public class GameManager : MonoBehaviour
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
    //public TextMeshProUGUI RedScoreDisplay;
    public TextMeshProUGUI BlueScoreDisplay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //BlueScoreDisplay.SetText((networkedHealth.BlueScore).ToString());
        //BlueScoreDisplay.SetText("Tst");
        
        
    }

    
}
