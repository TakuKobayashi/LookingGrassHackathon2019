using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject characterModelObj;
    [SerializeField] private RuntimeAnimatorController updateAnimator;
    // 移動距離が距離がこれ以下の場合Idleそれ以外は歩くか走るモーション
    [SerializeField] private float changeAnimationDistanceThreshold = 0.01f;
    [SerializeField] private float loiterRandomDistanceRange = 0.5f;
    // うろうろするかどうかの時間の間隔
    [SerializeField] private float loiterSecondSpan = 1.0f;

    private GameObject threedObject;
    private Animator characterAnimator;
    // 到達予定位置
    private Vector3 moveToTargetPosition;

    // カメラ目線になるためのもの
    private Vector3? defaultPreviewTargetPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        this.threedObject = ComponentUtil.InstantiateTo(this.gameObject, characterModelObj);
        this.characterAnimator = this.threedObject.GetComponent<Animator>();
        AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
        animatorOverride.runtimeAnimatorController = updateAnimator;
        this.characterAnimator.runtimeAnimatorController = animatorOverride;
        this.moveToTargetPosition = this.threedObject.transform.position;
        StartCoroutine(LoiterRandomRoutine());
    }

    public void ChangetToTriggerAnimation(string triggerName)
    {
        this.characterAnimator.SetTrigger(triggerName);
    }

    public void InputPreviewTarget(Vector3 targetPosition)
    {
        if(defaultPreviewTargetPosition == null)
        {
            defaultPreviewTargetPosition = targetPosition;
        }
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

    void Update()
    {
        float moveToDistance = (this.moveToTargetPosition - this.transform.position).sqrMagnitude;
        if(moveToDistance < changeAnimationDistanceThreshold)
        {
            this.ChangetToTriggerAnimation("Idle");
        }
    }

    private IEnumerator LoiterRandomRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(loiterSecondSpan);
            bool isLoiter = true;
            int layerCount = this.characterAnimator.layerCount;
            for(int i = 0;i < layerCount; ++i)
            {
                AnimatorStateInfo animState = this.characterAnimator.GetCurrentAnimatorStateInfo(i);
                Debug.Log(animState.IsName("Idle"));
                if (!animState.IsName("Idle"))
                {
                    isLoiter = false;
                    break;
                }
            }
            Debug.Log(isLoiter);
            if (isLoiter)
            {
                LoiterRandom();
            }
        }
    }

    private void LoiterRandom()
    {
        Vector3 moveToPostion = this.threedObject.transform.position;
        moveToPostion.x = UnityEngine.Random.Range(moveToPostion.x - loiterRandomDistanceRange, moveToPostion.x + loiterRandomDistanceRange);
        moveToPostion.z = UnityEngine.Random.Range(moveToPostion.z - loiterRandomDistanceRange, moveToPostion.z + loiterRandomDistanceRange);
        this.threedObject.transform.LookAt(moveToPostion);
        this.ChangetToTriggerAnimation("Walk");
        StartCoroutine(this.MoveCharacterAnimation(moveToPostion, 2.0f, () =>
        {
            this.threedObject.transform.LookAt(defaultPreviewTargetPosition.GetValueOrDefault());
        }));
    }

    public IEnumerator MoveCharacterAnimation(Vector3 toPosition, float reachedTime, Action onMoveCompleted = null)
    {
        Vector3 beforePostion = this.threedObject.transform.position;
        this.moveToTargetPosition = toPosition;
        float second = 0;
        while (second < reachedTime)
        {
            this.threedObject.transform.position = Vector3.Lerp(beforePostion, toPosition, second / reachedTime);
            second += Time.deltaTime;
            yield return null;
        }
        if (onMoveCompleted != null)
        {
            onMoveCompleted();
        }
    }
}
