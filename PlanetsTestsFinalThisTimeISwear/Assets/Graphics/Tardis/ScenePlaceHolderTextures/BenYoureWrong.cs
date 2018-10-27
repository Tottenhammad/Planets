using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenYoureWrong : MonoBehaviour {

    public GameObject Player;
    public GameObject DisplayPlane;

    public GameObject Camera;



    private void Update()
    {
        Camera.transform.localPosition = DisplayPlane.transform.position - Player.transform.position;
    }
}
