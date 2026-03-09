using UnityEngine;

public class SlowingObject : MonoBehaviour
{
    [SerializeField] private float velocityChange = 0.7f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            var rb = other.transform.root.gameObject.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity *= velocityChange;
        }
    }
}
