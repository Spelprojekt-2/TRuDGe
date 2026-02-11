using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Vibrations : MonoBehaviour
{
    private PlayerInput playerInput;
    private Gamepad pairedGamepad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    public void TriggerVibration(float lowFreq, float highFreq, float duration)
    {
        Gamepad gamepad = GetPlayerGamepad();

        if (gamepad != null)
        {
            // lowFreq: Heavy/Thumpy (left motor), highFreq: Sharp/Buzzing (right motor)
            gamepad.SetMotorSpeeds(lowFreq, highFreq);

            StartCoroutine(StopVibration(duration, gamepad));
        }
    }

    private IEnumerator StopVibration(float duration, Gamepad gamepad)
    {
        yield return new WaitForSeconds(duration);
        gamepad.ResetHaptics();
    }

    private Gamepad GetPlayerGamepad()
    {
        foreach (var device in playerInput.user.pairedDevices)
        {
            if (device is Gamepad gamepad)
            {
                return gamepad;
            }
        }
        return null;
    }

    private void OnDisable()
    {
        if (pairedGamepad != null) GetPlayerGamepad()?.ResetHaptics();
    }
}
