using UnityEngine;
using UnityEngine.Events;
public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] walls = new GameObject[3];
    [SerializeField] private int[] wallHealth = new int[3];
    [SerializeField] private float velocityChangeWhenHit = 0.6f;

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

    public void OnChildHit(int index)
    {
        wallHealth[index]--;

        if (wallHealth[index] <= 0)
        {
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
    public void Initialize(DestroyableObject parent, int index, float velocityChange)
    {
        mainScript = parent;
        myIndex = index;
        this.velocityChange = velocityChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            Debug.Log("Shield hit: " + other.gameObject.name);
            Destroy(other.gameObject);
            mainScript.OnChildHit(myIndex);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit: " + other.gameObject.name);
            var rb = other.transform.root.gameObject.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity *= velocityChange;
            mainScript.OnChildHit(myIndex);
        }

        if (other.CompareTag("Projectile"))
        {
            mainScript.OnChildHit(myIndex);
        }
    }
}