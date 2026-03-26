using UnityEngine;
using System.Collections;
using UnityEngine.Splines;
using System.Collections.Generic;
public class Airstrike : MonoBehaviour
{
    [SerializeField] private GameObject projectiles;
    [SerializeField] private float finalScale = 30;
    [SerializeField] private float lockonForwardOffset = 250;
    [SerializeField] private int strikeCount = 7;
    [SerializeField] private float timeToLockOn = 4;
    [SerializeField] private float timeTillFire = 1;
    [SerializeField] private float fireDuration = 1;
    private RaceController raceController;
    private RacerData leader;
    private List<PlayerHit> playersInHitRange;
    //Kan lägga en animation för när airstriken blir större

    private void Start()
    {
        playersInHitRange = new List<PlayerHit>();
        raceController = FindAnyObjectByType<RaceController>();
        if (raceController == null) return;
        StartCoroutine(SetTarget());
    }
    IEnumerator SetTarget()
    {
        float time = 0;
        while(time < timeToLockOn)
        {
            yield return null;

            //set scale
            time += Time.deltaTime;
            float currentScale = (time / timeToLockOn) * finalScale;
            transform.localScale = new Vector3(currentScale, 1000f, currentScale);

            //set pos
            Vector3 tangent = SplineUtility.EvaluateTangent(
                raceController.trackSpline.Spline,
                raceController.GetSplineProgress(leader.transform.position)
            );
            Quaternion tangentQuat = Quaternion.LookRotation(tangent);
            transform.position = leader.transform.position + (tangentQuat * Vector3.forward * lockonForwardOffset);
        }
        transform.localScale = new Vector3(finalScale, 1000f, finalScale);
        StartCoroutine(AirstrikeFire());
    }

    IEnumerator AirstrikeFire()
    {
        yield return new WaitForSeconds(timeTillFire);
        StartCoroutine(Projectiles());
        for (float f = fireDuration; f >= 0; f -= Time.deltaTime)
        {
            yield return null;
            for (int i = 0; i < playersInHitRange.Count; i++)
            {
                playersInHitRange[i].Hit(true);
            }
        }
        Destroy(gameObject);
    }

    IEnumerator Projectiles()
    {
        for (int i = 0; i < strikeCount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject proj = Instantiate(projectiles, transform.position + new Vector3(Random.Range(-10, 10), 25, Random.Range(-10, 10)), Quaternion.LookRotation(Vector3.down));
            proj.GetComponent<Projectile>().PrepareProjectile(proj, null, 0.3f);
        }
    }

    public void SetTarget(RacerData leader)
    {
        this.leader = leader;
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlayerHit playerHit = collision.transform.root.GetComponentInChildren<PlayerHit>();
        if (playerHit != null && !playersInHitRange.Contains(playerHit))
        {
            playersInHitRange.Add(playerHit);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        PlayerHit playerHit = collision.transform.root.GetComponentInChildren<PlayerHit>();
        if (playerHit != null && playersInHitRange.Contains(playerHit))
        {
            playersInHitRange.Remove(playerHit);
        }
    }
}
