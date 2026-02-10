using UnityEngine;
using System.Collections;
public class Pickup : MonoBehaviour
{
    [SerializeField] public PlayerPowerups.PowerUpType powerUpType;
    [SerializeField] private float powerupRespawnTime = 30f;

    private Vector3 startPos;
    private Transform targetPlayer;
    [SerializeField] private float flySpeed = 0.03f;

    private PlayerPowerups player;
    private Collider col;
    private MeshRenderer[] meshes;
    private void Start()
    {
        startPos = transform.position;
        col = GetComponent<Collider>();
        meshes = GetComponentsInChildren<MeshRenderer>();
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
                PlayerPowerups.PowerUpType randomPowerUp = (PlayerPowerups.PowerUpType)Random.Range(1, System.Enum.GetValues(typeof(PlayerPowerups.PowerUpType)).Length);
                player.GainedPowerUp(randomPowerUp);
                Debug.Log(randomPowerUp);
            }
            StartCoroutine(RespawnTimer());
        }
    }

    private IEnumerator RespawnTimer()
    {
        col.enabled = false;
        foreach (var mesh in meshes)
        {
            mesh.enabled = false;
        }
        yield return new WaitForSeconds(powerupRespawnTime);

        targetPlayer = null;
        transform.position = startPos;
        col.enabled = true;
        foreach (var mesh in meshes)
        {
            mesh.enabled = true;
        }
    }
    public void SetMagnetTarget(Transform player)
    {
        if (targetPlayer == null)
        {
            targetPlayer = player;
        }
    }

    private void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * flySpeed;
            transform.Rotate(Vector3.up * 300f * Time.deltaTime);
        }
    }
}
