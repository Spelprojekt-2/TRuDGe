using System;
using UnityEngine;

public class RoadsideCamera : MonoBehaviour
{
    public Action<Transform, Transform> ObservedObjectChanged;

    public void OnTriggerEnter(Collider other)
    {
        ObservedObjectChanged.Invoke(transform, other.transform);
    }

    public void OnTriggerExit(Collider other)
    {
        ObservedObjectChanged.Invoke(transform, null);
    }
}
