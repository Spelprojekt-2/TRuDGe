using UnityEngine;
using UnityEngine.Events;
public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] walls = new GameObject[3];
    [SerializeField] private int[] wallHealth = new int[3];
    [SerializeField] private float velocityChangeWhenHit = 0.6f;
    [SerializeField] private GameObject destructParticle;

    [Header("Audio")]
    public UnityEvent OnAudio;

    private void Start()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            wallHealth[i] = 1;
            if (walls[i] != null)
            {
                WallChildListener listener = walls[i].AddComponent<WallChildListener>();
                listener.Initialize(this, i, velocityChangeWhenHit);
            }
        }
    }

    public void OnChildHit(int index, Vector3 hitDirection)
    {
        wallHealth[index]--;

        if (wallHealth[index] <= 0)
        {
            GameObject particle = Instantiate(destructParticle, walls[index].transform.position, Quaternion.LookRotation(hitDirection));
            Destroy(particle, 2);
            Destroy(walls[index]);

            // Trigger audio
            OnAudio.Invoke();
        }
    }
}

public class WallChildListener : MonoBehaviour
{
    private DestroyableObject mainScript;
    private int myIndex;
    private float velocityChange;
    private bool wasHit = false;
    public void Initialize(DestroyableObject parent, int index, float velocityChange)
    {
        mainScript = parent;
        myIndex = index;
        this.velocityChange = velocityChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Nytt!!!
        Vector3 hitDirection = (transform.position - other.transform.position).normalized;
        if (other.CompareTag("Shield") && !wasHit)
        {
            wasHit = true;
            Debug.Log("Hit shield");
            PlayerHit hit = other.transform.root.GetComponentInChildren<PlayerHit>();
            hit.HitShield();
            Destroy(other.gameObject);

            mainScript.OnChildHit(myIndex, hitDirection);
        }
        
        if (other.CompareTag("Player") && !wasHit)
        {
            wasHit = true;
            Debug.Log("Hit player" + other.name);
            var rb = other.transform.root.gameObject.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity *= velocityChange;
            mainScript.OnChildHit(myIndex, hitDirection);
        }

        if (other.CompareTag("Projectile") && !wasHit)
        {
            wasHit = true;
            mainScript.OnChildHit(myIndex, -hitDirection);
        }
    }
}