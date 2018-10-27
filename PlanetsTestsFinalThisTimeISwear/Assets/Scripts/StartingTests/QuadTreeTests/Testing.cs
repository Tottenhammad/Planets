using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    QuadTree test;
    public int resolution;
    public float radius;

    private void Start()
    {
        test = new QuadTree(null, new GameObject(), resolution, radius, Vector3.up, 1, 6, Vector3.zero, Vector3.zero, null, 100, null);
    }

    private void Update()
    {
        test.CheckLod();
    }
}
