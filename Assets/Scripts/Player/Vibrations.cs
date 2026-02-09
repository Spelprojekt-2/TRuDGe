using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Vibrations : MonoBehaviour
{
    public void TriggerVibration(float lowFreq, float highFreq, float duration)
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            // lowFreq: Heavy/Thumpy (left motor), highFreq: Sharp/Buzzing (right motor)
            //gamepad.SetMotorSpeeds(lowFreq, highFreq);

            //StartCoroutine(StopVibration(duration, gamepad));
        }
    }

    private IEnumerator StopVibration(float duration, Gamepad gamepad)
    {
        yield return new WaitForSeconds(duration);
        gamepad.ResetHaptics();
    }
}
