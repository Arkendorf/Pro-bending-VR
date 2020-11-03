using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public GameObject collideEffect;

    private Missile missile;
    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Missile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject effect = Instantiate(collideEffect, transform.position, Quaternion.LookRotation(-missile.velocity));
        Destroy(gameObject);
    }
}
