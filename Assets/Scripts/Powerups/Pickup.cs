using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Pickup : MonoBehaviour
{
    [SerializeField] public PlayerPowerups.PowerUpType powerUpType;
    [SerializeField] private float powerupRespawnTime = 30f;
    [SerializeField] private ProbabilityPickupSO probability;
    [SerializeField] private ProbabilityPickupSO fullProbability;

    public static List<Pickup> AllPickups = new List<Pickup>();

    private Vector3 startPos;
    private Transform targetPlayer;
    [SerializeField] private float flySpeed = 0.03f;

    private PlayerPowerups player;
    private Collider col;
    private MeshRenderer[] meshes;
    private bool visible = true;
    private bool canRespawn = true;
    private void Start()
    {
        startPos = transform.position;
        col = GetComponent<Collider>();
        meshes = GetComponentsInChildren<MeshRenderer>();

        if (powerUpType != PlayerPowerups.PowerUpType.gasolineTank && RacingInformation.instance.isTimeTrial)
        {
            Destroy(gameObject);
        }
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
                if(player.gasTankAmount == 10)
                {
                    player.GainedPowerUp(fullProbability.RandomizePowerUp(racePosition));
                }
                else
                {
                    player.GainedPowerUp(probability.RandomizePowerUp(racePosition));
                }
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
        if (col == null) yield break;
        col.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            foreach (var mesh in meshes)
            {
                mesh.enabled = visible;
            }
            yield return new WaitForSeconds(0.3f);
            visible = !visible;
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

    private void OnEnable()
    {
        AllPickups.Add(this);
    }

    private void OnDisable()
    {
        AllPickups.Remove(this);
    }
}
