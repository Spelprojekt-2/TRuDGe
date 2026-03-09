using UnityEngine;
using System.Collections.Generic;
public class AutoAimCone : MonoBehaviour
{
    public List<Transform> targetList = new List<Transform>();
    public Transform GetTarget()
    {
        if(targetList.Count < 1)
        {
            return null;
        }
        return targetList[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !SceneController.instance.IsMenu)
        {
            targetList.Add(other.transform.root.GetComponentInChildren<PlayerCamera>().forOthersAimPoint);
            Debug.Log(targetList[0].gameObject.name + " Detected");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !SceneController.instance.IsMenu)
        {
            targetList.Remove(other.transform.root.GetComponentInChildren<PlayerCamera>().forOthersAimPoint);
            Debug.Log("Left Detect");
        }
    }
}
