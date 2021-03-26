using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootAnimator : MonoBehaviour
{
    /// <summary>
    /// the local-space starting position of this object.
    /// </summary>
    private Vector3 startPos;
    /// <summary>
    /// the local-space starting rotation of this object;
    /// </summary>
    private Quaternion startRot;

    /// <summary>
    /// An offset value to use for timing of the Sin wave
    /// that controls the walk animation. Measured in radians.
    /// 
    /// A value of Mathf.PI would be half-a-period.
    /// </summary>
    public float stepOffset = 0;

    GoonController goon;

    private Vector3 targetPos;
    private Quaternion targetRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        if (stepOffset != 0) stepOffset = Mathf.PI;
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

        //transform.position = AnimMath.Slide(transform.position, targetPos, .01f);
        //transform.rotation = AnimMath.Slide(transform.rotation, targetRot, .01f);
    }

    void AnimateWalk()
    {
        Vector3 finalPos = startPos;
        float time = (Time.time + stepOffset) * goon.stepSpeed;

        // lateral movement (z + x)
        float frontToBack = Mathf.Sin(time);
        finalPos += goon.moveDir * frontToBack * goon.walkScale.z;

        // vertical movement
        finalPos.y += Mathf.Cos(time) * goon.walkScale.y;

        bool isOnGround = (finalPos.y < startPos.y);

        if (isOnGround) finalPos.y = startPos.y;

        // convert from z (-1 to 1) to p (0 to 1 to 0)
        float p = 1 - Mathf.Abs(frontToBack);

        float anklePitch = isOnGround ? 0 : -p * 20;

        transform.localPosition = finalPos;
        transform.localRotation = startRot * Quaternion.Euler(0, 0, anklePitch);

        //targetRot = transform.parent.rotation * startRot * Quaternion.Euler(0, 0, anklePitch);
        //targetPos = transform.TransformPoint(finalPos);
    }

    void AnimateIdle()
    {
        transform.localPosition = startPos;
        transform.localRotation = startRot;

        //targetPos = transform.TransformPoint(startPos);
        //targetRot = transform.parent.rotation * startRot;
        FindGround();
    }

    void FindGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, .5f, 0), Vector3.down * 2);

        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.localPosition = hit.point;
            transform.localRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            //targetPos = hit.point;
            //targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {

        }


    }
}
