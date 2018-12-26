using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreePlanet : MonoBehaviour {

    public QuadTree[] meshes;
    [Range(0, 255)]
    public int resolution = 10;
    public float radius = 10;
    public bool refresh;
    public Vector3 storedPos;
    public float distanceToSplit = 100;
    public int maxLod = 6;
    [HideInInspector]
    public NoiseSettings noiseSettings = new NoiseSettings();
    [HideInInspector]
    public Material mat;
    public Texture2D normalMap;
    public Gradient grad;
    public Texture2D color;

    public void Create()
    {
        if (meshes != null)
        {
            foreach (QuadTree meshMade in meshes)
                meshMade.thisSide.Delete();
            meshes = null;
        }
        float modifier = ((resolution - 1) * radius) / 2;
        gameObject.transform.position = storedPos;
        meshes = new QuadTree[] {
            new QuadTree(gameObject, new GameObject(), resolution, radius, transform.up, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings, distanceToSplit/2, mat),
            new QuadTree(gameObject, new GameObject(), resolution, radius, -transform.up, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings, distanceToSplit/2, mat),
            new QuadTree(gameObject, new GameObject(), resolution, radius, -transform.right, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings,distanceToSplit/2, mat),
            new QuadTree(gameObject, new GameObject(), resolution, radius, transform.right, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings, distanceToSplit/2, mat),
            new QuadTree(gameObject, new GameObject(), resolution, radius, transform.forward, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings, distanceToSplit/2, mat),
            new QuadTree(gameObject, new GameObject(), resolution, radius, -transform.forward, 1, maxLod, Vector3.zero, Vector3.zero, noiseSettings, distanceToSplit/2, mat),
        };
        foreach (QuadTree tree in meshes)
            tree.thisSide.go.transform.position = storedPos;


    }
    private void Start()
    {
        storedPos = transform.position;


        color = new Texture2D(20, 1);

        Color[] colors = new Color[20];
        for (float i = 0f; i < 1; i += 0.05f)
        {
            colors[Mathf.RoundToInt(i * 20)] = EvalColor(i);
            // Debug.Log(colors[i]);
        }

        color.SetPixels(colors);
        color.Apply();

        Create();
    }
    private void Update()
    {
        if (refresh)
        {
            refresh = false;
            Create();
        }   
        foreach (QuadTree tree in meshes)
            tree.CheckLod();
    }

    public Color EvalColor(float timeStamp)
    {
        return grad.Evaluate(timeStamp);
    }
}
