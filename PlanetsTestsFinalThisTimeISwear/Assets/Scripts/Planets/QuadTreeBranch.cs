using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class QuadTreeBranch : MonoBehaviour {
    // Mesh Variables
    public MeshCollider col;
    public MeshFilter filter;
    public MeshRenderer renderer;
    public Mesh mesh;
    // Quadtree Variables
    public QuadTreeBranch parent;
    public List<QuadTreeBranch> children;
    // Int Variables
    public int lod;
    public int meshResolution;
    public int iIndex, jIndex;
    // Float Variables
    public float radius;
    // Arrays
    Vector3[] vertices;
    int[] indices;
    // Enum
    public cubeSide sideIndex;
    //bool
    public bool generated = false;
    public bool childrenVisible = false;





    // Construcotr class
    public void create(QuadTreeBranch parent, float radius, int resolution, cubeSide sideIndex, int i, int j)
    {
        col = GetComponent<MeshCollider>();
        filter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();

        this.parent = parent;
        this.radius = radius;
        this.meshResolution = resolution;

        this.iIndex = i;
        this.jIndex = j;

        this.sideIndex = sideIndex;
        Generate();
    }
    // Genertate vertices
    public void Generate()
    {
        vertices = new Vector3[meshResolution * meshResolution];
        float increment = (1 / meshResolution - 1) / Mathf.Pow(2, lod);

        for(int i = 0, index = 0; i < meshResolution; i++)
        {
            for(int j = 0; j < meshResolution; j++, index++)
            {
                float xPos = (float)j * increment - 0.5f + ((float)iIndex / Mathf.Pow(2, lod));
                float yPos = 0.5f;
                float zPos = (float)i * increment - 0.5f + ((float)jIndex / Mathf.Pow(2, lod));

                switch (sideIndex)
                {
                    case cubeSide.TOP:
                        vertices[index] = new Vector3(xPos, yPos, zPos);
                        break;
                    case cubeSide.BOTTOM:
                        vertices[index] = new Vector3(xPos, -yPos, -zPos);
                        break;
                    case cubeSide.LEFT:
                        vertices[index] = new Vector3(-yPos, zPos, -xPos);
                        break;
                    case cubeSide.RIGHT:
                        vertices[index] = new Vector3(yPos, zPos, xPos);
                        break;
                    case cubeSide.FORWARD:
                        vertices[index] = new Vector3(-xPos, zPos, yPos);
                        break;
                    case cubeSide.BACK:
                        vertices[index] = new Vector3(xPos, zPos, -yPos);
                        break;
                }
                vertices[index] *= radius;
            }
        }
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        if (generated)
        {
            renderer.enabled = true;
            col.enabled = true;
            return;
        }

        int[] buffer = new int[((meshResolution - 1) * (meshResolution - 1) * 6)];

        mesh = new Mesh();

        for (int triIndex = 0, vertIndex = 0, i = 0; i < meshResolution - 1; i++, vertIndex++)
        {
            for (int j = 0; j < meshResolution - 1; j++, triIndex += 6, vertIndex++)
            {
                buffer[triIndex] = vertIndex;
                buffer[triIndex + 1] = vertIndex + meshResolution;
                buffer[triIndex + 2] = vertIndex + 1;
                buffer[triIndex + 3] = vertIndex + meshResolution;
                buffer[triIndex + 4] = vertIndex + meshResolution + 1;
                buffer[triIndex + 5] = vertIndex + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = buffer;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        filter.sharedMesh = mesh;
        col.sharedMesh = mesh;
        generated = true;
    }

    public void SubDivideSurface()
    {
        if (children != null)
            return;

        for (int i = 0, index = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++, index++)
            {
                QuadTreeBranch newSide = new QuadTreeBranch();
                newSide.create(this, radius, meshResolution, sideIndex, (iIndex *2) + j, (jIndex * 2 ) + i);
                newSide.transform.parent = newSide.transform;

                children.Add(newSide);
            }
        }

        Hide();
    }

    public void UpdateLod()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, mesh.bounds.ClosestPoint(Camera.main.transform.position)); ;

        if(distance < 100 / lod)
        {
            if(children.Count < 4)
            {
                SubDivideSurface();
                Hide();
            }
            else
            {
                foreach(QuadTreeBranch child in children)
                {
                    child.Show();
                }
                childrenVisible = true;
                Hide();
            }
        }
        else
        {
            if(children.Count < 4 || !childrenVisible)
            {
                return;
            }
            else
            {
                foreach (QuadTreeBranch child in children)
                {
                    child.Hide();
                }
                Show();
                childrenVisible = false;
            }
        }
    }


    // Utilities
    public void Hide()
    {
        renderer.enabled = false;
        col.enabled = false;
        generated = false;
    }
    public void Show()
    {

        renderer.enabled = true;
        col.enabled = true;
        generated = true;

    }
}
