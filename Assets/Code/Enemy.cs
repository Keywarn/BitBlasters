using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public int data;
    public int health;

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
            transform.position += (path[currentNode] - transform.position).normalized * Time.deltaTime * moveSpeed;

            if (Vector3.Distance(transform.position, path[currentNode]) < 0.1f)
            {  //Next point in list has been reached
                currentNode++;
            }

            // Look at
            Vector3 direction = path[currentNode] - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COllision");
        if (col.gameObject.tag == "Finish")
        {
            Manager.Instance.mobDied(this);
            Destroy(gameObject);
        }
    }
}
