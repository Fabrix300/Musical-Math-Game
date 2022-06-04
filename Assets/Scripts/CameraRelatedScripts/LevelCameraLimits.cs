using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraLimits
{
    public string levelName;
    public float minPositionX; // left border
    public float maxPositionX; //  right border
    public float minPositionY;
    public float maxPositionY;

    public LevelCameraLimits(string levelName, float minPosX, float maxPosX, float minPosY, float maxPosY)
    {
        this.levelName = levelName;
        minPositionX = minPosX;
        maxPositionX = maxPosX;
        minPositionY = minPosY;
        maxPositionY = maxPosY;
    }

}
