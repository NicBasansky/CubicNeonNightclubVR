using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    GameObject player;
    PlayerHeadTarget headTarget;
    [Tooltip("How often in seconds to check if player is nearby")]
    float checkInterval = 3f;
    [Tooltip("If the player is nearby, what is the chance of deciding to look at the player")]
    [Range(0, 1f)]
    // [SerializeField]
    float chanceToLookAtPlayerIfClose = .65f;
    [SerializeField] float minDist = 2.69f;
    [SerializeField] float intimateDistance = 1.23f;
    float visAngle = 70.0f;
    [SerializeField] float minRandLookTime = 3f;
    [SerializeField] float maxRandLookTime = 6f;
    [SerializeField] float intimateCountThreshold = 4f;
    [SerializeField] float intimateBufferThreshold = 4.5f;
    [SerializeField] IKLookAt IKLookAt;

    Dancer dancer = null;
    bool isDancing = false;

    bool isLooking = false;
    bool isIntimate = false;
    bool inIntimateRange = false;
    float intimateCount = 0;
    float intimateBufferCount = 0;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        headTarget = player.GetComponentInChildren<PlayerHeadTarget>();

        dancer = GetComponent<Dancer>();
        
    }

    void Start()
    {
        AnimState animState = GetComponent<NPCBasicAIController>().GetAnimState();
        isDancing = (animState == AnimState.Dancing);
        
        if (IKLookAt)
        {
            StartCoroutine(CheckIfPlayerIsClose());
            StartCoroutine(CheckIfPlayerIsIntimateClose());

        }
    }

    private IEnumerator CheckIfPlayerIsIntimateClose()
    {
        if (!isDancing)
        {
            yield return null;
        }
        while (dancer != null) // if there is a dancer component then animation will slow. Remove said comp if not dancing
        {
            
            //Vector3 playerVec = player.transform.position - transform.position;
            if (Vector3.Distance(player.transform.position, transform.position) <= intimateDistance)
                        //&& Vector3.Angle(Vector3.forward, playerVec) < visAngle)
            {
                inIntimateRange = true;

                // reset the buffer timer
                intimateBufferCount = 0;

                // make eye contact
                IKLookAt.Look(headTarget);

                intimateCount += Time.deltaTime;

                if (intimateCount >= intimateCountThreshold)
                {
                    isIntimate = true;
                    
                    // set slower animation speed
                    dancer.SetIsIntimate(isIntimate); // todo
                }                             

                yield return null;
            }
            else if (inIntimateRange && Vector3.Distance(player.transform.position, transform.position) >= intimateDistance)
            {
                intimateCount = 0;
                intimateBufferCount += Time.deltaTime;
                if (intimateBufferCount >= intimateBufferThreshold)
                {
                    isIntimate = false;
                    // resume dance animation speed
                    dancer.SetIsIntimate(isIntimate);
                    inIntimateRange = false;
                    IKLookAt.ReleaseLook();

                }
            }
            yield return null;
        }
    }

    // This is still useful as this provides a chance for the NPC to look at the player if the
    // player in the FOV and further away. The intimacy coroutine handles if you are super close.\
    // 100 percent of close encounters will make them look, while only randomly will look at you here
    // Also it is more useful for non dancer npcs 
    private IEnumerator CheckIfPlayerIsClose()
    {
        while(true)
        {
            while(!isLooking)
            {
                
                Vector3 playerVec = player.transform.position - transform.position;

                // player is in field of view
                if (Vector3.Distance(player.transform.position, transform.position) < minDist 
                        && Vector3.Angle(Vector3.forward, playerVec) < visAngle)
                {
                    if (Random.Range(0, 1f) <= chanceToLookAtPlayerIfClose)
                    {

                        IKLookAt.Look(headTarget);
                        isLooking = true;
                                        
                    }
                    else
                    {
                        yield return new WaitForSeconds(checkInterval);
                    }
                }
                yield return null;
            }

            
            if (!isIntimate && isLooking)
            {
                yield return new WaitForSeconds(Random.Range(minRandLookTime, maxRandLookTime));

                IKLookAt.ReleaseLook();
                isLooking = false;

            }
            yield return null;
        }

    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, intimateDistance);

    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, minDist);
    // }
}
