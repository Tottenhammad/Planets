using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public MakeMesh[] meshes;
    public int resolution = 10;
    public float radius = 10;
    public bool refresh;


    public void Create()
    {
        if (meshes != null)
        {
            foreach (MakeMesh meshMade in meshes)
                meshMade.Delete();
            meshes = null;
        }
        float modifier = ((resolution - 1) * radius) / 2;
        float yOffset = 1;
        meshes = new MakeMesh[] {
            new MakeMesh(gameObject, new GameObject("Top"), resolution, radius, new Vector3(0, 0, 0), new Vector3(0, modifier - yOffset, 0)), // top
            new MakeMesh(gameObject, new GameObject("Bottom"), resolution, radius, new Vector3(180, 0, 0), new Vector3(0, -modifier - yOffset, 0)), // bottom
            new MakeMesh(gameObject, new GameObject("Left"), resolution, radius, new Vector3(90, 0, 0), new Vector3(0, -yOffset, modifier)), // left
            new MakeMesh(gameObject, new GameObject("Right"), resolution, radius, new Vector3(-90, 0, 0), new Vector3(0, -yOffset, -modifier)), // right
            new MakeMesh(gameObject, new GameObject("Forward"), resolution, radius, new Vector3(0, 0, 90), new Vector3(-modifier, -yOffset, 0)), // forward
            new MakeMesh(gameObject, new GameObject("Back"), resolution, radius, new Vector3(0, 0, -90), new Vector3(modifier, -yOffset, 0)), // back
        };

    }
    private void Start()
    {
        Create();
    }
    /*private void OnValidate()
    {
        Create();
    }*/

    private void Update()
    {
        if (refresh)
        {
            refresh = false;
            Create();
        }
    }
}
