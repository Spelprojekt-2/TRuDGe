using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    private enum TriggerEventType
    {
        Enter,
        Exit,
        Stay
    }
    [SerializeField] private TriggerEventType triggerEventType;
    [SerializeField] private string targetTag = "Player";
    public UnityEvent<GameObject> onTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (triggerEventType == TriggerEventType.Enter && other.CompareTag(targetTag))
        {
            onTrigger.Invoke(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerEventType == TriggerEventType.Exit && other.CompareTag(targetTag))
        {
            onTrigger.Invoke(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (triggerEventType == TriggerEventType.Stay && other.CompareTag(targetTag))
        {
            onTrigger.Invoke(other.gameObject);
        }
    }
}
