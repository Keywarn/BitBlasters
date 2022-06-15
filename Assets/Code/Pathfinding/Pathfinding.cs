using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private const bool CAN_TRAVEL_DIAGONAL = true;

    private Grid grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height, Vector3 position)
    {
        grid = new Grid(width, height, 1f, position);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetNode(startX, startY);
        PathNode endNode = grid.GetNode(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.parent = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbour)) continue;
                if(!neighbour.isWalkable)
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int newGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                if(newGCost < neighbour.gCost)
                {
                    neighbour.parent = currentNode;
                    neighbour.gCost = newGCost;
                    neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour)) openList.Add(neighbour);
                }
            }
        }

        // Completed search but no path
        return null;
    }

    private List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (node.x - 1 >= 0)
        {
            neighbours.Add(grid.GetNode(node.x - 1, node.y));

            if (CAN_TRAVEL_DIAGONAL)
            {
                if (node.y - 1 >= 0) neighbours.Add(grid.GetNode(node.x - 1, node.y - 1));
                if (node.y + 1 < grid.GetHeight()) neighbours.Add(grid.GetNode(node.x - 1, node.y + 1));
            }
        }

        if (node.x + 1 < grid.GetWidth())
        {
            neighbours.Add(grid.GetNode(node.x + 1, node.y));

            if (CAN_TRAVEL_DIAGONAL)
            {
                if (node.y - 1 >= 0) neighbours.Add(grid.GetNode(node.x + 1, node.y - 1));
                if (node.y + 1 < grid.GetHeight()) neighbours.Add(grid.GetNode(node.x + 1, node.y + 1));
            }
        }

        if (node.y - 1 >= 0) neighbours.Add(grid.GetNode(node.x, node.y - 1));
        if (node.y + 1 < grid.GetHeight()) neighbours.Add(grid.GetNode(node.x, node.y + 1));

        return neighbours;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        if (CAN_TRAVEL_DIAGONAL)
        {
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        else
        {
            return (xDistance + yDistance) * MOVE_STRAIGHT_COST;
        }

    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowest = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowest.fCost)
            {
                lowest = pathNodeList[i];
            }
        }

        return lowest;
    }

    public Grid GetGrid()
    {
        return grid;
    }
}
