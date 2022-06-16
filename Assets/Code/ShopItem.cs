using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject prefab;

    private Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = Manager.Instance;

        this.GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<Image>().color = prefab.GetComponent<SpriteRenderer>().color;

        this.GetComponent<Button>().onClick.AddListener(delegate () { Clicked(); });
    }

    private void Update()
    {
        if (manager == null)
        {
            manager = Manager.Instance;
        }
    }

    void Clicked()
    {
        manager.BeginPlacement(prefab);
    }
}
