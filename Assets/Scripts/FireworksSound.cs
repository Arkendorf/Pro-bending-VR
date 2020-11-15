using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksSound : MonoBehaviour
{

    public AudioClip FireworkBorn;
    public AudioClip FireworkDeath;
    public ParticleSystem Rocket;

    private AudioSource audioSource;
    private int numParticles = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Play Cheering sounds
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }

        // Play fireworks sounds
        var count = Rocket.particleCount;
        if (count < numParticles) {
            audioSource.PlayOneShot(FireworkDeath, 1f);
        }
        else if (count > numParticles) {
            audioSource.PlayOneShot(FireworkBorn, 1f);
        }
        numParticles = count;
    }
}