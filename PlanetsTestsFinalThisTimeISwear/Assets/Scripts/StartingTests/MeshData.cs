using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData {

    public Vector3[] vertices;
    public int[] triangles;


    public int triangleI = 0;

    NoiseFilter noise = new NoiseFilter();
    NoiseSettings noiseSettings;

    public Mesh mesh;
    public Material mat;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCol;

    public MeshData(int resolution, MeshRenderer meshRenderer, MeshCollider meshCol, NoiseSettings noiseSettings)
    {
        this.noiseSettings = noiseSettings;
        this.meshRenderer = meshRenderer;
        this.meshCol = meshCol;
        mesh = new Mesh();
        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];
    }
    
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleI] = a;
        triangles[triangleI+1] = b;
        triangles[triangleI+2] = c;

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

    public void Tests()
    {
        DiamondSquare ds = new DiamondSquare(9, 2);
        int pixelGapX = Mathf.RoundToInt(ds.GetSize() / Mathf.Sqrt(vertices.Length));
        int pixelGapY = Mathf.RoundToInt(ds.GetSize() / Mathf.Sqrt(vertices.Length));
        int y = 0;
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] *=   ds.Get(Mathf.RoundToInt(i - (y * Mathf.Sqrt(vertices.Length))) * pixelGapX, y * pixelGapY);
            if (i - (y * Mathf.Sqrt(vertices.Length)) >= Mathf.Sqrt(vertices.Length))
            {
                y += 1;
            }

        }
    }
    public void SetMaterial(Material material)
    {
        this.mat = material;
    }
    public void PositionBasedNoise(Transform tr, Vector3 scaler)
    {


        for (var i = 0; i < vertices.Length; i++)
        {
            NoiseSettings test = new NoiseSettings();
            float noisePoint = ((noise.Evaluate(tr.TransformPoint(vertices[i]), noiseSettings, true) + 1) * .5f);

            vertices[i] = Vector3.Scale(vertices[i], scaler * (1 + noisePoint));

        }
    }
    //Copied
    public Vector2 PerlinNoise3D(Transform tr, float radius, float scaler, int octaves = 2, float lucanarity = 2f, float gain = 0.1f, float warp = 0.1f, float testing = 10)
    {
        LibNoise.Generator.Perlin planes = GameObject.FindGameObjectWithTag("Setting").GetComponent<TestongThings>().planes;
        LibNoise.Generator.RidgedMultifractal mountains = GameObject.FindGameObjectWithTag("Setting").GetComponent<TestongThings>().mountains;
        float highest = 0;
        float lowest = 0;
        for (var i = 0; i < vertices.Length; i++)
        {
            Vector3 point = tr.TransformPoint(vertices[i]);
            //Vector3 point = vertices[i];
            float sum = 0.0f, freq = 1.0f, amp = 1.0f;

            for(int j = 0; j < octaves; j++)
            {
                sum += amp * (float)planes.GetValue(point);
                freq *= lucanarity;
                amp *= gain;
            }

            sum *= (float)mountains.GetValue(point * freq)  * octaves * testing;
            sum = Mathf.Max(-1  , sum/2);


            vertices[i] = vertices[i].normalized * (radius * scaler + sum);


            if (sum > highest)
                highest = sum;
            if (sum < lowest && sum > 0)
                lowest = sum;
        }
        return new Vector2(lowest, highest);
    }
    public void DisplacementMap()
    {

        Texture2D testing = Camera.main.GetComponent<TestongThings>().tests;
        int pixelGapX = Mathf.RoundToInt(testing.width / Mathf.Sqrt(vertices.Length));
        int pixelGapY = Mathf.RoundToInt(testing.height / Mathf.Sqrt(vertices.Length));
        int y = 0;
        for (var i = 0; i < vertices.Length; i++)
        {

            vertices[i] +=  Vector3.one * (1 - testing.GetPixel(Mathf.RoundToInt(i - (y * Mathf.Sqrt(vertices.Length))) * pixelGapX, y * pixelGapY).grayscale)  * 100;
            if(i - (y * Mathf.Sqrt(vertices.Length)) >= Mathf.Sqrt(vertices.Length))
            {
                y += 1;
            }

        }
    }
}
