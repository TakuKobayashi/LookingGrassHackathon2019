using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class HTTPSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		HTTPManager.Instance.Request(
            url: "https://asia-northeast1-lookinggrasshackathon2019.cloudfunctions.net/sample",
			methods: HTTPMethods.Get,
			onSuccess: (dh) =>
			{
				//				JsonUtility.FromJson<>(dh.text);
				Debug.Log(dh.text);
				SampleModel sample = JsonConvert.DeserializeObject<SampleModel>(dh.text);
			}
		);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
}
