using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShouldersAnimator : MonoBehaviour
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
        Vector3 finalPos = startPos;
        float time = Time.time;

        finalPos.y += Mathf.Sin(time) * 0.015f;

        transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.01f);
    }

    void AnimateJump()
    {
        Vector3 finalPos = startPos;
        Quaternion finalRot = startRot;

        if (jumpOnce)
        {
            finalPos.y -= 4;
            finalPos.z += 1;
            finalRot.x += 2;
            jumpOnce = false;
        }

        if (finalPos.y < 0) finalPos.y += Time.deltaTime * 0.5f;
        if (finalPos.z > 0) finalPos.z -= Time.deltaTime * 0.5f;
        if (finalRot.z > 0) finalRot.x -= Time.deltaTime * 0.5f;


        transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, finalRot, 0.01f);
    }

    void AnimateWalk()
    {
        float time = Time.time * pawn.stepSpeed;
        float roll = 0;
        //float pitch = 0;
        float yaw = 0;

        if (pawn.isSprinting)
        {
            roll = Mathf.Sin(time) * 10;
            yaw = Mathf.Sin(time) * 20;
        }
        else
        {
            roll = Mathf.Sin(time) * 2;
            yaw = Mathf.Sin(time) * 2;
        }



        transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(0, yaw, roll), 0.05f);
    }
}
