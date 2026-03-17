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
    [SerializeField] private RacerData rd;
    [SerializeField] private bool respectEnabled;      
    private float speedThreshold = 10f;  
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!respectEnabled && !SceneController.instance.IsMenu)
        {
            if (rd.racername == "King Napoleon III")
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
            bool idle = speed < speedThreshold;

            if (idle)
            {
                crownfill.fillAmount -= Time.deltaTime * 0.5f; 
            }
            else
            {
                crownfill.fillAmount += Time.deltaTime * 0.1f;
            }
            crownfill.fillAmount = Mathf.Clamp01(crownfill.fillAmount);

            if (!idle)
            {
                if (!flames.isPlaying)
                flames.Play();
            }
            else
            {
                flames.Stop();
            }
        }
    }
    public IEnumerator LowerRespect()
    {
        if (rd.racername == "King Napoleon III" && respectEnabled)
        crownfill.fillAmount -= 0.5f; 
        //return;
        yield return new WaitForSeconds(1f);
    }
    public void toggleRespect(bool state)
    {
        respectEnabled = state;
        Respect.SetActive(state);
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
    /*float speed = rb.linearVelocity.magnitude;
            float normalizedSpeed = Mathf.Clamp01(speed/topSpeed);
            float inverted = 1f - normalizedSpeed;
            crownfill.fillAmount = Mathf.Lerp(crownfill.fillAmount, 1f, Time.deltaTime * 5f);*/
}
