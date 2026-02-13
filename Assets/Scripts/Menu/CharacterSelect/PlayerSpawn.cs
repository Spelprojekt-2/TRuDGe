using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject Player;
    public void Spawn()
    {
        Instantiate(Player);
    }
}
