using UnityEngine;

[CreateAssetMenu(menuName = "ProbabilityPickup")]
public class ProbabilityPickupSO : ScriptableObject
{
    public PowerUpProbabilityPosition[] probability;

    public PlayerPowerups.PowerUpType RandomizePowerUp(int position)
    {
        return probability[position-1].GetPowerUp();
    }
}


[System.Serializable]
public class PowerUpProbabilityPosition
{
    public PowerUpProbability[] powerups;
    public PlayerPowerups.PowerUpType GetPowerUp()
    {
        int tot = 0;
        foreach (var powerup in powerups)
        {
            tot += powerup.weight;
        }
        int rnd = Random.Range(1, tot);
        Debug.Log(rnd + ", " + tot);
        for (int i = 0; i < powerups.Length; i++)
        {
            if (rnd <= powerups[i].weight) return powerups[i].type;
            else rnd -= powerups[i].weight;
        }
        return powerups[^1].type;
    }
}


[System.Serializable]
public class PowerUpProbability
{
    public PlayerPowerups.PowerUpType type;
    public int weight;
}
