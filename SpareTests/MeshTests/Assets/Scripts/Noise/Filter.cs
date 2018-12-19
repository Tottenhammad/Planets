using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter{
    Noise noise = new Noise();
    NoiseSettings noiseSettings;

    public Filter(NoiseSettings noiseSettings)
    {
        this.noiseSettings = noiseSettings;
    }
    public float Eval(Vector3 point)
    {
        float value = 0;
        float freq = noiseSettings.startRough;
        float amp = 1;
        for(int i =0; i < noiseSettings.layerCount; i++)
        {
            float j = noise.Evaluate(point * freq + noiseSettings.center);
            value += ((1 + j) / 2) * amp;
            freq *= noiseSettings.rough;
            amp *= noiseSettings.persist;
        }
        value = Mathf.Max(0, value - noiseSettings.min);
        return value * noiseSettings.strength;
    }
}
