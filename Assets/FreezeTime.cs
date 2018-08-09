using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FreezeTime {

    public static float slowdownFactor = 0.05f;
   // TODO: UNCOMMENT THIS SHIT

    public static void SlowdownTime(float slowdownFactor)
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;

    }


    public static void StartTimeAgain()
    {
        Time.timeScale = 1.0f;
    }



}
