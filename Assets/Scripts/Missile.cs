using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Missile : MonoBehaviourPun, Photon.Pun.IPunObservable
{
    [Tooltip("How much faster the projectile is than the hand that created it")]
    public float speedMultiplier = 3;
    [Tooltip("The minimum speed the projectile can move at before being deleted")]
    public float minSpeed = .01f;
    [Tooltip("The amount of seconds before the projectile starts deccelerating")]
    public float timeBeforeDecceleration = 1;
    [Tooltip("How quickly the projectile deccelerates")]
    public float deccelerationSpeed = 1;

    [HideInInspector] public Vector3 velocity { set; get; }
    private float lifetime;

    private TransformLerp transformLerp;

    // Start is called before the first frame update
    void OnEnable()
    {
        lifetime = 0;
    }

    void Start()
    {
        transformLerp = GetComponent<TransformLerp>();
        if (photonView.IsMine)
        {
            transformLerp.enabled = false;
            gameObject.tag = "mine";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only update missile locally if it was created by the local player
        if (photonView.IsMine){
            transform.position += velocity * speedMultiplier * Time.deltaTime;

            lifetime += Time.deltaTime;
            if (lifetime > timeBeforeDecceleration)
            {
                speedMultiplier -= deccelerationSpeed * Time.deltaTime;
            }

            if (velocity.sqrMagnitude * speedMultiplier < minSpeed * minSpeed)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    // Write position to stream, and receive it by players who did not create it
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {   
            if (!photonView.IsMine) {  
                if (transformLerp)
                {
                    transformLerp.UpdateTransform((Vector3)stream.ReceiveNext(), Quaternion.identity);
                }
                else
                {
                    transform.position = (Vector3)stream.ReceiveNext();
                }
            }
        }
    }
}
