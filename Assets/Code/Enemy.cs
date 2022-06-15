using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    private int currentNode = 0;

    public List<Vector3> path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(path.Count > 0)
        {
            // TODO do a lookat function
            transform.position += (path[currentNode] - transform.position).normalized * Time.deltaTime * moveSpeed;

            if (Vector3.Distance(transform.position, path[currentNode]) < 0.1f)
            {  //Next point in list has been reached
                currentNode++;
            }
        }
    }
}
