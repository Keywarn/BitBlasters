using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBlaster : Placeable
{
    private Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = Manager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
      // Check we are not building before finding target
      if(!manager.building)
        {
            // Get target
            // look at
            // shoot if cooldown is over
            // reset cooldown
        }
    }
}
