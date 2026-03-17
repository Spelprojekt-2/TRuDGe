using System;
using UnityEngine;

public class IceArrow : MonoBehaviour
{

    //[SerializeField] GameObject iceArrow;
    
    [SerializeField] private MeshRenderer iceArrow;
    [SerializeField] public GameObject target1;
    [SerializeField] public GameObject target2;
    [SerializeField] private GameObject currentTarget;

    public float speed = 10f;
    //private bool passed = false;
    
    //[SerializeField] public RacerData racerData;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        
        float singleStep = speed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        
        Debug.DrawRay(transform.position, newDirection, Color.red);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        
        
    }


    public void Activate(MeshRenderer iceArrow)
    {
        iceArrow.enabled = true;
       
    }

    public void Inactive(MeshRenderer iceArrow)
    {

        float distanceToPoint = Vector3.Distance(transform.position, currentTarget.transform.position);
        

        if (distanceToPoint < 50)
        {
            iceArrow.enabled = false;
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

    public void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Funkar");
        
        if (other.tag == "IceCollider")
        {
            
            
            
        } 
        
    }
    
}
