using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public int data = 0;
    public int refundData = 0;
    public bool isActive = false;
    public bool canBeReplaced = false;
    public bool canReplace = false;
    public string enemyTag = "Enemy";
    protected bool isEnabled = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnabled()
    {
        isEnabled = true;
    }
}
