using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(MeshFilter))]
public class Segment : MonoBehaviour {

    // Contains all info and commands for the mesh control
    public MeshData meshData;

    //Could privatise later?
    public float radius;
    public int resolution;
    public int lodLevel;

    public bool active;

    public Vector3 offset;

    // What planet it belongs to
    private QuadTreePlanet planet;

    public Segment[] children;

    public Segment(QuadTreePlanet planet, int resolution, float radius, Vector3 upDir)
    {
        // Creates a new instance of the meshData ready for molding
        meshData = new MeshData(resolution, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), upDir, transform);

        this.radius = radius;
        this.resolution = resolution;

        this.planet = planet;
    }
    public void MakeSegment(QuadTreePlanet planet, int resolution, float radius, Vector3 upDir, int lod, Vector3 offset)
    {
        // Creates a new instance of the meshData ready for molding
        meshData = new MeshData(resolution, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), upDir, transform);
        // Generates Mesh
        meshData.Generate();
        // Offset Mesh To Right Position
        
        meshData.Offset(offset);
        meshData.Normalise();
        // Scakes the mesh to be size of radius
        Debug.Log(lod);
        if (lod > 1)
        {
            meshData.Scale(Vector3.one * (radius / Mathf.Pow(2, lod)));
            meshData.Scale(Vector3.one * Mathf.Pow(2, lod));
        }
        else
            meshData.Scale(Vector3.one * radius);        
        


        //Temp Material
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        // Recalculate normals, set meshFilter/renderer etc...
        meshData.RefreshMesh();


        // Set this segments local variables 
        this.radius = radius;
        this.resolution = resolution;
        this.planet = planet;
        this.lodLevel = lod;
        this.offset = offset;

    }

    public void CheckLod()
    {

        bool check = false;
        if (meshData.meshCol.enabled == false)
        {
            check = true;
            meshData.meshCol.enabled = true;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(player.transform.position, meshData.meshCol.bounds.ClosestPoint(player.transform.position));
        if (check)
            meshData.meshCol.enabled = false;

        if (dist < planet.distanceToSplit && lodLevel != planet.maxLod)
        {
            Split();
        }
       else {
            if (lodLevel > 1 && dist > 100 * planet.distanceToSplit)
            {
                Hide();
                if (children != null)
                {
                    foreach (Segment c in children)
                        c.Hide();
                    //Debug.Log("In");
                }
            }
            else
            {
                Show();
                if (children != null)
                {
                    foreach (Segment child in children)
                        child.Hide();
                    //Debug.Log("In");
                }
            }
        }
        if (children != null)
            foreach (Segment c in children)
                if (c.active)
                    c.CheckLod();
    }




    public void Split()
    {
        if (children == null)
        {
            children = new Segment[4];

            Vector3[] offsets = new Vector3[] {
                    meshData.upDirection + meshData.secondDirection + meshData.thirdDirection + offset * 2,
                    meshData.upDirection + meshData.secondDirection - meshData.thirdDirection + offset * 2,
                    meshData.upDirection - meshData.secondDirection - meshData.thirdDirection + offset * 2,
                    meshData.upDirection - meshData.secondDirection + meshData.thirdDirection + offset * 2
                };
            int count = 0;
            foreach (Vector3 off in offsets)
            {
                GameObject go = new GameObject(name + ": " + meshData.upDirection);
                go.transform.parent = transform;
                Segment s = go.AddComponent<Segment>();
                s.MakeSegment(planet, resolution, radius, meshData.upDirection, lodLevel + 1, off);
                children[count] = s;
                count++;
            }
        }
        else
        {
            foreach (Segment child in children)
                child.Show();
        }
        Hide();
    }



    public void Show()
    {
        meshData.meshRenderer.enabled = true;
        meshData.meshCol.enabled = true;
        active = true;
    }

    public void Hide()
    {
        meshData.meshRenderer.enabled = false;
        meshData.meshCol.enabled = false;
        active = false;
    }
}
