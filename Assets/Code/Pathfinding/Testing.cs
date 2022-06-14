using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid<int> grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<int>(4, 2, 1f, new Vector3(-5f, -5f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetValue(worldPosition, 50);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(grid.GetValue(worldPosition));
        }
    }
}
