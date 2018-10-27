﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour {

    public GravitySource source;

    private void Update()
    {
        source.Attract(transform);
    }
}
