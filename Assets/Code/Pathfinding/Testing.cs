using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(15, 10, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);

            List<Vector3> path = pathfinding.FindPath(transform.position, worldPosition);

            if (pathfinding != null)
            {
                for (int i = 0; i < path.Count -1; i++)
                {
                    Debug.DrawLine(path[i], path[i+1], Color.green, 5f);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetNode(worldPosition).isWalkable = false;
        }
    }
}
