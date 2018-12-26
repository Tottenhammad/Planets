using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum cubeSide { TOP, BOTTOM, LEFT, RIGHT, FORWARD, BACK}
public class Planet : MonoBehaviour {

    public float radius;
    public int meshResolution;
    public int lod;

    public List<QuadTreeBranch> sideList;

    private void Start()
    {
        Generate();
    }
    public void Generate()
    {
        for (int index = 0; index < 6; index++)
        {
            for (int i = 0; i < (int)Mathf.Pow(2, lod); i++)
            {
                for (int j = 0; j < (int)Mathf.Pow(2, lod); j++)
                {
                    GameObject gO = new GameObject(i + "_" + j + "_" + index);
                    QuadTreeBranch branch = gO.AddComponent<QuadTreeBranch>();
                    branch.create(null, radius, meshResolution, (cubeSide)index, j, i);
                    gO.transform.parent = this.transform;

                    sideList.Add(branch);
                }
            }
        }
    }
}
