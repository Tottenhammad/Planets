using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenYoureWrong : MonoBehaviour {

    public GameObject Player;
    public GameObject DisplayPlane;

    public GameObject Camera;

    public Vector3 scaler = new Vector3();

    public bool x;
    public bool y;
    public bool z;

    Vector3 start;

    private void Awake()
    {
        start = Camera.transform.localPosition;
    }

    private void Update()
    {
        Camera.transform.localPosition = new Vector3(x? (DisplayPlane.transform.position - Player.transform.position).x * scaler.x : start.x,
            y ? (DisplayPlane.transform.position - Player.transform.position).y * scaler.y : start.y,
             z? (DisplayPlane.transform.position - Player.transform.position).z * scaler.z : start.z);


       // Camera.transform.localPosition = (DisplayPlane.transform.position - Player.transform.position) * scaler;

        //Scale down rotation
        Camera.transform.rotation = Quaternion.Euler(Player.transform.rotation.eulerAngles);
        //Camera.transform.Rotate(new Vector3(0, 180, 0));
    }
}
