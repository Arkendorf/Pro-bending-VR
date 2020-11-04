using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedBender : MonoBehaviourPun
{
    [Tooltip("The networked player who's hand to track")]
    public NetworkedPlayer networkedPlayer;
    [Space(10)]
    [Tooltip("Whether to track the right hand (or the left).")]
    public bool rightHand;
    [Tooltip("The button(s) that can be pressed to initiate bending")]
    public OVRInput.Button button;
    [Tooltip("The offset from the head where the user's shoulder is assumed to be. This is used to calculate the direction of projectiles")]
    public Vector3 shoulderOffset = new Vector3(.1f, -.1f, 0);
    [Space(10)]
    [Tooltip("If the hand moves above this speed, a element projectile is created that starts tracking the hand's movements")]
    public float speedThreshold = .6f;
    [Tooltip("If the hand moves below this speed, an active element projectile will stop responding to the hand's movements")]
    public float speedMin = .4f;
    [Tooltip("The minimum amount of time the user has to be making a punch motion for before a projectile is created (to prevent spam)")]
    public float minPunchTime = .1f;
    [Space(10)]
    [Tooltip("The element prefab to spawn when an element projectile is created")]
    public string element;  

    // The hand's current velocity
    private Vector3 velocity;
    // The hand's position in the previous frame
    private Vector3 oldPosition;
    // The distance between the hand and the head (only on the x-z plane) last frame
    private float oldDistance;
    // Whether a projectile has already been fired out of this hand
    private bool fired;
    // A reference to the missile script on the fired projectile
    private Missile missile;
    // The maximum magnitude of the velocity this hand has reached while firing the current projectile
    private float maxMagnitude;
    // How long the hand has been punching for
    private float punchTime;
    // The current assumed position of the user's shoulder
    private Vector3 shoulderPos;
    // Start is called before the first frame update
    private Transform handPos;
    
    void Start()
    {
        // Get reference to networked player
        if (!networkedPlayer) {
            networkedPlayer = GetComponent<NetworkedPlayer>();
        } 

        // Get a reference to the proper hand's transform    
        if (rightHand) {
            handPos = networkedPlayer.playerRightHandLocal;
        } 
        else {
            handPos = networkedPlayer.playerLeftHandLocal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only update the bender if it is the owner
        if (networkedPlayer.photonView.IsMine) {

            // Calculated the asssumed position of the shoulder
            shoulderPos = networkedPlayer.playerHeadLocal.position + networkedPlayer.playerHeadLocal.rotation * shoulderOffset;

            // Get the hand's velocity (and the magnitude of that velocity)
            velocity = (handPos.position - oldPosition) / Time.deltaTime;
            float magnitude = velocity.magnitude;

            // Calculate the distance between the hand and the head on the x-z plane
            Vector3 pos1 = networkedPlayer.playerHeadLocal.position;
            Vector3 pos2 = handPos.position;
            pos1.y = 0;
            pos2.y = 0;
            float distance = Vector3.Distance(pos2, pos1);

            // If a projectile has been fired, continue to update it's velocity
            if (fired)
            {
                // If the current magnitude is greater than the previous max magnitude, update the max magnitude
                if (magnitude > maxMagnitude)
                {
                    maxMagnitude = magnitude;
                }
                // Set the missile's velocity to move in the direction the arm is reaching (offset between the hand and shoulder)
                // Set the missile's velocity to move at the speed of the maximum magnitude of this punch
                missile.velocity = (handPos.position - shoulderPos).normalized * maxMagnitude;
            }
            // Check if the button is pressed (to know if a projectile can be created or not)
            if (OVRInput.Get(button))
            {
                // If a projectile hasn't been fired, the hand's velocity is moving fast enough, and the hand is moving away from the head...
                if (!fired && magnitude >= speedThreshold && distance > oldDistance)
                {
                    // If all of the above are true, the hand is considered "punching". Update the timer on how long the punch has lasted
                    punchTime += Time.deltaTime;
                    // If the punch time passes the threshold, create a new element projectile
                    if (punchTime > minPunchTime)
                    {
                        Shoot();
                    }               
                }
                // Otherwise, if the hand has fired a projectile and the punch has now ended (because the speed has gone below the minimum)...
                else if (fired && (magnitude <= speedMin))
                {
                    // Reset the hand, allowing for another punch to be made
                    Reload();
                }
            }
            // If the button is no longer pressed, but a projectile has been fired, reset the hand.
            else if (fired)
            {
                Reload();
            }

            // Update the old position and distance values for use next frame
            oldPosition = handPos.position;
            oldDistance = distance;          
        }
    }

        // Instantiates a new element projectile
    private void Shoot()
    {
        fired = true; // Note that a projectile has been fired

        GameObject elementObj = PhotonNetwork.Instantiate(element, handPos.position, Quaternion.identity);
        // Save a reference to the missile script on the new projectile
        missile = elementObj.GetComponent<Missile>();
        // Set the missile's velocity to move in the direction the arm is reaching (offset between the hand and shoulder)
        // Set the missile's velocity to move at the speed of the maximum magnitude of this punch
        missile.velocity = (handPos.position - shoulderPos).normalized * maxMagnitude;
    }

    // Reset the hand
    private void Reload()
    {
        fired = false;
        maxMagnitude = 0;
        punchTime = 0;
    }
}
