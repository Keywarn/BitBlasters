using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private Grid grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode parent;

    public PathNode(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = hCost + gCost;
    }

    public override string ToString()
    {
        return ("[" + x + "," + y + "]");
    }
}
