using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NoiseSettings {

    public Vector3 noiseCenter;
    [Range(1, 8)]
    public int numberLayers = 4 ;


    public float strength = 1;
    public float roughness = 1;
    public float persistence = 0.5f;
    public float baseRoughness = 1;
    public float minValue = 10;


}
