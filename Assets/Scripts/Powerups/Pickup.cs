using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerPowerups.PowerUpType powerUpType;
    private PlayerPowerups player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            player = other.gameObject.GetComponentInParent<PlayerPowerups>();
            player.GainedPowerUp(powerUpType);
            Destroy(gameObject);
        }
    }
}
