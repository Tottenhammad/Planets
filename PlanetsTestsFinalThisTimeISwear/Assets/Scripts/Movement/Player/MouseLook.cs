using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    Vector2 look;
    Vector2 smoothLook;

    public float smoothingScale = 3;
    public float sens = 5.0f;

    Transform character;

    private void Start()
    {
        character = transform.parent.transform;
    }

    private void Update()
    {
        Vector2 mouseVector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseVector = Vector2.Scale(mouseVector, new Vector2(sens * smoothingScale, sens * smoothingScale));

        smoothLook.x = Mathf.Lerp(smoothLook.x, mouseVector.x, 1 / smoothingScale);
        smoothLook.y = Mathf.Lerp(smoothLook.y, mouseVector.y, 1 / smoothingScale);
        look += smoothLook;
        transform.localRotation = Quaternion.AngleAxis(-look.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(look.x, character.up);
    }
}
