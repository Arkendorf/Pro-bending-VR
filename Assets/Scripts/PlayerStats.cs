using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ScriptableObject
{
    public int health = 10;
    // 0 for red, 1 for blue
    public int team;
    private int hits;
    private int knockouts;
    public OVRPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
