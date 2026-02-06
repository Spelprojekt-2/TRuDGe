using UnityEngine;
using System.Collections;
public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerPowerups.PowerUpType powerUpType;
    [SerializeField] private float powerupRespawnTime = 30f;

    private PlayerPowerups player;
    private Collider col;
    private MeshRenderer mesh;
    private void Start()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerPowerups>();
            if(powerUpType == PlayerPowerups.PowerUpType.gasolineTank)
            {
                player.GainedPowerUp(powerUpType);
            }
            else
            {
                PlayerPowerups.PowerUpType randomPowerUp = (PlayerPowerups.PowerUpType)Random.Range(0, System.Enum.GetValues(typeof(PlayerPowerups.PowerUpType)).Length);
                player.GainedPowerUp(randomPowerUp);
            }
            StartCoroutine(RespawnTimer());
        }
    }

    private IEnumerator RespawnTimer()
    {
        col.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(powerupRespawnTime);
        col.enabled = true;
        mesh.enabled = true;
    }
}
