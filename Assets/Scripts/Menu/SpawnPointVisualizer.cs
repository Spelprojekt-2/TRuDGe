using UnityEngine;

public class SpawnPointVisualizer : MonoBehaviour
{
    public bool timeTrialSpawnpos;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GetRotation() * Vector3.forward * 3);
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
