using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
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

    // Start is called before the first frame update
    void OnEnable()
    {
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * speedMultiplier * Time.deltaTime;

        lifetime += Time.deltaTime;
        if (lifetime > timeBeforeDecceleration)
        {
            speedMultiplier -= deccelerationSpeed * Time.deltaTime;
        }

        if (velocity.sqrMagnitude * speedMultiplier < minSpeed * minSpeed)
        {
            Destroy(gameObject);
        }
    }
}
