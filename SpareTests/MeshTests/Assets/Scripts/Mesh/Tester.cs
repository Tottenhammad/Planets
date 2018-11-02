using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(MeshFilter))]
public class Tester : MonoBehaviour {

    private MeshData meshData;

    public int res;
    public Vector3 scale = Vector3.one;
    public float noiseStrength;
	void Start () {
        meshData = new MeshData(25, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), transform.up, transform);
        meshData.Generate();
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        meshData.RefreshMesh();
	}


    private void OnValidate()
    {
        meshData = new MeshData(res, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), transform.up, transform);
        meshData.Generate();
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        meshData.Scale(scale);
      //  meshData.Noise(noiseStrength);
        meshData.RefreshMesh();
    }

}
