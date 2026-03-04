using UnityEngine;
using System.Collections;
public class Airstrike : MonoBehaviour
{
    [SerializeField] private GameObject projectiles;
    [SerializeField] private float scaleTimer = 3;
    [SerializeField] private float finalScale = 30;
    [SerializeField] private int strikeCount = 7;

    private void Start()
    {
        StartCoroutine(AirstrikeFire());
    }
    IEnumerator AistrikeScale()
    {
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1f);
            transform.localScale = new Vector3(finalScale / i, 0.01f, finalScale / i);
        }
        StartCoroutine(AirstrikeFire());
    }

    IEnumerator AirstrikeFire()
    {
        for (int i = 0; i < strikeCount; i++)
        {
            GameObject proj = Instantiate(projectiles, transform.position + new Vector3(Random.Range(-10, 10), 25, Random.Range(-10, 10)), Quaternion.LookRotation(Vector3.down));
            proj.GetComponent<Projectile>().PrepareProjectile(gameObject, transform, 0);
            yield return new WaitForSeconds(1f);
        }
    }
}
