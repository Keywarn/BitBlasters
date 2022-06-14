using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid<int> grid;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<int>(10, 3, 1f, new Vector3(-3f, -3f, 0f));

        for (int x = 0; x < 10; ++x)
        {
            for (int y = 0; y < 3; ++y) {

                Instantiate(prefab, grid.GetWorldPosition(x,y), Quaternion.identity);
            }
        }
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
