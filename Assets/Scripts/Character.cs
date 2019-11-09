using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject characterModelObj;

    private GameObject threedObject;

    // Start is called before the first frame update
    void Start()
    {
        this.threedObject = ComponentUtil.InstantiateTo(this.gameObject, characterModelObj);
    }

    public void InputPreviewTarget(Vector3 targetPosition)
    {
        StartCoroutine(LookAtPreview(targetPosition));
    }

    private IEnumerator LookAtPreview(Vector3 targetPos)
    {
        while(this.threedObject == null)
        {
            yield return null;
        }
//        this.threedObject.transform.position = targetPos;
        this.threedObject.transform.position = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
