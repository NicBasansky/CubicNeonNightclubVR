using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLookAt : MonoBehaviour
{
    Animator animator;
    Transform target = null;
    [Range(0, 1)]
    public float weight = 0f;
    public float lookSpeed = 0.8f;
    [Range(0, 1)]
    public float bodyWeightIK = 0.25f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {

        if (target == null) return;

        animator.SetLookAtPosition(target.position);
        animator.SetLookAtWeight(weight, bodyWeightIK);
    }

    public void Look(PlayerHeadTarget headTarget)
    {
        target = headTarget.transform;
        StartCoroutine(AdjustLookWeight(1f));
    }

    private IEnumerator AdjustLookWeight(float newTargetWeight)
    {
        while(true)
        {
            weight = Mathf.Lerp(weight, newTargetWeight, lookSpeed * Time.deltaTime);
            
            yield return null;
        }
    }

    public void ReleaseLook()
    {
        
        StopAllCoroutines();
        StartCoroutine(AdjustLookWeight(0f));
      
    }
}
