using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Collision : MonoBehaviourPun
{
    public string collideEffect;

    private Missile missile;
    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Missile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't collide with the player who instantiated this missile
        if (other.gameObject.tag != "Player") {
            // TODO: fix instantiation of puff effect
            PhotonNetwork.Instantiate(collideEffect, transform.position, Quaternion.LookRotation(-missile.velocity));
            // Destroy the missile on all clients
            PhotonNetwork.Destroy(gameObject);
        }      
    }
}
