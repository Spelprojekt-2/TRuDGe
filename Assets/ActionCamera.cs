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
    [SerializeField] private Camera cam;
    private int index;
    private GameObject playerTarget = null;
    
    
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
        //cam = GetComponent<Camera>(); 
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
        /*
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeCameraPosition();
            timer = 10;
        }
        */
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.gameObject;
            FollowPlayer();
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (playerTarget == other.gameObject)
        {
            ChangeCameraPosition();
        }
    }

    
    public void FollowPlayer()
    {
        Debug.Log("FollowPlayer");
        
        Vector3 direction = playerTarget.transform.position - cam.transform.position;
        Vector3 rotation = Vector3.RotateTowards(cam.transform.forward, direction, 30f, 0f);
        cam.transform.rotation = Quaternion.LookRotation(rotation);
        
        cam.transform.LookAt(playerTarget.transform.position);
    }

    public void ChangeCameraPosition()
    {
        Debug.Log("ChangeCameraPosition");
        cam.transform.position = cameraPositions[currentCamera].transform.position;
        Vector3 direction = cameraTargets[currentCamera].transform.position - cam.transform.position;
        Vector3 rotation = Vector3.RotateTowards(cam.transform.forward, direction, 90f, 0.0f);
        cam.transform.rotation = Quaternion.LookRotation(direction);
        currentCamera = (currentCamera + 1) % cameraPositions.Length;
        
    }
}
