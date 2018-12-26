using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

    Light light;
    public float maxIntensity = 1;
    public float minIntensity = 0;
    public float increment = 0.01f;
    private void Start()
    {
        light = GetComponent<Light>();
    }


    private void Update()
    {
        light.intensity += increment;
        if ((light.intensity <= minIntensity && increment < 0) ||  (light.intensity >= maxIntensity && increment > 0))
        {
            increment = -increment;
        }
        
    }
}
