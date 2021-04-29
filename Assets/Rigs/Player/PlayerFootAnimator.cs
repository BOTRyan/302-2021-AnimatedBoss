using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootAnimator : MonoBehaviour
{
    private Vector3 startingPos;
    private Quaternion startingRot;

    public float stepOffset = 0;

    PlayerMovement pawn;

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
    }

    void AnimateWalk()
    {
        if (pawn.isSprinting)
        {
            Vector3 finalPos = startingPos;

            float time = (Time.time + stepOffset) * pawn.stepSpeed;

            float frontToBack = Mathf.Sin(time) * 1.5f;
            frontToBack = Mathf.Clamp(frontToBack, -1.5f, 0.5f);

            if (Input.GetKey(KeyCode.W)) finalPos.z += frontToBack * pawn.walkScale.z;
            else if (Input.GetKey(KeyCode.S)) finalPos.z -= frontToBack * pawn.walkScale.z;

            if (Input.GetKey(KeyCode.A)) finalPos.x -= frontToBack * pawn.walkScale.z * 0.8f;
            else if (Input.GetKey(KeyCode.D)) finalPos.x += frontToBack * pawn.walkScale.z * 0.8f;

            finalPos.y += Mathf.Cos(time) * pawn.walkScale.y;

            bool isOnGround = (finalPos.y < startingPos.y);
            if (isOnGround) finalPos.y = startingPos.y;

            float p = 1 - Mathf.Abs(frontToBack);

            float anklePitch = isOnGround ? 0 : -p * 20;

            finalPos = new Vector3(finalPos.x, finalPos.y - 0.35f, finalPos.z);

            transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.001f);
            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRot * Quaternion.Euler(0, 0, anklePitch), 0.001f);
        }
        else
        {
            Vector3 finalPos = startingPos;

            float time = (Time.time + stepOffset) * pawn.stepSpeed;

            float frontToBack = Mathf.Sin(time);

            if (Input.GetKey(KeyCode.W)) finalPos.z += frontToBack * pawn.walkScale.z;
            else if (Input.GetKey(KeyCode.S)) finalPos.z -= frontToBack * pawn.walkScale.z;

            if (Input.GetKey(KeyCode.D)) finalPos.x += frontToBack * pawn.walkScale.z;
            else if (Input.GetKey(KeyCode.A)) finalPos.x -= frontToBack * pawn.walkScale.z;

            finalPos.y += Mathf.Cos(time) * pawn.walkScale.y;

            bool isOnGround = (finalPos.y < startingPos.y);

            if (isOnGround) finalPos.y = startingPos.y;

            float p = 1 - Mathf.Abs(frontToBack);

            float anklePitch = isOnGround ? 0 : -p * 20;

            finalPos = new Vector3(finalPos.x, finalPos.y - 0.075f, finalPos.z);

            transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRot * Quaternion.Euler(0, 0, anklePitch), 0.01f);
        }
    }

    void AnimateIdle()
    {
        transform.localPosition = AnimMath.Slide(transform.localPosition, startingPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startingRot, 0.01f);

        FindGround();
    }
    void FindGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), Vector3.down * 2);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.point = new Vector3(hit.point.x, hit.point.y + 0.075f, hit.point.z);
            transform.position = AnimMath.Slide(transform.position, hit.point, 0.01f);
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, 0.01f);
        }
    }

}