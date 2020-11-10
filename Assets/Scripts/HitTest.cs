using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;

public class HitTest : MonoBehaviour
{
    // currently this is local to an assigned bender
    public OVRPlayerController bender;
    public int health;
    public int crithealth;
    // canvas animator for screen effect (didn't work well w/ post-processing);
    public Animator anim;
    // heartbeat audio
    public AudioClip heartbeat;
    private AudioSource aud;


    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Hit", false);
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other");
        if (other.tag == "element")
        {
            Debug.Log("hit");
            health -= 1;

            // destroy element upon hit
            // Destroy(other.GameObject)

            // screen flashes red
            anim.SetBool("Hit", true);


            // vibration
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            StartCoroutine(hapticsTimeout());

            // stop screen flash
            anim.SetBool("Hit", false);
           

            // bloody screen and heartbeat intensified
            if (health < crithealth)
            {
                // if another animation is wanted - this one lines up pretty closely w/ audio
                // and would stay until health regenerates... if we're doing that
                anim.SetBool("CritHit", true);

                // heartbeat audio
                aud.PlayOneShot(heartbeat);

            }

            else
            {
                anim.SetBool("CritHit", false);
            }
        }
    }


    private IEnumerator hapticsTimeout()
    {
        yield return new WaitForSeconds(1);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }


}
