using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashPreventer : MonoBehaviour {


    public float detectionDistance;
    public float offset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3[] directions = new Vector3[] { transform.up, -transform.up, transform.right, -transform.right, transform.forward, -transform.forward};

        foreach (Vector3 dir in directions)
            if (CheckHit(dir))
                transform.position += -transform.TransformDirection(dir) * offset;
	}

    bool CheckHit(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit, detectionDistance))
            return true;
        else
            return false;
    }
}
