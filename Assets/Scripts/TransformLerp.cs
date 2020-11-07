using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TransformLerp : MonoBehaviour
{
    // The current transform
    private Vector3 startPosition;
    private Quaternion startRotation;
    // The goal transform
    private Vector3 endPosition;
    private Quaternion endRotation;
    // The time since the last transform
    private float timeSinceLastUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set the transforms to the initial position
        startPosition = transform.position;
        startRotation = transform.rotation;
        endPosition = transform.position;
        endRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time since the last update
        timeSinceLastUpdate += Time.deltaTime;
        // Get the predicted fraction of time until the next update
        float frac = timeSinceLastUpdate / (1f / PhotonNetwork.SerializationRate);

        // Update the objects actual transform to be between the start and end transforms, by frac amount
        transform.position = Vector3.Lerp(startPosition, endPosition, frac);
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, frac);
    }

    // Updates the gameobject to interpolate to the given transform
    public void UpdateTransform(Transform transform)
    {
        startPosition = endPosition;
        startRotation = endRotation;
        endPosition = transform.position;
        endRotation = transform.rotation;

        ResetTransform();
    }

    // Updates the gameobject to interpolate to the given position and rotation
    public void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        startPosition = endPosition;
        startRotation = endRotation;
        endPosition = position;
        endRotation = rotation;

        ResetTransform();
    }

    private void ResetTransform()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        timeSinceLastUpdate = 0;
    }
}
