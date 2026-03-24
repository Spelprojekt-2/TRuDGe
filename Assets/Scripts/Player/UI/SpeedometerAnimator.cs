using UnityEngine;
using UnityEngine.UI;

public class SpeedometerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Image speedImage;
    [SerializeField] private Image topSpeedImage;
    [SerializeField] private float meterTopSpeed = 200f;
    
    void Update()
    {
        float speed = playerMovement.GetCurrentSpeed(false);
        float topSpeed = playerMovement.GetTopSpeed(true);

        topSpeedImage.fillAmount = topSpeed/meterTopSpeed;
        speedImage.fillAmount = Mathf.Lerp(speedImage.fillAmount, speed/meterTopSpeed, Time.deltaTime * 5f);
    }
}
