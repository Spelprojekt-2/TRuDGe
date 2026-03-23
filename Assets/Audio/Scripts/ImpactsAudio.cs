using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Impacts")]
public class ImpactsAudio : ScriptableObject
{
    #region EventReferences
    [Header("Impacts")]
    [SerializeField] private EventReference TankImpactRef;
    [SerializeField] private EventReference WoodImpactRef;
    [SerializeField] private EventReference WallDestroyRef;
    [SerializeField] private EventReference SnowImpactRef;
    #endregion

    #region Functions
    public void PlayTankImpact()
    {
        if (TankImpactRef.IsNull)
        {
            Debug.LogWarning("ImpactsAudio: TankImpactRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(TankImpactRef);
    }

    public void PlayWoodImpact()
    {
        if (WoodImpactRef.IsNull)
        {
            Debug.LogWarning("ImpactsAudio: WoodImpactRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(WoodImpactRef);
    }

    public void PlayDestroyWall(Transform pos)
    {
        if (WallDestroyRef.IsNull)
        {
            Debug.LogError("ImpactsAudio: WallDestroyRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(WallDestroyRef, pos.position);
    }

    public void PlaySnowImpact()
    {
        if (SnowImpactRef.IsNull)
        {
            Debug.LogWarning("ImpactsAudio: SnowImpactRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(SnowImpactRef);
    }
    #endregion
}
