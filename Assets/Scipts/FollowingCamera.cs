using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public List<Transform> targets;
    // [SerializeField] GameObject thingToFollow;
    public Vector3 offset;

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;
        Vector3 centerPoint = GetCenterPoint();
        transform.position = centerPoint + offset;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}