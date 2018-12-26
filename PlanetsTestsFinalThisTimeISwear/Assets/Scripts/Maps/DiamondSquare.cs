using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare {

    int size;
    float roughness;
    int max;
    float[] grid;

    public DiamondSquare(int size, float roughness)
    {
        this.size = Mathf.RoundToInt((Mathf.Pow(2, size)) + 1);
        this.max = this.size - 1;
        this.roughness = roughness;
        this.grid = new float[this.size * this.size];
        for(var i = 0; i < this.size * this.size; i++)
        {
            this.grid[i] = -1f;
        }
        MakeGrid(this.size);
        Divide(max);
    }

    public void Set(int x, int y, float value)
    {
        grid[x + size * y] = value;
    }

    public float Get(int x, int y)
    {
        if (x < 0 || x > max || y < 0 || y > max)
            return 1;
        return grid[x + size * y];
    }

    public void Divide(int size)
    {
        int x, y, half = size / 2;
        float scale = roughness * size;

        if (half < 1)
            return;

        // Split Into Squares
       for(int i = half; i < max; i += size)
        {
            for (int j = half; j < max; j += size)
            {
                var squareScale = Random.Range(0, 1) * scale * 2 - scale;
                Square(j, i, half, squareScale);
            }
        }

        // Split Into Diamond
        for (int i = 0; i < max + 1; i += half)
        {
            for (int j = (i + half) % size; j < max + 1; j += size)
            {
                var diamondScale = Random.Range(0, 1) * scale * 2 - scale;
                Square(j, i, half, diamondScale);
            }
        }
        Divide(size / 2);
    }

    public void Square(int x, int y, int size, float scale)
    {
        var top_left = Get(x - size, y - size);
        var top_right = Get(x + size, y - size);
        var bottom_left = Get(x + size, y + size);
        var bottom_right = Get(x - size, y + size);

        var average = ((top_left + top_right + bottom_left + bottom_right) / 4);
        Set(x, y, average + scale);
    }

    public void Diamond(int x, int y, int size, float scale)
    {
        var top = Get(x, y - size);
        var right = Get(x + size, y);
        var bottom = Get(x, y + size);
        var left = Get(x - size, y);

        var average = ((top + right + bottom + left) / 4);
        Set(x, y, average + scale);
    }

    public void MakeGrid(int size)
    {
     //   Debug.Log("Before");
        Set(0, 0, max);
        Set(max, 0, max / 2);
        Set(max, max, 0);
        Set(0, max, max / 2);

    }
    public float[] getGrid()
    {
        return grid;
    }
    public int GetSize()
    {
        return size;
    }

}
