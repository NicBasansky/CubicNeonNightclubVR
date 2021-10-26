using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : MonoBehaviour
{
    enum DanceStyle
    {
        Style1,
        Style2,
        Style3,
        Style4,
        Style5, 
        OneSimpleDance
    };

    Animator animator;
    [SerializeField] DanceStyle danceStyle;
    [Range(0, 1f)]
    [SerializeField] float intimateDanceSpeed = 0.638f;
    [SerializeField] float intimateDanceTransitionSpeed = 0.6f;
    [SerializeField] bool isIntimate = false;
    
    float danceSpeed = 1;
    int danceIndex = 0;
    bool isSlowDancing = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {      
        SetDanceStyle();

        danceIndex = UnityEngine.Random.Range(0, 3);
        animator.SetInteger("DanceIndex", danceIndex);

        animator.SetFloat("cycleOffset", UnityEngine.Random.Range(0, 1.0f));
        
        StartCoroutine(PeriodicallyChangeDanceIndex());
        
        // called in NPC ai controller
        //animator.SetBool("Dancing", true);

        animator.SetFloat("speed", 1.0f);


    }

    void Update()
    {
        if (isIntimate)
        {
            danceSpeed = Mathf.Lerp(danceSpeed, intimateDanceSpeed, intimateDanceTransitionSpeed * Time.deltaTime);
            animator.SetFloat("speed", danceSpeed);
            isSlowDancing = true;

        }
        else if (isSlowDancing && !isIntimate)
        {
            danceSpeed = Mathf.Lerp(danceSpeed, 1f, intimateDanceTransitionSpeed * Time.deltaTime);
            animator.SetFloat("speed", danceSpeed);
            Invoke("StopSlowDancing", 3f);
        }
    }

    public void SetIsIntimate(bool isIntimate)
    {
        this.isIntimate = isIntimate;
    }

    private void StopSlowDancing()
    {
        isSlowDancing = false;
    }

    private IEnumerator PeriodicallyChangeDanceIndex()
    {
        while (true)//animator.GetBool("Dancing"))
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 12f));

            danceIndex = UnityEngine.Random.Range(0, 3);
            animator.SetInteger("DanceIndex", danceIndex);

        }
    }

    private void SetDanceStyle()
    {
        switch (danceStyle)
        {
            case DanceStyle.Style1:
                SetAnimatorBool("Style1");
                break;
            case DanceStyle.Style2:
                SetAnimatorBool("Style2");
                break;
            case DanceStyle.Style3:
                SetAnimatorBool("Style3");
                break;
            case DanceStyle.Style4:
                SetAnimatorBool("Style4");
                break;
            case DanceStyle.Style5:
                SetAnimatorBool("Style5");
                break;
            case DanceStyle.OneSimpleDance:
                SetAnimatorBool("OneSimpleDance");
                break;
            default:
                break;
        }
        
    }

    private void SetAnimatorBool(string styleString)
    {
        animator.SetBool("Style1", false);
        animator.SetBool("Style2", false);
        animator.SetBool("Style3", false);
        animator.SetBool("Style4", false);
        animator.SetBool("Style5", false);
        animator.SetBool(styleString, true);
    }

}
