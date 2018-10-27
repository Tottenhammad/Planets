using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrarinSide {

    public MeshData meshData;

    public Vector3 upDirection;
    public Vector3 secondDirection;
    public Vector3 thirdDirection;

    MeshFilter meshFilter;

    public int resolution = 10;
    public float radius = 10;
    public Vector3 rotation;
    public Vector3 offset;
    public Vector3 scaler;
    public bool useNoise = false;
    private int lod = 0;
    public GameObject go;
    public Material mat;
    public TerrarinSide(GameObject parent, GameObject go, int lod, int resolution, float radius, Vector3 Updirection, Vector3 displacement, Vector3 scaler, NoiseSettings noiseSettings, bool useNoise, Material mat)
    {
        this.go = go;

        if (parent != null)
        {
            this.go.transform.parent = parent.transform;
            this.go.transform.position = parent.transform.position;
        }
        this.resolution = resolution;
        this.radius = radius;
        this.upDirection = Updirection;
        this.offset = displacement;
        this.useNoise = useNoise;
        if (scaler == Vector3.zero)
            scaler = Vector3.one;
        this.scaler = scaler;
        this.lod = lod;
        this.mat = mat;


        secondDirection = new Vector3(upDirection.y, upDirection.z, upDirection.x);
        thirdDirection = Vector3.Cross(upDirection, secondDirection);

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        meshData = new MeshData(this.resolution, mr, go.AddComponent<MeshCollider>(), noiseSettings);
        meshFilter = go.AddComponent<MeshFilter>();


        Generate();
    }
    public void Generate()
    {
        for (int i = 0, index = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++, index++)
            {
                Vector2 per = new Vector2(j, i) / (resolution - 1);
                Vector3 positionOnFace = upDirection + (per.x - .5f) * 2 * secondDirection + (per.y - .5f) * 2 * thirdDirection;
                //Debug.Log(positionOnFace);
                meshData.vertices[index] = positionOnFace;
                if (i < resolution - 1 && j < resolution - 1)
                {
                    meshData.AddTriangle(index, index + resolution + 1, index + resolution);
                    meshData.AddTriangle(index, index + 1, index + resolution +1);
                }
            }
        }
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        meshData.Offset(offset);
        // meshData.Normalise();
        /*
        if (useNoise)
            meshData.PositionBasedNoise(go.transform, radius * scaler / 2);
        else
        {
            meshData.Scale(new Vector3(radius, radius, radius));
            meshData.Scale(scaler);
        }*/
        meshData.Scale(new Vector3(radius, radius, radius));
        Vector2 size = new Vector2(1, 1);
        if (useNoise)
        {
            //meshData.Tests();
            size += meshData.PerlinNoise3D(go.transform, radius,scaler.x, 3, 1.3f, 0.1f,  0.05f);
           // meshData.PositionBasedNoise(go.transform, 0.65f * Vector3.one);
        }


        //meshData.Scale(scaler);

        //meshData.DisplacementMap();


        //Texture2D color = new Texture2D(20, 1);

        Material mat2 = new Material(Shader.Find("Test"));

       /* Color[] colors = new Color[20];
        float increment = 1 / 50;
        for (float i = 0f; i < 1; i += 0.05f )
        {
            colors[Mathf.RoundToInt(i * 20)] = go.transform.root.GetComponent<QuadTreePlanet>().EvalColor(i);
            // Debug.Log(colors[i]);
        }

        color.SetPixels(colors);
        color.Apply();*/

        mat2.SetTexture("_texture", go.transform.root.GetComponent<QuadTreePlanet>().color);
        mat2.SetTexture("_normalMap", go.transform.root.GetComponent<QuadTreePlanet>().normalMap);
        Vector2 myVector = new Vector2(radius * scaler.x + size.y, radius * scaler.x *  size.x);
        mat2.SetVector("_minMax", myVector);

//        Debug.Log("Radius: " + myVector.x +" test: " + myVector.y);

        meshData.SetMaterial(mat2);
        meshData.RefreshMesh();

        meshFilter.sharedMesh = meshData.mesh;
    }

    public void Delete()
    {
        MonoBehaviour.Destroy(go);
    }
}
