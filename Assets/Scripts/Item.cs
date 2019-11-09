using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject _parent;
    private ItemMaster manager;
    private MeshRenderer renderer;

    void Start()
    {

        //親オブジェクトを取得
        _parent = transform.root.gameObject;
        manager = _parent.GetComponent<ItemMaster>();
        renderer = this.GetComponent<MeshRenderer>();

    }

    void Update()
    {
        Debug.Log(manager.isItemShow);
        if (!manager.isItemShow)
        {
            renderer.enabled = (false);
        }
        else
        {
            renderer.enabled = (true);
        }
    }

   void log()
    {
        Debug.Log("aa");
    }
    

}

