using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject prefab;

    private Manager manager;
    private Button button;
    private int data;
    // Start is called before the first frame update
    void Start()
    {
        manager = Manager.Instance;
        button = this.GetComponent<Button>();
        data = prefab.GetComponent<Placeable>().data;

        this.GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<Image>().color = prefab.GetComponent<SpriteRenderer>().color;

        button.onClick.AddListener(delegate () { Clicked(); });

        gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ToString();
    }

    private void Update()
    {
        if (manager == null)
        {
            manager = Manager.Instance;
        }

        button.interactable = manager.data >= data && manager.building;

    }

    void Clicked()
    {
        manager.BeginPlacement(prefab);
    }
}
