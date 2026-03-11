using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    [SerializeField] private int wallLayer;
    [SerializeField] private UnityEvent hitWall;
    [SerializeField] private int playerLayer;
    [SerializeField] private UnityEvent hitPlayer;
    
    private void OnCollisionEnter(Collision collision)
    {
        // layermasks aren't constant and can't be used for a switch
        if (collision.gameObject.layer == wallLayer)
        {
            hitWall.Invoke();
        }
        if (collision.gameObject.layer == playerLayer)
        {
            hitPlayer.Invoke();
        }
    }
}