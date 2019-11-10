using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject characterModelObj;
    [SerializeField] private RuntimeAnimatorController updateAnimator;

    private GameObject threedObject;
    private Animator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.threedObject = ComponentUtil.InstantiateTo(this.gameObject, characterModelObj);
        this.characterAnimator = this.threedObject.GetComponent<Animator>();
        AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
        animatorOverride.runtimeAnimatorController = updateAnimator;
        this.characterAnimator.runtimeAnimatorController = animatorOverride;
        this.ChangetToWalkAnimation();
    }

    private IEnumerator setTimeout(float second)
    {
        yield return new WaitForSeconds(second);
    }

    public void ChangetToWalkAnimation()
    {
        this.characterAnimator.SetTrigger("Walk");
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
        //this.threedObject.transform.position = targetPos;
        this.threedObject.transform.LookAt(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
