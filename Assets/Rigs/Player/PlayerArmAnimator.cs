using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmAnimator : MonoBehaviour
{
    private Vector3 startingPos;
    private Quaternion startingRot;

    public float stepOffset = 0;
    public float attackTimer = 0;

    PlayerMovement pawn;
    public Transform castPos;
    public Transform hintRot;

    void Start()
    {
        startingPos = transform.localPosition;
        startingRot = transform.localRotation;
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
                AnimateIdle();
                break;
            case PlayerMovement.States.Dead:
                break;
        }

        if (pawn.isAttacking)
        {
            AnimateAttack();
        }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;

    }

    void AnimateWalk()
    {
        if (gameObject.name == "IK Arm R" && attackTimer > 0)
        {

        }
        else
        {
            Vector3 finalPos = startingPos;

            float time = (Time.time + stepOffset) * pawn.stepSpeed;

            float frontToBack = Mathf.Sin(time);

            if (pawn.h != 0)
            {

                if (Input.GetKey(KeyCode.W)) finalPos.z += frontToBack * pawn.walkScale.z * 0.25f;
                else if (Input.GetKey(KeyCode.S)) finalPos.z -= frontToBack * pawn.walkScale.z * 0.25f;

                if (Input.GetKey(KeyCode.D)) finalPos.x += frontToBack * pawn.walkScale.z * 0.25f;
                else if (Input.GetKey(KeyCode.A)) finalPos.x -= frontToBack * pawn.walkScale.z * 0.25f;

                time = (Time.time + stepOffset) * 7;
            }
            else
            {
                if (Input.GetKey(KeyCode.W)) finalPos.z += frontToBack * pawn.walkScale.z * 0.8f;
                else if (Input.GetKey(KeyCode.S)) finalPos.z -= frontToBack * pawn.walkScale.z * 0.8f;

                if (Input.GetKey(KeyCode.D)) finalPos.x += frontToBack * pawn.walkScale.z * 0.8f;
                else if (Input.GetKey(KeyCode.A)) finalPos.x -= frontToBack * pawn.walkScale.z * 0.8f;
            }

            if (pawn.isSprinting && pawn.h == 0) finalPos = new Vector3(finalPos.x, finalPos.y + 0.1f, finalPos.z);
            else finalPos = new Vector3(finalPos.x, finalPos.y + 0.025f, finalPos.z);

            transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
            transform.rotation = hintRot.rotation;
        }
    }

    void AnimateAttack()
    {
        if (gameObject.name == "IK Arm R")
        {
            transform.position = castPos.position;
            transform.rotation = Quaternion.Euler(90, 0, 0);

            pawn.isAttacking = false;
        }
    }

    void AnimateIdle()
    {
        if (gameObject.name == "IK Arm R" && attackTimer > 0)
        {

        }
        else
        {
            transform.localPosition = AnimMath.Slide(transform.localPosition, startingPos, 0.01f);
            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRot, 0.01f);
        }
    }
}