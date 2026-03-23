using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class IceArrow : MonoBehaviour
{

    //[SerializeField] GameObject iceArrow;
    
    private GameObject iceArrow;
    //public GameObject iceArrows;
    [SerializeField] public GameObject target1;
    [SerializeField] public GameObject target2;
    [SerializeField] public GameObject currentTarget;

    public float speed = 10f;
    
    
    
    //private bool passed = false;
    
    //[SerializeField] public RacerData racerData;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // iceArrow = GameObject.FindGameObjectWithTag("Arrow");
    }   

    // Update is called once per frame
    void Update()
    {
        /*
        if (iceArrow == null) return;
        Vector3 targetDirection = currentTarget.transform.position - iceArrow.transform.position;
        
        float singleStep = speed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(iceArrow.transform.forward, targetDirection, singleStep, 0.0f);
        
        Debug.DrawRay(transform.position, newDirection, Color.red);
        
        iceArrow.transform.rotation = Quaternion.LookRotation(newDirection);
        */
    }

/*
    public void Activate(GameObject target)
    {
        iceArrow = target;
        MeshRenderer iceArrowMesh = iceArrow.GetComponent<MeshRenderer>();
        iceArrowMesh.enabled = true;
    }

    public void Inactive(GameObject target)
    {
        iceArrow = target;
        MeshRenderer iceArrowMesh = iceArrow.GetComponent<MeshRenderer>();
        
        
        float distanceToPoint = Vector3.Distance(iceArrow.transform.position, currentTarget.transform.position);
        
        Debug.Log(distanceToPoint);
        
        if (distanceToPoint < 120)
        {
            iceArrowMesh.enabled = false;
            /*
            Debug.Log(currentTarget.name);
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
    */
}
