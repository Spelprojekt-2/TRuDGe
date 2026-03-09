using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public static class Leaderboard
{
    private static RacerData[] leaderboard;

    public static void SetLeaderboard(RacerData[] newLeaderboard)
    {
        leaderboard = newLeaderboard;
    }

    public static string GetLeaderboardString()
    {
        string leaderboardtxt = "";

        for (int i = 0; i < leaderboard.Length; i++)
        {
            leaderboard[i].DisablePosition();
            leaderboardtxt += $"{GetPosString(i + 1)} {GetPlayerColor(leaderboard[i].index)}{leaderboard[i].racername}</color>\n";
        }
        GameObject.FindWithTag("DisplayChar").GetComponent<CharAfterRace>().Display2DCharacter(leaderboard[0].racername);
        //Displaychar();
        return leaderboardtxt;             
    }
    private static string GetPosString(int pos)
    {
        switch (pos)
        {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return pos + "th";
        }
    }

    private static string GetPlayerColor(int playerIndex)
    {
        return playerIndex switch
        {
            0 => "<color=#FF1919>",
            1 => "<color=#32C832>",
            2 => "<color=#FFFF00>",
            3 => "<color=#00BEFF>",
            _ => "<color=#FFFFFF>"
        };
    }

    public static string FormatTime(double time)
    {
        int minutes = (int)Math.Floor(time / 60);
        int seconds = (int)Math.Floor(time % 60);
        int milliseconds = (int)Math.Floor((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

}
