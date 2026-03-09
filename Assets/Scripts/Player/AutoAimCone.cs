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

    private void Update()
    {
        if (targetList.Count < 1) return;
        foreach (var target in targetList)
        {
            if(target == null)
            {
                targetList.Remove(target);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DestroyableWall") && !SceneController.instance.IsMenu)
        {
            Debug.Log("Wall");
            targetList.Add(other.transform);
        }
        else if (other.CompareTag("Player") && !SceneController.instance.IsMenu)
        {
            targetList.Add(other.transform.root.GetComponentInChildren<PlayerCamera>().forOthersAimPoint);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DestroyableWall") && !SceneController.instance.IsMenu)
        {
            targetList.Remove(other.transform);
        }
        if (other.CompareTag("Player") && !SceneController.instance.IsMenu)
        {
            targetList.Remove(other.transform.root.GetComponentInChildren<PlayerCamera>().forOthersAimPoint);
        }
    }
}
