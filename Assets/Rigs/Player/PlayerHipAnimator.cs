using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHipAnimator : MonoBehaviour
{

    Quaternion startRot;
    Vector3 startPos;
    PlayerMovement pawn;
    bool jumpOnce = true;

    void Start()
    {
        startRot = transform.localRotation;
        startPos = transform.localPosition;
        pawn = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        switch (pawn.state)
        {
            case PlayerMovement.States.Idle:
                AnimateIdle();
                break;
            case PlayerMovement.States.Walk:
                AnimateWalk();
                break;
            case PlayerMovement.States.Jumping:
                AnimateJump();
                break;
            case PlayerMovement.States.Dead:
                break;
        }
    }

    void AnimateIdle()
    {
        jumpOnce = true;
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.01f);
    }

    void AnimateJump()
    {
        Vector3 finalPos = startPos;

        if (jumpOnce)
        {
            finalPos.y -= 7f;
            jumpOnce = false;
        }

        if (finalPos.y < 0)
        {
            finalPos.y += Time.deltaTime * 0.5f;
        }

        transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.01f);
    }

    void AnimateWalk()
    {
        float time = Time.time * pawn.stepSpeed;
        float roll = 0;
        float pitch = 0;
        float yaw = 0;

        if (pawn.isSprinting)
        {
            pitch = Mathf.Sin(time) * 7;
            yaw = Mathf.Sin(time) * 20;
            time = Time.time * pawn.stepSpeed;
            roll = Mathf.Sin(time) * 5;
        }
        else
        {
            pitch = 0;
            yaw = Mathf.Sin(time) * 4;
            roll = Mathf.Sin(time) * 2;
        }


        transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(pitch, yaw, roll), 0.05f);
    }
}
