using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public List<Transform> targets;
    [SerializeField] GameObject bigMap;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    void Start()
    {
        var map = bigMap.GetComponent<Renderer>().bounds.size;;
        float vertExtent = (float)Camera.main.GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * (float)Screen.width / (float)Screen.height;
       // Debug.Log("Hello");
        // Calculations assume map is position at the origin
        minX = horzExtent - map.x / 2.0f;
        maxX = map.x / 2.0f - horzExtent;
        minY = vertExtent - map.y / 2.0f;
        maxY = map.y / 2.0f - vertExtent;
    }
    void LateUpdate()
    {
        if (targets.Count == 0)
            return;
        Vector3 centerPoint = GetCenterPoint();
        transform.position = centerPoint + new Vector3(0, 0, -5);
        var v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = v3;
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

