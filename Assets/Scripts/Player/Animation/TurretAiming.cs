using UnityEngine;

public class TurretAiming : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] float aimingTurnRate;
    [SerializeField] float idlingTurnRate;

    public void Update()
    {
        if (playerCamera.CurrentTarget != null)
        {
            // Aim
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                MathHelpers.PointAtGlobalPointOnLocalAxisRotation(
                    transform.parent,
                    playerCamera.CurrentTarget.position,
                    Vector3.up),
                aimingTurnRate * Time.deltaTime);
        }
        else
        {
            // Don't aim
            transform.localRotation = Quaternion.Euler(new Vector3(
                0,
                Mathf.LerpAngle(transform.localEulerAngles.y, 0f, idlingTurnRate * Time.deltaTime),
                0));
        }
    }
}
