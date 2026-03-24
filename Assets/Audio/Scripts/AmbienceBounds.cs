using UnityEngine;
using FMODUnity;

public class AmbienceBounds : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private string parameterName = "InBounds";
    [SerializeField] private LayerMask playerLayer;

    private int playersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject.layer, playerLayer)) return;

        playersInside++;
        Debug.Log("PLAYERS IN BOUNDS: " +  playersInside);
        UpdateParameter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject.layer, playerLayer)) return;

        playersInside = Mathf.Max(0, playersInside - 1);
        Debug.Log("PLAYERS IN BOUNDS: " + playersInside);
        UpdateParameter();
    }

    private void UpdateParameter()
    {
        if (emitter == null) return;

        float value = playersInside > 0 ? 1f : 0f;
        emitter.SetParameter(parameterName, value);
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}