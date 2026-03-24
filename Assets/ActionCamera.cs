using System;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{

    GameObject cameraPosition;
    [SerializeField] private GameObject[] cameraPositions;
    private GameObject cameraTarget;
    [SerializeField] private GameObject[] cameraTargets;
    private int currentCamera;
    [SerializeField] public float timer = 5f;
    private Camera cam;
    private int index;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        cameraPositions = GameObject.FindGameObjectsWithTag("ActionCamera");
        cameraTargets = GameObject.FindGameObjectsWithTag("RotationPivot");
        */

        if (cameraTargets.Length == 0 && cameraPositions.Length == 0)
        {
            return;
        }
        cam = GetComponent<Camera>(); 
        cam.rect = new Rect(
            0.5f,
            0,
            0.5f,
            0.5f);
        
        currentCamera = 0;
        ChangeCameraPosition();
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeCameraPosition();
            timer = 10;
        }
    }

    

    void ChangeCameraPosition()
    {
        
        transform.position = cameraPositions[currentCamera].transform.position;
        Vector3 direction = cameraTargets[currentCamera].transform.position - transform.position;
        Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, 90f, 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        currentCamera = (currentCamera + 1) % cameraPositions.Length;
        
    }
}
