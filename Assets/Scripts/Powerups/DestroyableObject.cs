using UnityEngine;
using System.Collections;
public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] walls = new GameObject[3];
    [SerializeField] private int[] wallHealth = new int[3];
    [SerializeField] private float velocityChangeWhenHit = 0.6f;

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
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            var rb = other.transform.root.gameObject.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity *= velocityChange;
            mainScript.OnChildHit(myIndex);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            mainScript.OnChildHit(myIndex);
        }
    }
}