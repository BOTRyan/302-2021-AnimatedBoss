using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadAnimator : MonoBehaviour
{

    private Vector3 startPos;
    private Quaternion startRot;

    BossMovement pawn;

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponentInParent<BossMovement>();
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (pawn.state)
        {
            case BossMovement.States.Idle:
                AnimateIdle();
                break;
            case BossMovement.States.Walk:
                AnimateWalk();
                break;
            case BossMovement.States.Attacking:
                AnimateAttack();
                break;
            case BossMovement.States.Dead:
                AnimateDead();
                break;
        }
    }

    void AnimateIdle()
    {
        Vector3 finalPos = startPos;
        float time = Time.time * 1.5f;

        finalPos.y += Mathf.Sin(time) * 0.1f;

        if (pawn.player)
        {
            if (Vector3.Distance(transform.position, pawn.player.transform.position) < 10)
            {
                transform.position = AnimMath.Slide(transform.position, pawn.player.transform.position, 0.05f);
                transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.05f);
            }
            else
            {
                transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.05f);
                transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.05f);
            }
        }
    }

    void AnimateAttack()
    {
        Vector3 finalPos = transform.localPosition;
        float time = Time.time;

        finalPos.y += Mathf.Sin(time * 50) * 2;

        transform.localPosition = AnimMath.Slide(transform.localPosition, finalPos, 0.01f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.01f);
    }

    void AnimateWalk()
    {
        transform.localPosition = AnimMath.Slide(transform.localPosition, startPos, 0.05f);
        transform.localRotation = AnimMath.Slide(transform.localRotation, startRot, 0.05f);
    }

    void AnimateDead()
    {

    }
}
