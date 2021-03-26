using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipAnimator : MonoBehaviour
{

    Quaternion startRot;
    GoonController goon;

    void Start()
    {
        startRot = transform.localRotation;
        goon = GetComponentInParent<GoonController>();
    }

    void Update()
    {
        switch (goon.state)
        {
            case GoonController.States.Idle:
                AnimateIdle();
                break;
            case GoonController.States.Walk:
                AnimateWalk();
                break;
        }
    }

    void AnimateIdle()
    {
        transform.localRotation = startRot;
    }

    void AnimateWalk()
    {
        float time = Time.time * goon.stepSpeed;
        float roll = Mathf.Sin(time);

        transform.localRotation = Quaternion.Euler(0, 0, roll);
    }
}
