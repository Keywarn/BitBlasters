using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int gridWidth, gridHeight;

    public int startX, startY, endX, endY;

    private Pathfinding pathfinding;

    private Vector3 startPosition, endPosition;

    List<Vector3> path;
    private bool pathfindingDirty;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0f);

        pathfinding = new Pathfinding(gridWidth, gridHeight, transform.position);

        startPosition = pathfinding.GetGrid().GetWorldPosition(startX, startY);
        endPosition = pathfinding.GetGrid().GetWorldPosition(endX, endY);

        pathfindingDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfindingDirty || path.Count == 0)
        {
            path = pathfinding.FindPath(startPosition, endPosition);
            pathfindingDirty = false;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.green);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetNode(worldPosition).isWalkable = false;
            pathfindingDirty = true;
        }
    }
}
