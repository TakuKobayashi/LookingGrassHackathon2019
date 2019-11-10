using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject eatEffect;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "feed")
        {
            Destroy(other.gameObject);
            GameObject makeEffect = GameObject.Instantiate(eatEffect) as GameObject;
            makeEffect.transform.position = this.transform.position;
            makeEffect.transform.position += offset;
            makeEffect.GetComponent<ParticleSystem>().Play();
 
        }
     }
}
