using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationVec;
    public float speed = 1;
    public bool randomizeRotation = false;
    public float minRandRotation = -3f;
    public float maxRandRotation = 3f;
    Vector3 rot;

    void Start()
    {
        if (randomizeRotation)
        {
            rot = new Vector3(Random.Range(minRandRotation, maxRandRotation),
                            Random.Range(minRandRotation, maxRandRotation),
                            Random.Range(minRandRotation, maxRandRotation));
            rotationVec = rot;

        }
    }

    void LateUpdate()
    {
       
            rot = rotationVec * Time.deltaTime * speed;
            transform.Rotate(rot.x, rot.y, rot.z, Space.Self);
       
    }
}
