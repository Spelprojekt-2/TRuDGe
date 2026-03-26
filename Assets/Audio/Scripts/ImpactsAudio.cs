using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptables/Audio/Impacts")]
public class ImpactsAudio : ScriptableObject
{
    #region EventReferences
    [Header("Impacts")]
    [SerializeField] private EventReference TankImpactRef;
    [SerializeField] private EventReference WoodImpactRef;
    [SerializeField] private EventReference WallDestroyRef;
    [SerializeField] private EventReference SnowImpactRef;
    [SerializeField] private EventReference ProjectileImpactRef;
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

    public void PlayProjectileImpact()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex <= 3)  // AVOID IMPACT IN MAINMENU
        {
            Debug.Log("Avoided impact audio!");
            return;
        }

        if (ProjectileImpactRef.IsNull)
        {
            Debug.LogWarning("ImpactsAudio: ProjectileImpactRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ProjectileImpactRef);
    }
    #endregion
}
