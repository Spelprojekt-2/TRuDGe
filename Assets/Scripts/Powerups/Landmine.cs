using UnityEngine;
using System.Collections;
public class Landmine : MonoBehaviour
{
    [SerializeField] private float startupTime = 1f;
    [SerializeField] private float upwardForce = 50f;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(startupTime);
        GetComponent<Collider>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        Vector3 upwardForceVector = Vector3.up * upwardForce;
        upwardForceVector = new Vector3(rb.linearVelocity.x, upwardForceVector.y, rb.linearVelocity.z);
        rb.AddForce(upwardForceVector, ForceMode.Impulse);
        Destroy(gameObject);
    }
}
