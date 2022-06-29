using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPN : Placeable
{
    public float delay;
    public float scaleTime;
    public float finalScale;
    public int damage;

    private float currentScaleTime = 0;
    private bool scaling = false;
    private bool shrinking = false;


    private Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = Manager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Only do something if the VPN has been put down
        if (!isEnabled)
        {
            return;
        }

        if (scaling)
        {
            currentScaleTime += Time.deltaTime;

            if (!shrinking)
            {
                float factor = Mathf.Lerp(1f, finalScale, currentScaleTime / scaleTime);
                gameObject.transform.localScale = Vector3.one * factor;

                if (currentScaleTime >= scaleTime)
                {
                    shrinking = true;
                    currentScaleTime = 0;
                    // Get all mobs


                    // For each mob apply damage
                }
            }

            // Shrinking animation
            else
            {
                float factor = Mathf.Lerp(finalScale, 0f, currentScaleTime * 2f / scaleTime);
                gameObject.transform.localScale = Vector3.one * factor;

                if (currentScaleTime * 2f >= scaleTime)
                {
                    Destroy(gameObject);
                }
            }

        }

        // Check we are not building before finding target
        else if(!manager.building)
        {
            delay -= Time.deltaTime;

            if(delay <= 0f)
            {
                scaling = true;
            }
        }
    }

}
