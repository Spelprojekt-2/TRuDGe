using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void Run(IEnumerator routine)
    {
        if (instance == null)
        {
            Debug.LogError("CoroutineRunner instance is null, cannot run coroutine.");
            return;
        }
        instance.StartCoroutine(routine);
    }
}
