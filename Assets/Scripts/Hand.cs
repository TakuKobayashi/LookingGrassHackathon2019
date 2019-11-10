using UnityEngine;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Hand : MonoBehaviour
{

    GameObject _target;
    public GameObject[] feeds;
    public GameObject handPosition;

    private GameObject feed;



 private void Start() {
        
        StartCoroutine(loop());
        this.feed = this.feeds[0];
    }

    public void setTarget(GameObject target)
    {
        if (_target == null)
        {
            _target = target;
        }

    }

    public void pickupTarget()
    {
        Debug.Log("aaa");
        if (_target)
        {
            StartCoroutine(changeParent());
            Rigidbody rb = _target.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }


    public void releaseTarget()
    {
        if (_target && _target.activeInHierarchy)
        {
            if (_target.transform.parent == transform)
            { //Only reset if we are still the parent
                Rigidbody rb = _target.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                _target.transform.parent = null;
            }
            _target = null;
        }

        GameObject makeFeed = GameObject.Instantiate(feed) as GameObject;
        makeFeed.transform.position = handPosition.transform.position;
    }

    public void clearTarget()
    {
        _target = null;
    }

        //Avoids object jumping when passing from hand to hand.
    IEnumerator changeParent()
    {
        yield return null;
        if (_target != null)
            _target.transform.parent = transform;
    }

    private IEnumerator loop() {
    // ループ
        while (true) {
            // 1秒毎にループします
            yield return new WaitForSeconds(1f);
            getCurrentItem();
        }
    }

    private void getCurrentItem() {
    // 1秒毎に呼ばれます
    Debug.Log("on timer");
    HTTPManager.Instance.Request(
            url: "https://script.google.com/macros/s/AKfycbwBX-3XgnXvKuzhr_DTqMgOwHmrQPuTaa4J22iZN1k2fi1WkHY/exec",
			methods: HTTPMethods.Get,
			onSuccess: (dh) =>
			{
				// SampleModel sample = JsonConvert.DeserializeObject<SampleModel>(dh.text);
				// Debug.Log(int.Parse(sample.val));
                // if(sample.val){
         
                //     this.feed = this.feeds[int.Parse(sample.val)];
                // }else{
                //     this.feed = this.feeds[0];
                // }
                this.feed = this.feeds[0];
			}
		);
}
}