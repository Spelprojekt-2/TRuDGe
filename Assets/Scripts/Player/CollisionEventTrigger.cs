using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private UnityEvent hitWall;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private UnityEvent hitPlayer;
    
    private void OnCollisionEnter(Collision collision)
    {
        // layermasks aren't constant and can't be used for a switch
        if (collision.gameObject.layer == wallLayer.value) hitWall.Invoke();
        if (collision.gameObject.layer == playerLayer.value) hitPlayer.Invoke();
    }
}