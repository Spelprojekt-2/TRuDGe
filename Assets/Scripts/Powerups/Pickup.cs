using UnityEngine;
using System.Collections;
public class Pickup : MonoBehaviour
{
    [SerializeField] public PlayerPowerups.PowerUpType powerUpType;
    [SerializeField] private float powerupRespawnTime = 30f;
    [SerializeField] private ProbabilityPickupSO probability;

    private Vector3 startPos;
    private Transform targetPlayer;
    [SerializeField] private float flySpeed = 0.03f;

    private PlayerPowerups player;
    private Collider col;
    private MeshRenderer[] meshes;
    private bool visible = true;
    private bool canRespawn = true;
    private void Awake()
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
            int racePosition= player.GetComponent<RacerData>().racePosition;

            if(powerUpType == PlayerPowerups.PowerUpType.gasolineTank)
            {
                player.GainedPowerUp(powerUpType);
            }
            else
            {
                player.GainedPowerUp(probability.RandomizePowerUp(racePosition));
            }

            if (canRespawn)
            {
                StartCoroutine(RespawnTimer());
            }
            else
            {
                Destroy(gameObject);
            }
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

    public IEnumerator DroppedTanks()
    {
        canRespawn = false;
        col.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            foreach (var mesh in meshes)
            {
                mesh.enabled = visible;
                visible = !visible;
            }
            yield return new WaitForSeconds(0.3f);
        }
        col.enabled = true;
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
