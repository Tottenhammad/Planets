using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class DropSound : MonoBehaviour {

    public AudioClip Heavy;
    public AudioClip Medium;
    public AudioClip Light;

    public Vector3 magnitudes;

    AudioSource AS;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude >= magnitudes[2])
            AS.clip = Heavy;
        else if (collision.relativeVelocity.magnitude >= magnitudes[1])
            AS.clip = Medium;
        else if (collision.relativeVelocity.magnitude >= magnitudes[0])
            AS.clip = Light;
        else
            AS.clip = null;

        AS.Play();

    }
}
