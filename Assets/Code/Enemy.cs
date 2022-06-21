using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public float moveSpeed = 1;
    public int data = 5;
    public int populationCost = 1;
    public int minimumRound = 0;
    public float maxGap = 1f;
    public float minGap = 0.1f;
    
    public float startProbability = 0f;
    public float roundProbabiltyMultiplier = 1.1f;
    public float roundGapModifier = 0.05f;


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
            direction.z = 0;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
    }

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            health--;
            if (health <= 0)
            {
                Kill();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Finish")
        {
            Kill();
        }
    }

    void Kill()
    {
        Manager.Instance.mobDied(this);
        Destroy(gameObject);
    }
}
