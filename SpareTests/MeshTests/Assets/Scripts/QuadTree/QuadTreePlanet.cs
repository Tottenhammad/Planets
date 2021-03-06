﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Place { TOPLEFT, BOTTOMLEFT, TOPRIGHT, BOTTOMRIGHT, PLANE}
public class QuadTreePlanet : MonoBehaviour
{
    public Place[] test = new Place[] {Place.TOPLEFT, Place.TOPRIGHT, Place.BOTTOMLEFT, Place.BOTTOMRIGHT, Place.PLANE};

    public Segment[] segments;
    [Range(0, 255)]
    public int resolution = 25;

    public float radius = 10;

    public float distanceToSplit = 200;
    public bool preLoad = true;
    public int preLoadDistance;
    public int maxLod = 10;
    public int preMaxLod = 3;

    public NoiseSettings noiseSettings;


    GameObject player;
    public string Name = "Planet: ";

    public Place fireSplit;

    
    public Gradient grad;
    [HideInInspector]
    public Texture2D color;

    bool done = false;
    private void Start()
    {
        Create();
        StartCoroutine(loop());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator loop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            fireSplit = Place.PLANE;
            yield return new WaitForSecondsRealtime(0.1f);
            fireSplit = Place.TOPLEFT;
            yield return new WaitForSecondsRealtime(0.1f);
            fireSplit = Place.BOTTOMLEFT;
            yield return new WaitForSecondsRealtime(0.1f);
            fireSplit = Place.TOPRIGHT;
            yield return new WaitForSecondsRealtime(0.1f);
            fireSplit = Place.BOTTOMRIGHT;

        }
    }

    public void Create()
    {
        if (segments != null)
        {
            try
            {
                foreach (Segment s in segments)
                    s.Delete(true);
            }
            catch
            {

            }

            segments = null;
        }


        segments = new Segment[6];

        Vector3[] vectors = new Vector3[]
        {
            transform.up,
            -transform.up,
            -transform.right,
            transform.right,
            transform.forward,
            -transform.forward
        };
        int count = 0;
        foreach (Vector3 vect in vectors)
        {
            GameObject go = new GameObject(Name + ": " + vect);
            go.transform.parent = transform;
            go.transform.position = transform.position;
            Segment s = go.AddComponent<Segment>();
            s.MakeSegment(this, resolution, radius, vect, 1, Vector3.zero, Place.PLANE);
            segments[count] = s;
            count++;
        }

    }

    private void Update()
    {

        foreach (Segment s in segments)
            s.CheckLod();
// Debug.Log("Distance: " + Vector3.Distance(player.transform.position, transform.position) + " / " + preLoadDistance);
        if(Vector3.Distance(player.transform.position, transform.position) <= preLoadDistance && done == false && Vector3.Distance(player.transform.position, transform.position) >= distanceToSplit)
        {
            foreach (Segment s in segments)
            {
                Camera.main.GetComponent<SplitController>().NextLoop.Add(s);
            }
            done = true;
        }
       
    }



    IEnumerator Checker()
    {
        foreach (Segment s in segments)
        {
            yield return new WaitForSeconds(0.1f);
            s.CheckLod();
        }
    }
    string json;
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            segments[0].Delete(true);
    }
    private void OnValidate()
    {
        foreach (Segment s in segments)
            s.ResetSegment();
    }
}
