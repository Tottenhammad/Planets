using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour {

    public TerrarinSide[] meshes;
    public int resolution = 10;
    public float radius = 10;
    public bool refresh;
    

    public void Create()
    {
        if (meshes != null)
        {
            foreach (TerrarinSide meshMade in meshes)
                meshMade.Delete();
            meshes = null;
        }
        float modifier = ((resolution - 1) * radius) / 2;
        meshes = new TerrarinSide[] {
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, transform.up, Vector3.zero, Vector3.zero, null, false, null),
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, -transform.up, Vector3.zero, Vector3.zero, null, false, null),
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, -transform.right, Vector3.zero, Vector3.zero, null, false, null),
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, transform.right, Vector3.zero, Vector3.zero, null, false, null),
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, transform.forward, Vector3.zero, Vector3.zero, null, false, null),
            new TerrarinSide(gameObject, new GameObject(), 0, resolution, radius, -transform.forward, Vector3.zero, Vector3.zero, null, false, null),
        };

    }
    private void Start()
    {
        Create();
    }
    private void Update()
    {
        if (refresh)
        {
            refresh = false;
            Create();
        }
    }
}
