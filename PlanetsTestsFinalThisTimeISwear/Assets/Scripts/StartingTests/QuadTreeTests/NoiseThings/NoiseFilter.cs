using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter {
    Noise noise = new Noise();

    public float Evaluate(Vector3 position, NoiseSettings settings, bool limit)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        for(int i = 0; i < settings.numberLayers; i++)
        {
            float v = noise.Evaluate(position * frequency + settings.noiseCenter);
            noiseValue += (v + 1) / 2 * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
       // if (limit)
         //   noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
