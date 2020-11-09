using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private int redleft;
    private int blueleft;
    public PlayerStats red1;
    public PlayerStats red2;
    public PlayerStats red3;
    public PlayerStats blue1;
    public PlayerStats blue2;
    public PlayerStats blue3;
    private ArrayList players;


    // Start is called before the first frame update
    void Start()
    {
        redleft = 3;
        blueleft = 3;
        players = new ArrayList(new PlayerStats[] { red1, red2, red3, blue1, blue2, blue3 });
    }

    // Update is called once per frame
    void Update()
    {
        string winner = didWin();
        if (winner != null)
        {
            // end the game, show the winner
        }
    }

    private string didWin()
    {
        if (redleft == 0)
        {
            return "red";
        }

        else if (blueleft == 0) {
            return "blue";
        }

        return null;
    }

    public void wasHit(OVRPlayerController bender)
    {
        foreach (PlayerStats pstats in players)
        {
            if (!pstats || pstats == null)
            {
                continue;
            }

            if (pstats.player == bender)
            {
                pstats.health -= 1;
                if (pstats.health == 0)
                {
                    if (pstats.team == 0)
                    {
                        redleft -= 1;
                    }

                    else
                    {
                        blueleft -= 1;
                    }
                }

                handleHit(pstats);
            }
        }
    }

    private void handleHit(PlayerStats bender)
    {
        // get controller and send haptics



        // depending on health, bloody screen
        if (bender.health < 6)
        {

        }

        if (bender.health < 3)
        {

        }
    }
}
