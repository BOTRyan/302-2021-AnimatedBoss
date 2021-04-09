using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFoot : MonoBehaviour
{

    public Transform stepPosition;

    public AnimationCurve vertStepMovement;

    private Vector3 prevPlantedPosition;
    private Quaternion prevPlantedRotation = Quaternion.identity;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation = Quaternion.identity;

    private float timeLength = .25f;
    private float timeCurrent = 0;

    void Update()
    {
        if (CheckIfCanStep())
        {
            DoRaycast();
        }

        if(timeCurrent < timeLength)
        {
            timeCurrent += Time.deltaTime;

            float p = timeCurrent / timeLength;

            Vector3 finalPos = AnimMath.Lerp(prevPlantedPosition, plantedPosition, p);

            finalPos.y += vertStepMovement.Evaluate(p);

            transform.position = finalPos;

            transform.rotation = AnimMath.Lerp(prevPlantedRotation, plantedRotation, p);
        }
        else
        {
            transform.position = plantedPosition;
            transform.rotation = plantedRotation;
        }
    }

    bool CheckIfCanStep()
    {
        Vector3 vBetween = transform.position - stepPosition.position;
        float threshold = 5;

        return (vBetween.sqrMagnitude > threshold * threshold);
    }

    void DoRaycast()
    {
        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            // setup beginning of animation:
            prevPlantedPosition = transform.position;
            prevPlantedRotation = transform.rotation;

            plantedPosition = hit.point;
            plantedRotation = Quaternion.FromToRotation(transform.up, hit.normal);

            // Begin animation
            timeCurrent = 0;
        }
    }
}
