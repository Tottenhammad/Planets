using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshData {


    public Vector3[] vertices;
    public Vector3[] baseVerts;
    public int[] triangles;


    public int triangleI = 0;
    public int resolution;

    public Mesh mesh;
    public Material mat;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCol;
    public MeshFilter meshFilter;

    public Vector3 upDirection;
    public Vector3 secondDirection;
    public Vector3 thirdDirection;

    Transform meshOwner;
    Filter noiseFilter;
    public MeshData(int resolution, MeshRenderer meshRenderer, MeshCollider meshCol, MeshFilter meshFilter,  Vector3 upDir, Transform owner, NoiseSettings nS)
    {
        this.resolution = resolution;
        this.meshRenderer = meshRenderer;
        this.meshCol = meshCol;
        this.meshFilter = meshFilter;
        this.meshOwner = owner;
        mesh = new Mesh();
        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        upDirection = upDir;
        secondDirection = new Vector3(upDirection.y, upDirection.z, upDirection.x);
        thirdDirection = Vector3.Cross(upDirection, secondDirection);
        noiseFilter = new Filter(nS);

    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleI] = a;
        triangles[triangleI + 1] = b;
        triangles[triangleI + 2] = c;

        triangleI += 3;
    }

    public void RefreshMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshRenderer.sharedMaterial = mat;
        meshCol.sharedMesh = mesh;
        meshFilter.sharedMesh = mesh;

    }
    public void Generate()
    {
        triangleI = 0;
        for (int i = 0, index = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++, index++)
            {
                Vector2 per = new Vector2(j, i) / (resolution - 1);
                Vector3 positionOnFace = upDirection + (per.x - .5f) * 2 * secondDirection + (per.y - .5f) * 2 * thirdDirection;
                //Debug.Log(positionOnFace);
                vertices[index] = positionOnFace;
                if (i < resolution - 1 && j < resolution - 1)
                {
                    AddTriangle(index, index + resolution + 1, index + resolution);
                    AddTriangle(index, index + 1, index + resolution + 1);
                }
            }
        }

        baseVerts = vertices;
    }

    public void Normalise()
    {
        for (var i = 0; i < vertices.Length; i++)
            vertices[i] = vertices[i].normalized;
    }
    public void Offset(Vector3 offset)
    {
        for (var i = 0; i < vertices.Length; i++)
            vertices[i] += offset;
    }
    public void Rotate(Vector3 pivot, Vector3 rotateAmount)
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = rotateAmount;

        for (var i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * (vertices[i] - pivot) + pivot;
    }
    public void Scale(Vector3 scaler)
    {
        for (var i = 0; i < vertices.Length; i++)
            vertices[i] = Vector3.Scale(vertices[i], scaler);
    }

    public void SetMaterial(Material material)
    {
        this.mat = material;
    }

    public void Noise()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            float elevation = noiseFilter.Eval(baseVerts[i]);
            vertices[i] = baseVerts[i] * (1+elevation);
        }
    }
}
