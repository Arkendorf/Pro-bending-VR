using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyButtonTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public int buttonId;
    public Animator buttonPress;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other){

        gameManager.StartButtonHit(buttonId);
        // Play button animation
        buttonPress.SetBool("buttonPressed", true);
        // It will trigger lets immediately set that bad boy to false;

        //buttonPress.SetBool("buttonPressed",false);

        

    }


    // Not working.. 
    private void OnTriggerExit(Collider other){

        buttonPress.SetBool("buttonPressed", false);
    }
    
}
