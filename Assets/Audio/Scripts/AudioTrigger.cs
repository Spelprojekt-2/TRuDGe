using UnityEngine;
using UnityEngine.Events;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    [SerializeField] private LayerMask playerLayer;

    void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject.layer, playerLayer)) return;
        OnEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject.layer, playerLayer)) return;
        OnExit.Invoke();
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
