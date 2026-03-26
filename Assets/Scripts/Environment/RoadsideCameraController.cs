using UnityEngine;
using System.Collections.Generic;

public class RoadsideCameraController : MonoBehaviour
{

    [SerializeField] private List<RoadsideCamera> cameraPositions;
    private Transform currentPos = null;
    private Transform currentTarget = null;
    private Camera cam;
    [SerializeField] private float maxFov = 100f;
    [SerializeField] private float minDist = 10f;
    [SerializeField] private float minFov = 40f;
    [SerializeField] private float maxDist = 80f;
    [Header("Debug")]
    [SerializeField] private float currentDist = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>(); 
        cam.rect = new Rect(
            0.5f,
            0,
            0.5f,
            0.5f);
    }
    public void Awake()
    {
        if (PlayerTrackerManager.instance.GetPlayerCount() != 3) Destroy(gameObject);
        foreach(RoadsideCamera camPos in cameraPositions)
        {
            camPos.ObservedObjectChanged += ChangeCamState;
        }
    }
    public void OnDestroy()
    {
        foreach(RoadsideCamera camPos in cameraPositions)
        {
            camPos.ObservedObjectChanged -= ChangeCamState;
        }
    }

    public void ChangeCamState(Transform camPos, Transform player)
    {
        if (currentPos == camPos)
        {
            if (player == null)
            {
                currentPos = null;
                currentTarget = null;
            }
        }
        else
        {
            if (player != null)
            {
                if (currentPos == null)
                {
                    currentPos = camPos;
                    transform.position = currentPos.position;
                    currentTarget = player;
                }
            }
        }
    }

    public void Update()
    {
        if (currentTarget != null)
        {
            transform.LookAt(currentTarget);

            currentDist = Vector2.Distance(transform.position, currentTarget.position);

            cam.fieldOfView = Mathf.Lerp(
                minFov, maxFov,
                Mathf.InverseLerp(maxDist, minDist, currentDist));

        }
    }
}
