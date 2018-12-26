using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Rigidbody rb;
    public float speed = 10;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float forward = Input.GetAxis("Vertical") * speed;
        float sideways = Input.GetAxis("Horizontal") * speed;
        forward *= Time.deltaTime;
        sideways *= Time.deltaTime;

        transform.Translate(sideways, 0, forward);


    }
}
