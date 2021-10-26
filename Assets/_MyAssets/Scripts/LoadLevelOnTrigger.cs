using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LoadLevelOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<SteamVR_LoadLevel>().Trigger();
        }
    }
}
