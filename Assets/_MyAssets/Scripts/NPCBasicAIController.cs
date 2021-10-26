using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBasicAIController : MonoBehaviour
{
    

    [SerializeField] AnimState animState;
    [SerializeField] float minSecondsToDrink = 2f;
    [SerializeField] float maxSecondsToDrink = 10f;
    [SerializeField] Animator animator;

    [Header("Animation Index Config")]
    [SerializeField] int numSitTalk;
    [SerializeField] int numTalking;

    int animIndex = 0;
    [Range(0, 1)]
    float animationOffset = 0;

    private void Start() 
    {
        animator.SetFloat("cycleOffset", UnityEngine.Random.Range(0, 1.0f));
        switch (animState)
        {
            case AnimState.Sitting:          
                animator.SetBool("isSitting", true);
                break;

            case AnimState.SittingCross:
                animator.SetBool("isSittingCrossed", true);
                break;

            case AnimState.SitTalk:
                if (numSitTalk > 1)
                {
                    StartCoroutine(PeriodicallyChangeAnimIndex(numSitTalk));
                }
                animator.SetBool("isSittingTalking", true);
                break;

            case AnimState.Talking:
                if (numTalking > 1)
                {
                    StartCoroutine(PeriodicallyChangeAnimIndex(numTalking));
                }
                animator.SetBool("isTalking", true);
                break;

            case AnimState.HavingMeeting:
                animator.SetBool("HavingMeeting", true);
                break;

            case AnimState.Dancing:
                animator.SetBool("Dancing", true);
                break;

            case AnimState.StandDrinking:
                StartCoroutine(RandomlyDrink());
                break;
        }
    }

    public AnimState GetAnimState()
    {
        return animState;
    }

    IEnumerator PeriodicallyChangeAnimIndex(int numAnimations)
    {
        animIndex = UnityEngine.Random.Range(0, numAnimations);
        animator.SetInteger("animIndex", animIndex);
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 12.0f));
            animIndex = UnityEngine.Random.Range(0, numAnimations + 1);
            animator.SetInteger("animIndex", animIndex);

            //animator.SetFloat("cycleOffset", UnityEngine.Random.Range(0, 1.0f));
        }

    }

    IEnumerator RandomlyDrink()
    {
        while (animState == AnimState.StandDrinking)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSecondsToDrink, maxSecondsToDrink));
            animator.SetTrigger("TakeDrink");
        }
        
    }

}

    
