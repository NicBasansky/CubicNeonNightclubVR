using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowController : MonoBehaviour
{
    public bool showControllers = false;

    void Update()
    {
        foreach(var hand in Player.instance.hands)
        {
            if (showControllers)
            {
                hand.ShowController();
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
            }
            else
            {
                hand.HideController();
            }
        }
    }
}
