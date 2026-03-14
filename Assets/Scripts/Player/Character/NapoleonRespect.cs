using System.Numerics;
using System.Runtime.Serialization;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NapoleonRespect : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Image crownfill;
    [SerializeField] private float topSpeed = 100f;
    [SerializeField] private bool respectEnabled;
        
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        
    }

    void Update()
    {
        //Vector3 horizontalVelocity = rb.linearVelocity;
        //horizontalVelocity.y = 0;
        //float speed = horizontalVelocity.magnitude;
        float speed = rb.linearVelocity.magnitude;
        float normalizedSpeed = speed/topSpeed;
        crownfill.fillAmount = Mathf.Lerp(crownfill.fillAmount, normalizedSpeed, Time.deltaTime * 5f);
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
