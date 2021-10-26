using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    [Header("SteamVR Inputs")]
    public SteamVR_Action_Boolean nextSongAction;
    public SteamVR_Action_Boolean prevSongAction;



    void Update()
    {
        if (audioManager == null) return;

        bool rightPress = SteamVR_Input.GetStateDown("RightTrackpadPress", SteamVR_Input_Sources.RightHand);//SteamVR_Actions.default_RightTrackpadPress.state;
        bool leftPress = SteamVR_Input.GetStateDown("LeftTrackpadPress", SteamVR_Input_Sources.RightHand);//SteamVR_Actions.default_LeftTrackpadPress.state;
        if (rightPress)
        {
            audioManager.PlayNextTrack();
        }
        if (leftPress)
        {
            audioManager.PlayPreviousTrack();
        }
    }
}
