using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collision : MonoBehaviourPun
{
    public GameObject collideEffect;

    private Missile missile;
    private Collider collider;
    private ParticleSystem particles;

    private Vector3 oldPos;
    private Quaternion puffRotation;

    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Missile>();
        collider = GetComponent<Collider>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        puffRotation = Quaternion.LookRotation(oldPos - transform.position);
        oldPos = transform.position;

        // When particle system is ended, destroy this missile
        if (particles.isStopped)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    [PunRPC]
    public void Collide(){
        Instantiate(collideEffect, transform.position, puffRotation);
        // Disable collider to prevent more collisions from the same particle while it is deleting
        collider.enabled = false;
        // Stop particle system
        if(photonView.IsMine){
            particles.Stop();
            missile.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
           // Don't collide with the player who instantiated this missile
           // Missile if we instantiated it will be tagged mine
           // Player controller on the ovr will be tagged playerController, and centerEyeAnchor where we currently have the collider box will be tagged MainCamera
        if (gameObject.tag!="mine" || (other.tag != "playerController" && other.tag != "MainCamera") ){
            GetComponent<PhotonView>().RPC("Collide", RpcTarget.All);
        }   
    }
}
