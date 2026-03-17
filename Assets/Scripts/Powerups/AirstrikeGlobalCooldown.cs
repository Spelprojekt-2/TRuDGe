using UnityEngine;

public class AirstrikeGlobalCooldown : MonoBehaviour
{
    [HideInInspector] public static bool canUseAirstrike = true;
    [SerializeField] private float airStrikeCooldownduration = 10;
    private float timer = 0;

    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        if(timer >= airStrikeCooldownduration && !canUseAirstrike)
        {
            //Debug.Log("Can use airstrike again");
            canUseAirstrike = true;
            timer = 0;
        }
        else if(timer < airStrikeCooldownduration && !canUseAirstrike)
        {
            canUseAirstrike = false;
            timer += Time.deltaTime;
        }
    }
}
