using UnityEngine;
using UnityEngine.Events;

public class OnDestroyAudio : MonoBehaviour
{
    public UnityEvent OnDestroyAction;

    private void OnDestroy()
    {
        OnDestroyAction.Invoke();
    }
}
