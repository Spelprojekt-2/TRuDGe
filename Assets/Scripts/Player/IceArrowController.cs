using System;
using UnityEngine;

public class IceArrowController : MonoBehaviour
{
    public GameObject iceArrow;
    private MeshRenderer iceArrowRenderer;
    public GameObject target1;
    public GameObject target2;
    public GameObject currentTarget;
    
    
    
    public float speed = 10f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (iceArrow == null) return;
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        
        float singleStep = speed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        newDirection.y = 0;
        Debug.DrawRay(transform.position, newDirection, Color.red);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
    }


    void OnSceneLoaded()
    {
        iceArrowRenderer = iceArrow.GetComponent<MeshRenderer>();
        iceArrowRenderer.enabled = false;
        currentTarget = target1;
    }
    
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceEntry"))
        {
            iceArrowRenderer = iceArrow.GetComponent<MeshRenderer>();
            iceArrowRenderer.enabled = true;
            
        }
        
        
        if (other.CompareTag("LakeExit"))
        {
            iceArrowRenderer = iceArrow.GetComponent<MeshRenderer>();
            float distanceToPoint = Vector3.Distance(transform.position, currentTarget.transform.position);
        
            //Debug.Log(distanceToPoint);
            if (distanceToPoint < 120)
            {
                
                iceArrowRenderer.enabled = false;
                if (currentTarget == target1)
                {
                    currentTarget = target2;
                }
                else if (currentTarget == target2)
                {
                    currentTarget = target1;
                }
            }
        }
        
    }
}
