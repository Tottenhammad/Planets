using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public GameObject planetLodAnchor;

    public bool splitted = false;

    private void Start()
    {
        StartCoroutine(testing());
    }

    IEnumerator testing()
    {
        while (true){
            yield return new WaitForSeconds(1 * Time.deltaTime);
            splitted = false;
        }
    }
}
