using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScaleAudio {
 public static float ScaleAudioByRoomDistance(int distance)
    {
        float ourAudioScale = 1.0f;
        float maxDistance = 0;
        if (GameHandler.roomManager.numberOfRooms % 2 == 0)
        {
            maxDistance = GameHandler.roomManager.numberOfRooms / 2;
        }
        else if (GameHandler.roomManager.numberOfRooms % 2 != 0)
        {
            maxDistance = (int)(GameHandler.roomManager.numberOfRooms / 2 + 0.5f);
        }

        if (distance == maxDistance)
        {
            ourAudioScale = 0.5f;
        }
        else if (distance == (int)(maxDistance * 0.75))
        {
            ourAudioScale = 0.625f;
        }
        else if (distance == (maxDistance * 0.5))
        {
            ourAudioScale = 0.75f;
        }
        else if (distance == (maxDistance * 0.25))
        {
            ourAudioScale = 0.875f;
        }
        else if (distance == 1)
        {
            ourAudioScale = 1;
        }
        return ourAudioScale;

    }
	// Use this for initialization
	
}
