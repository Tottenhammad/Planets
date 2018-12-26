using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestingThings : MonoBehaviour {

    DiamondSquare ds;
    public int size = 256;
    public float roughness = 4;
    Texture2D testingTextures;
    private void Awake()
    {
        ds = new DiamondSquare(size, roughness);
        testingTextures = new Texture2D(ds.GetSize(), ds.GetSize(), TextureFormat.ARGB32, false);
        for (int i = 0; i < ds.GetSize(); i++)
        {
            for (int j = 0; j < ds.GetSize(); j++)
            {
                testingTextures.SetPixel(j, i, new Color(ds.Get(j, i), ds.Get(j, i), ds.Get(j, i), 1));
            }
        }
    }
    private void OnValidate()
    {
        ds = new DiamondSquare(size, roughness);
        testingTextures = new Texture2D(ds.GetSize(), ds.GetSize(), TextureFormat.ARGB32, false);
        for (int i = 0; i < ds.GetSize(); i++)
        {
            for (int j = 0; j < ds.GetSize(); j++)
            {
                testingTextures.SetPixel(j, i, new Color(ds.Get(j, i) * 255, ds.Get(j, i), ds.Get(j, i), 1));
            }
        }
    }
    private void Update()
    {

      // Debug.Log(ds.getGrid());
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(10, 10, 60, 60), testingTextures);
    }
}
