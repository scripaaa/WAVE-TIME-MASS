using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    private static int freezeCount = 0;

    public static void FreezeTime()
    {
        freezeCount++;
        Time.timeScale = 0f;
    }

    public static void UnfreezeTime()
    {
        if (freezeCount > 0)
        {
            freezeCount--;
            if (freezeCount == 0)
            {
                Time.timeScale = 1f;
            }
        }
    }
}

