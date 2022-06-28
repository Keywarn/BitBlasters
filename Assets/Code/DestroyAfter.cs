using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float time;
    public bool easeIn;
    public bool easeOut;

    private float timer;
    private float halfTime;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        halfTime = time * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (easeIn && timer < halfTime)
        {
            float newAlpha = Mathf.Lerp(0, 1, timer / halfTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);
        }
        else if(easeOut && timer > halfTime)
        {
            float newAlpha = Mathf.Lerp(1, 0, (timer-halfTime)/ halfTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);
        }

        if(timer >= time)
        {
            Destroy(gameObject);
        }
    }
}
