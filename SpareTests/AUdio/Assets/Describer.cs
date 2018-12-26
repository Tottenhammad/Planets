using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Describer : MonoBehaviour {

    public AudioClip AC;


    AudioSource AS;


    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }


    private void Update()
    {
        AS.clip = AC;
        if (Input.GetKeyDown(KeyCode.Space))
            AS.Play();
    }
}
