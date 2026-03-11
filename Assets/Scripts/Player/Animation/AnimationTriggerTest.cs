using UnityEngine;

public class AnimationTriggerTest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void PlayAnimation()
    {
        animator.SetTrigger("Airstrike");
        Debug.Log("playAnim");
    }

    public void Start()
    {
        Debug.LogWarning("remove me");
    }
}
