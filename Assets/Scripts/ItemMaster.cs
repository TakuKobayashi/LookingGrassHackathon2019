using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : MonoBehaviour
{

    public bool isItemShow;
    // Start is called beore the first frame update
    void Start()

    {
        isItemShow = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        this.isItemShow = true;
    }

    private void OnTriggerExit(Collider col)
    {
        this.isItemShow = false;
    }
}
