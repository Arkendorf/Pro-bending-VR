using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedBall : MonoBehaviourPun, Photon.Pun.IPunObservable
{
    public GameObject ball;
    public Transform ballPosLocal;
    public DidTrigger otherScript;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine){
        this.transform.position = Vector3.zero;
        this.transform.localPosition = Vector3.zero;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);

        ballPosLocal = ball.transform; 
        ballPosLocal.position = Vector3.zero;
        ballPosLocal.localPosition = Vector3.zero;
        otherScript = (DidTrigger)ball.GetComponent(typeof(DidTrigger));

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine){
        if(otherScript.didYouTrigger()){
            Vector3 temp = new Vector3(1, 0 ,0 );
            ballPosLocal.position += temp;
            otherScript.didTrigger = false;
        }
        }
        
    }



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ballPosLocal.position);
            
        }
        else
        {
            
            ballPosLocal.position = (Vector3)stream.ReceiveNext();
            
        }
    }

}
