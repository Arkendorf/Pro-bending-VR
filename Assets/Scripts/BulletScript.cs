using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BulletScript : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVelocity(Vector3 vel){
        this.GetComponent<PhotonView>().RPC("setVelocityPUN", RpcTarget.All, vel);
    }

    [PunRPC]
    public void setVelocityPUN(Vector3 vel){
        Rigidbody br = this.GetComponent<Rigidbody>();
        br.velocity = vel;


    }
}
