using System.Numerics;
using System.Runtime.Serialization;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class NapoleonRespect : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject Respect;
    [SerializeField] private Image crownfill;
    [SerializeField] private float topSpeed = 100f;
    [SerializeField] private ParticleSystem flames;
    [SerializeField] private bool respectEnabled;      
    private float speedThreshold = 30f;  
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!respectEnabled && !SceneController.instance.IsMenu)
        {
            RacerData data = transform.root.GetComponentInChildren<RacerData>();
            if (data.racername == "King Napoleon III")
            {
                Respect.SetActive(true);
                respectEnabled = true;
            }
        }
        if (respectEnabled && SceneController.instance.IsMenu)
        {
            Respect.SetActive(false);
            respectEnabled = false;
        }
    }

    void Update()
    {
        if (respectEnabled)
        {
            float speed = rb.linearVelocity.magnitude;
            float normalizedSpeed = Mathf.Clamp01(speed/topSpeed);
            float inverted = 1f - normalizedSpeed;
            crownfill.fillAmount = Mathf.Lerp(crownfill.fillAmount, inverted, Time.deltaTime * 5f);
            if (speed < speedThreshold)
            {
                if (!flames.isPlaying)
                flames.Play();
            }
            else
            {
                if (flames.isPlaying)
                flames.Stop();
                StartCoroutine(LowerRespect());
            }

        }
    }
    IEnumerator LowerRespect()
    {
        yield return new WaitForSeconds(5f);
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
