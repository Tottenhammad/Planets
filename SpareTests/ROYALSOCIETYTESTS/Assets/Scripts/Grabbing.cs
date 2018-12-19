using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FixedJoint))]
public class Grabbing : MonoBehaviour {
    // Can use Parenting but trying jointing first

    FixedJoint joint;

    List<Rigidbody> potentialPickUps = new List<Rigidbody>();

    public string grabTag;



    private void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    private void Update()
    {
        // Input to call drop and grab
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == grabTag)
            potentialPickUps.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == grabTag)
            potentialPickUps.Remove(other.GetComponent<Rigidbody>());
    }

    Rigidbody current;
    void Grab()
    {
        current = NearestRB();

        if (current)
        {
            current.transform.position = transform.position;
            joint.connectedBody = current;
        }
    }

    void Drop()
    {
        if (current)
        {

        }
    }

    Rigidbody NearestRB()
    {
        Rigidbody nearest = null;

        float min = float.MaxValue;
        float distance = 0;

        foreach(Rigidbody rb in potentialPickUps)
        {
            distance = (rb.gameObject.transform.position - transform.position).sqrMagnitude;

            if(distance < min)
            {
                min = distance;
                nearest = rb;
            }
        }
        return nearest;
    }
}
