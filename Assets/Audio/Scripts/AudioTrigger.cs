using UnityEngine;
using UnityEngine.Events;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    [SerializeField] private string triggerTag = "";

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        OnEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        OnExit.Invoke();
    }
}
