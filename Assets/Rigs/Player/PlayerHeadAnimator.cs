using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadAnimator : MonoBehaviour
{

    Quaternion startRot;
    Vector3 startPos;
    PlayerMovement pawn;
    public Transform cam;

    //private float checkTimer = 10;
    //private float playTimer = 0;
    //private float alpha = 0;
    //private bool doOnce = true;
    //private bool playAnim = false;

    void Start()
    {
        startRot = transform.localRotation;
        startPos = transform.localPosition;
        pawn = GetComponentInParent<PlayerMovement>();
        transform.localRotation = Quaternion.identity;
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
                AnimateIdle();
                break;
            case PlayerMovement.States.Dead:
                break;
        }
    }

    void AnimateIdle()
    {
        //if (doOnce)
        //{
        //    checkTimer = 10;
        //    doOnce = false;
        //}

        float yaw = 0;

        /*if (checkTimer > 0)
        {
            checkTimer -= Time.deltaTime;
        }
        else if (checkTimer <= 0 && !playAnim)
        {
            float rand = Random.Range(1, 6);
            if (rand == 1 || rand == 3 || rand == 5)
            {
                playAnim = true;
            }
            else
            {
                checkTimer = 10;
            }
        }

        if (playAnim)
        {
            Vector3 finalPos = transform.localPosition;

            alpha += Time.deltaTime * 1.25f;

            yaw = Mathf.Sin(alpha) * 45;

            if (playTimer <= 0) playTimer = 4;
        }
        else
        {
            yaw = 0;
        }

        if (playTimer > 0) playTimer -= Time.deltaTime;

        if (playTimer <= 0)
        {
            playTimer = 0;
            alpha = 0;
            playAnim = false;
        }*/

        transform.localPosition = AnimMath.Slide(transform.localPosition, startPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(0, yaw, 0), 0.05f);
    }

    void AnimateWalk()
    {
        //doOnce = true;
        transform.localPosition = AnimMath.Slide(transform.localPosition, startPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.05f);
    }
}
