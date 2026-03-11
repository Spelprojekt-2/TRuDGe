using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationTriggerTest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string[] anims = new string[]
    {
        "Turbo",
        "ScatterShot",
        "Airstrike",
        "Mine"
    };
    private int indexer = 0;
    
    public void PlayAnimation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger(anims[indexer++]);
            Debug.Log("playAnim");
            indexer %= anims.Length;
        }
    }

    public void Start()
    {
        Debug.LogWarning("remove me");
    }
}
