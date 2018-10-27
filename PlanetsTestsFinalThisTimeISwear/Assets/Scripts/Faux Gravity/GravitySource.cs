using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour {
    public float GravityStrength = -9.8f;
    public float radius = 100;
    public void Attract(Transform attractee)
    {
        Vector3 gravityUp = (attractee.position - transform.position).normalized;
        Vector3 sourceUp = attractee.up;

        attractee.GetComponent<Rigidbody>().AddForce(gravityUp * GravityStrength * attractee.GetComponent<Rigidbody>().mass);

        Quaternion rot = Quaternion.FromToRotation(sourceUp, gravityUp) * attractee.rotation;

        attractee.rotation = Quaternion.Slerp(attractee.rotation, rot, 1f * Time.deltaTime);
    }
    public void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider col in hitColliders)
        {
            if(col.gameObject.GetComponent<AffectedByGravity>())
                if(col.GetComponent<AffectedByGravity>().isActiveAndEnabled && col.gameObject.GetComponent<AffectedByGravity>().affected)
                    Attract(col.transform);
        }
    }
}
