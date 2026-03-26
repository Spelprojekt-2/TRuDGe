using UnityEngine;
using System.Collections;
public class Landmine : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private float startupTime = 1f;
    [SerializeField] private float upwardForce = 50f;

    [SerializeField] InteractablesAudio interactablesAudio;

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
        if (other != null)
        {
            Debug.Log(other.gameObject.name);
            GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(particle, 2f);
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb == null) return;
            Vector3 upwardForceVector = Vector3.up * upwardForce;
            upwardForceVector = new Vector3(rb.linearVelocity.x, upwardForceVector.y, rb.linearVelocity.z);
            rb.AddForce(upwardForceVector, ForceMode.VelocityChange);
            if (interactablesAudio != null) interactablesAudio.LandmineTriggerAudio(gameObject); // Play landmine audio
            Destroy(gameObject);
        }
    }
}
