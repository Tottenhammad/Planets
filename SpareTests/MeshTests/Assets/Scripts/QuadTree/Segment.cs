using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(MeshFilter))]
public class Segment : MonoBehaviour
{

    // Contains all info and commands for the mesh control
    public MeshData meshData;

    //Could privatise later?
    public float radius;
    public int resolution;
    public int lodLevel;

    public bool active;

    public Vector3 offset;

    // What planet it belongs to
    public QuadTreePlanet planet;

    public Segment[] children;

    public Place place;

    public bool splitting = false;
    Vector3 upDir;
    public Segment(QuadTreePlanet planet, int resolution, float radius, Vector3 upDir)
    {
        // Creates a new instance of the meshData ready for molding
        meshData = new MeshData(resolution, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), upDir, transform, planet.noiseSettings);

        this.radius = radius;
        this.resolution = resolution;

        this.planet = planet;
    }
    public void MakeSegment(QuadTreePlanet planet, int resolution, float radius, Vector3 upDir, int lod, Vector3 offset, Place place)
    {
        // Creates a new instance of the meshData ready for molding
        meshData = new MeshData(resolution, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), upDir, transform, planet.noiseSettings);
        // Generates Mesh
        meshData.Generate();
        // Offset Mesh To Right Position

        meshData.Offset(offset);
        meshData.Normalise();
        //Refenrce to know its plcase for prelaoding
        this.place = place;

        // Scakes the mesh to be size of radius
        meshData.Noise();
        if (lod > 1)
        {
            meshData.Scale(Vector3.one * (radius / Mathf.Pow(2, lod)));
            meshData.Scale(Vector3.one * Mathf.Pow(2, lod));
        }
        else
            meshData.Scale(Vector3.one * radius);



        this.upDir = upDir;

        //Temp Material
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        // Recalculate normals, set meshFilter/renderer etc...
        meshData.RefreshMesh();

        //meshData.SaveMesh();
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


        if ((dist < planet.distanceToSplit && lodLevel != planet.maxLod))
        {
            StartCoroutine(Split(true));

        }
        /*else if (dist < planet.preLoadDistance && lodLevel != planet.preMaxLod && dist > planet.distanceToSplit && planet.preLoad && lodLevel < planet.maxLod)
        {
            StartCoroutine(Split(false));
            if (children != null)
                foreach (Segment c in children)
                {
                    if (c != null)
                        c.CheckLod();

                }

        }*/
        else
        {

            if (lodLevel > 1 && dist > planet.distanceToSplit)
            {
                Hide();
                if (children != null)
                {
                    StartCoroutine(HideChildren());
                    //Debug.Log("In");
                }
            }
            else
            {
                Show();
                if (children != null)
                {
                    StartCoroutine(HideChildren());
                    //Debug.Log("In");
                }
            }
        }


    }


    public void CallSplit(bool display = false)
    {
        StartCoroutine(Split(display));
    }
    IEnumerator HideChildren()
    {
        foreach (Segment child in children)
        {
            yield return new WaitForEndOfFrame();
            if (child != null)
                child.Hide();
        }
    }

    IEnumerator Split(bool display)
    {
        if (splitting)
            yield break;
        else
            splitting = true;
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
                yield return new WaitUntil(() => planet.fireSplit == Place.BOTTOMRIGHT);
                GameObject go = new GameObject(name + ": " + meshData.upDirection);
                go.transform.parent = transform;
                Segment s = go.AddComponent<Segment>();
                s.MakeSegment(planet, resolution, radius, meshData.upDirection, lodLevel + 1, off, planet.test[count]);
                children[count] = s;
                count++;
            }
        }
        else
        {
            if (display)
            {
                foreach (Segment child in children)
                {
                    if (child != null)
                        child.Show();
                }
            }
        }
        if (display)
            Hide();
        if (children != null)
            foreach (Segment c in children)
                if (c)
                    c.CheckLod();
        splitting = false;
    }


    public void QueueChildrenForSplit()
    {
        if (lodLevel != planet.preMaxLod - 1)
        {
            foreach (Segment c in children)
                Camera.main.GetComponent<SplitController>().NextLoop.Add(c);
        }
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

    public void Delete(bool killChildren, int lodToKIll = 2)
    {
        if (children != null && killChildren)
            foreach (Segment child in children)
                child.Delete(true);
        if(lodLevel >= lodToKIll)
            Destroy(transform.gameObject);
    }

    public void ResetSegment()
    {
        // Creates a new instance of the meshData ready for molding
        meshData = new MeshData(resolution, GetComponent<MeshRenderer>(), GetComponent<MeshCollider>(), GetComponent<MeshFilter>(), upDir, transform, planet.noiseSettings);
        // Generates Mesh
        meshData.Generate();
        // Offset Mesh To Right Position

        meshData.Offset(offset);
        meshData.Normalise();
        //Refenrce to know its plcase for prelaoding
        this.place = place;

        // Scakes the mesh to be size of radius
        meshData.Noise();
        if (lodLevel > 1)
        {
            meshData.Scale(Vector3.one * (radius / Mathf.Pow(2, lodLevel)));
            meshData.Scale(Vector3.one * Mathf.Pow(2, lodLevel));
        }
        else
            meshData.Scale(Vector3.one * radius);



        //Temp Material
        meshData.SetMaterial(new Material(Shader.Find("Standard")));
        // Recalculate normals, set meshFilter/renderer etc...
        meshData.RefreshMesh();
    }
}
