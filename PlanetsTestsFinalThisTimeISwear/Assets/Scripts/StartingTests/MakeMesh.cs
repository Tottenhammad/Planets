using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MakeMesh {

    MeshData meshData;
    MeshFilter meshFilter;

    public int resolution = 10;
    public float radius = 10;
    public Vector3 rotation;
    public Vector3 offset;

    public GameObject go;
    public MakeMesh(GameObject parent,GameObject go, int resolution, float radius, Vector3 rotation, Vector3 offset)
    {
        this.go = go;
        if (parent != null)
            this.go.transform.parent = parent.transform;
        this.resolution = resolution;
        this.radius = radius;
        this.rotation = rotation;
        this.offset = offset;

        meshData = new MeshData(this.resolution, go.AddComponent<MeshRenderer>(), go.AddComponent<MeshCollider>(), null);
        meshFilter = go.AddComponent<MeshFilter>();


        Generate();
    }
    /*private void OnValidate()
    {
        meshData = new MeshData(resolution);
        meshFilter = go.GetComponent<MeshFilter>();

       Generate();
    }*/

    public void Generate()
    {
        for (int i = 0, index = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++, index++)
            {
                meshData.vertices[index] = new Vector3(i, 1, j);
                if (i < resolution - 1 && j < resolution - 1)
                {
                    meshData.AddTriangle(index, index + resolution + 1, index + resolution);
                    meshData.AddTriangle(index + resolution + 1, index, index + 1);
                }
            }
        }
        //Debug.Log(meshData.vertices[0] + "_" + meshData.vertices[resolution - 1]);
        meshData.Normalise();
        meshData.Scale(new Vector3(radius, 1, radius));
        meshData.Offset(offset);
        meshData.RefreshMesh();
        meshData.Rotate(meshData.mesh.bounds.center, rotation);

        meshData.RefreshMesh();
        meshFilter.sharedMesh = meshData.mesh;
    }

    public void Delete()
    {
        MonoBehaviour.Destroy(go);
    }

}
