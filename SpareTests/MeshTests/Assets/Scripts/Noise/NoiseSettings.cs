using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NoiseSettings{
    public float strength = 1;
    public float rough = 2;
    public int layerCount = 1;
    public float persist = 0.5f;
    public float startRough = 1;
    public float min;
    public Vector3 center;
}
