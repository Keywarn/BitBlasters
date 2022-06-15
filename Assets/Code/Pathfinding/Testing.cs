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

            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

            if (pathfinding != null)
            {
                for (int i = 0; i < path.Count -1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) + transform.position + Vector3.one * 0.5f, new Vector3(path[i+1].x, path[i+1].y) + transform.position + Vector3.one * 0.5f, Color.green, 5f);
                }
            }
        }

        //if (Input.GetMouseButton(1))
        //{
        //    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log(grid.GetValue(worldPosition));
        //}
    }
}
