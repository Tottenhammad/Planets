using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree {

    public TerrarinSide thisSide;
    public QuadTree[] children;
    public QuadTree parent;

    public GameObject parentGo;
    public int LOD;
    public int maxLod;
    public bool active = true;

    public Vector3 scaler;
    public NoiseSettings noiseSettings;
    public float distanceToSplit = 100;
    Material mat;
    GameObject go;
    public QuadTree(GameObject parent, GameObject gO, int resolution, float radius, Vector3 Direction, int LOD, int maxLod, Vector3 Displacement, Vector3 scaler, NoiseSettings noiseSettings, float DistanceToSplit, Material mat )
    {
        gO.transform.position = parent.transform.position;
        this.noiseSettings = noiseSettings;
        this.scaler = scaler;
        this.distanceToSplit = DistanceToSplit;
        this.thisSide = new TerrarinSide(parent, gO, LOD, Mathf.Clamp(resolution, 0, 255), radius, Direction, Displacement, this.scaler, noiseSettings, true ? LOD > 0 : false, mat);
        this.LOD = LOD;
        this.maxLod = maxLod;
        this.scaler = scaler;
        this.mat = mat;
        this.parentGo = parent;
        
        this.go = gO;
    }

    public void CheckLod()
    {
        //Mesh.bounds.ClosestPoint
        //float dist = Vector3.Distance(Camera.main.transform.position, (thisSide.meshData.mesh.bounds.ClosestPoint(Camera.main.transform.position)));
        bool check = false;
        if(thisSide.meshData.meshCol.enabled == false)
        {
            check = true;
            thisSide.meshData.meshCol.enabled = true;
        }
        var player = GameObject.FindGameObjectWithTag("Setting").GetComponent<Settings>().planetLodAnchor.transform.position;
        float dist = Vector3.Distance(player, thisSide.meshData.meshCol.bounds.ClosestPoint(player));
        if (check)
            thisSide.meshData.meshCol.enabled = false;
         /*if (dist < distanceToSplit / 2)
         {
             Debug.DrawRay(thisSide.go.transform.TransformPoint(thisSide.meshData.mesh.bounds.ClosestPoint(Camera.main.transform.position)), Camera.main.transform.position, Color.green);
         }
         else
         {
             Debug.DrawRay(thisSide.go.transform.TransformPoint(thisSide.meshData.mesh.bounds.ClosestPoint(Camera.main.transform.position)), Camera.main.transform.position, Color.red);
         }*/
        if (dist < distanceToSplit && LOD != maxLod)
        {

            if (children == null && GameObject.FindGameObjectWithTag("Setting").GetComponent<Settings>().splitted == false)
            {
                float modifier = Mathf.Pow(2, (LOD));
               // Debug.Log(modifier);
                children = new QuadTree[]
                {
                new QuadTree(thisSide.go, new GameObject(), Mathf.RoundToInt(thisSide.resolution/1f), thisSide.radius / 2, thisSide.upDirection, LOD + 1, maxLod, (thisSide.upDirection + thisSide.secondDirection + thisSide.thirdDirection)  + thisSide.offset * 2, Vector3.one * modifier, noiseSettings, distanceToSplit/1f, mat),
                new QuadTree(thisSide.go, new GameObject(), Mathf.RoundToInt(thisSide.resolution/1f), thisSide.radius / 2, thisSide.upDirection, LOD + 1, maxLod, thisSide.upDirection + thisSide.secondDirection - thisSide.thirdDirection  + thisSide.offset * 2,  Vector3.one * modifier, noiseSettings, distanceToSplit/1f, mat),
                new QuadTree(thisSide.go, new GameObject(), Mathf.RoundToInt(thisSide.resolution/1f), thisSide.radius / 2, thisSide.upDirection, LOD + 1, maxLod, thisSide.upDirection - thisSide.secondDirection + thisSide.thirdDirection  + thisSide.offset * 2,  Vector3.one * modifier, noiseSettings, distanceToSplit/1f, mat),
                new QuadTree(thisSide.go, new GameObject(), Mathf.RoundToInt(thisSide.resolution/1f), thisSide.radius / 2, thisSide.upDirection, LOD + 1, maxLod, thisSide.upDirection - thisSide.secondDirection - thisSide.thirdDirection  + thisSide.offset * 2,  Vector3.one * modifier, noiseSettings, distanceToSplit/1f, mat),
                };
                GameObject.FindGameObjectWithTag("Setting").GetComponent<Settings>().splitted = true;
            }
            else
            {
                foreach (QuadTree c in children)
                    c.Show();
            }

            Hide();

        }
        else
        {
            if (LOD > 1 && dist > 2 * distanceToSplit)
            {
                Hide();
                if (children != null)
                {
                    foreach (QuadTree c in children)
                        c.Hide();
                    //Debug.Log("In");
                }
            }
            else
            {
                Show();
                if (children != null)
                {
                    foreach (QuadTree c in children)
                        c.Hide();
                    //Debug.Log("In");
                }
            }

        }

        if (children != null)
            foreach (QuadTree c in children)
                if (c.active)
                    c.CheckLod();


    }
    public void Show()
    {
        thisSide.meshData.meshRenderer.enabled = true;
        thisSide.meshData.meshCol.enabled = true;
        active = true;
    }

    public void Hide()
    {
        thisSide.meshData.meshRenderer.enabled = false;
        thisSide.meshData.meshCol.enabled = false;
        active = false;
    }
}
