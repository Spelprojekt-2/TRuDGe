using UnityEngine;

public class AnimationTriggerTest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void PlayAnimation()
    {
        animator.SetTrigger("Turbo");
        Debug.Log("playAnim");
    }

    public void Start()
    {
        Debug.LogWarning("remove me");
    }
}
