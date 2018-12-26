using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour {
    [Range(0, 99)]
    public int Speed = 10;

    
    float percentage;
    float counter;
    private void Update()
    {
        percentage = 360 / (100 - Speed);
        transform.localRotation = Quaternion.Euler(0, 0, counter);
        counter += percentage;
        if (counter >= 360)
            counter = 0;
    }
}
