using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLerp : MonoBehaviour
{
    [Tooltip("How many updates the position should get a second (such as how many updates per second sent by the server)")]
    public int UpdatesPerSecond = 12;

    // The current transform
    private Transform startTransform;
    // The goal transform
    private Transform endTransform;
    // The time since the last transform
    private float timeSinceLastUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set the transforms to the initial position
        startTransform = transform;
        endTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time since the last update
        timeSinceLastUpdate += Time.deltaTime;
        // Get the predicted fraction of time until the next update
        float frac = timeSinceLastUpdate / (1 / UpdatesPerSecond);

        // Update the objects actual transform to be between the start and end transforms, by frac amount
        transform.position = Vector3.Lerp(startTransform.position, endTransform.position, frac);
        transform.rotation = Quaternion.Lerp(startTransform.rotation, endTransform.rotation, frac);
    }

    // Updates the gameobject to interpolate to the given transform
    public void UpdateTransform(Transform transform)
    {
        startTransform = endTransform;
        endTransform = transform;

        timeSinceLastUpdate = 0;
    }

    // Updates the gameobject to interpolate to the given position and rotation
    public void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        startTransform = endTransform;
        Transform newTransform = new GameObject().transform;
        newTransform.position = position;
        newTransform.rotation = rotation;
        endTransform = newTransform;

        timeSinceLastUpdate = 0;
    }
}
