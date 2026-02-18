using UnityEngine;

public class DeployedWall : MonoBehaviour
{
    [SerializeField] private GameObject[] walls = new GameObject[3];
    private int[] wallHealth = new int[3];

    private void Start()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            wallHealth[i] = 1;
            if (walls[i] != null)
            {
                WallChildListener listener = walls[i].AddComponent<WallChildListener>();
                listener.Initialize(this, i);
            }
        }
    }

    public void OnChildHit(int index)
    {
        wallHealth[index]--;
        Debug.Log($"Wall piece {index} hit! Health left: {wallHealth[index]}");

        if (wallHealth[index] <= 0)
        {
            Debug.Log(walls[index].name + " destroyed");
            walls[index].GetComponent<MeshRenderer>().enabled = false;
            walls[index].GetComponent<Collider>().enabled = false;
        }
    }
}

public class WallChildListener : MonoBehaviour
{
    private DeployedWall mainScript;
    private int myIndex;

    public void Initialize(DeployedWall parent, int index)
    {
        mainScript = parent;
        myIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        mainScript.OnChildHit(myIndex);
    }
    private void OnCollisionEnter(Collision other)
    {
        mainScript.OnChildHit(myIndex);
    }
}