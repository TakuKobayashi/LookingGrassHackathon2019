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
    [SerializeField] private float loiterRandomDistanceRange = 2.0f;
    // うろうろするかどうかの時間の間隔
    [SerializeField] private float loiterSecondSpan = 5.0f;

    private GameObject threedObject;
    private Animator characterAnimator;
    // 到達予定位置
    private Vector3 moveToTargetPosition;

    // カメラ目線になるためのもの
    private Vector3? defaultPreviewTargetPosition = null;

    private CharacterState currentState = CharacterState.Idle;

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

    public void ChangetToTriggerAnimation(CharacterState state)
    {
        this.currentState = state;
        this.characterAnimator.SetTrigger(state.ToString());
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
        float moveToDistance = (this.moveToTargetPosition - this.threedObject.transform.position).sqrMagnitude;
        if(moveToDistance < changeAnimationDistanceThreshold)
        {
            this.ChangetToTriggerAnimation(CharacterState.Idle);
        }
    }

    private IEnumerator LoiterRandomRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(loiterSecondSpan);
            Debug.Log(this.currentState);
            if(this.currentState == CharacterState.Idle)
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
        this.ChangetToTriggerAnimation(CharacterState.Walk);
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
