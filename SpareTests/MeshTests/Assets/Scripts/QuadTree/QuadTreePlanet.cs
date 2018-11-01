using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreePlanet : MonoBehaviour {
    public Segment[] segments;
    [Range(0, 255)]
    public int resolution = 25;

    public float radius = 10;

    public float distanceToSplit = 200;
    public int maxLod = 10;

    public string name = "Planet: ";
    private void Start()
    {
        Create();
    }
    public void Create()
    {
        if(segments != null)
        {
            //foreach (Segment s in segments) 
            //Delete S
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
        foreach(Vector3 vect in vectors)
        {
            GameObject go = new GameObject(name + ": " + vect);
            go.transform.parent = transform;
            Segment s = go.AddComponent<Segment>();
            s.MakeSegment(this, resolution, radius, vect, 1, Vector3.zero);
            segments[count] = s;
            count++;
        }

    }

    private void Update()
    {
        foreach (Segment s in segments)
            s.CheckLod();
    }
}
