using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SpaceShip : MonoBehaviour {

    public float speed = 100.0f;
    public float throttle = 0.0f;
    public float throttleIncrement = 0.01f;
    public float maxMagnitude = 50;
    public float speedDropOff = 0.1f;
    public float jumpOffstet = 10;

    public bool inUse = false;
    public bool flightMode = false;
    public bool jumpMode = false;

    Vector3 jumpLocation;
    Quaternion jumpRotation;

    public Camera flightCam;

    public GameObject pilot;
    public Camera pilotCam;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnterShip(GameObject.FindGameObjectWithTag("Player"));
    }

    public void EnterShip(GameObject pilot)
    {
        this.pilot = pilot;
        pilotCam = Camera.main;
        this.inUse = true;

        ControlFlight(true);
    }
    public void ControlFlight(bool fly)
    {

        this.flightMode = fly;
        flightCam.enabled = fly;
        Camera.main.enabled = !fly;
        this.pilot.GetComponent<MeshRenderer>().enabled = !fly;
        this.pilot.GetComponent<BoxCollider>().enabled = !fly;


        if (fly)
            GameObject.FindGameObjectWithTag("Setting").GetComponent<Settings>().planetLodAnchor = gameObject;
        else
            GameObject.FindGameObjectWithTag("Setting").GetComponent<Settings>().planetLodAnchor = pilot;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !jumpMode)
        {
            RaycastHit hit;
            if(Physics.Raycast(flightCam.transform.position, flightCam.transform.forward, out hit, Mathf.Infinity))
            {
                jumpLocation = hit.point + -(transform.forward * jumpOffstet);
                Vector3 GravityUp = (transform.position - hit.transform.position).normalized;

                jumpRotation = Quaternion.FromToRotation(transform.up, GravityUp) * transform.rotation;
                jumpMode = true;
            }
        }
        if (inUse && flightMode && !jumpMode)
        {
            pilot.transform.position = transform.position;
            if (Input.GetKey(KeyCode.LeftShift))
                throttle += throttleIncrement;
            if (Input.GetKey(KeyCode.Space))
                throttle -= throttleIncrement;

            throttle = Mathf.Clamp(throttle, 0, 1);
            Vector3 camMovement = transform.position - transform.forward * 3 + Vector3.up * 1.25f;
            float bias = 0.9f;
            flightCam.transform.position = flightCam.transform.position * bias + camMovement * (1 - bias);
            flightCam.transform.LookAt(transform.position + transform.forward * (speed / 3));

            transform.position += transform.forward * Time.deltaTime * (speed * throttle);
           // if (rb.velocity.magnitude < maxMagnitude)
              //  rb.AddForce(transform.forward * Time.deltaTime * (speed * throttle));
            // Increase speed based on diretion maybe????


            transform.Rotate(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
            flightCam.transform.rotation =  Quaternion.Lerp(flightCam.transform.rotation, transform.rotation, 1f);
        }else if(inUse && flightMode && jumpMode)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, jumpRotation, 1f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, jumpLocation, 1 * Time.deltaTime);

            Debug.Log(Vector3.Distance(transform.position, jumpLocation));
            if (Vector3.Distance(transform.position, jumpLocation) < 50f)
            {
                jumpMode = false;
                transform.position = jumpLocation;
                throttle = 0;
            }

        }

    }
}
