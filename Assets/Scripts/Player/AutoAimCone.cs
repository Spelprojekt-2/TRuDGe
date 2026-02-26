using UnityEngine;
using System.Collections.Generic;
public class AutoAimCone : MonoBehaviour
{
    public List<Transform> targetList = new List<Transform>();
    public Transform GetTarget()
    {
        if(targetList == null)
        {
            return null;
        }
        return targetList[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetList.Add(other.transform.root.GetComponentInChildren<PlayerMovement>().transform);
            Debug.Log(targetList[0].gameObject.name + " Detected");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetList.Remove(other.transform.root.GetComponentInChildren<PlayerMovement>().transform);
            Debug.Log("Left Detect");
        }
    }
}
