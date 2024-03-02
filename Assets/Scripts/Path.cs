using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Path : MonoBehaviour
{
    readonly List<Transform> points = new();
    int current = 0;

    void Awake()
    {
        Assert.IsTrue(transform.childCount > 0, "No children to give!");
        foreach(Transform point in transform)
        {
            points.Add(point);
        }
    }

    public Vector3 Next()
    {
        current = (current + 1) % points.Count;
        return points.ElementAt(current).position;
    }
}
