using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestongThings : MonoBehaviour {
    public Texture2D tests;
    public LibNoise.Generator.Perlin planes = new LibNoise.Generator.Perlin(0.03, 2.7, 0.5, 6, 1337, LibNoise.QualityMode.Low);
    public LibNoise.Generator.RidgedMultifractal mountains = new LibNoise.Generator.RidgedMultifractal(0.003, 6.5, 2, 1337, LibNoise.QualityMode.Medium);


}
