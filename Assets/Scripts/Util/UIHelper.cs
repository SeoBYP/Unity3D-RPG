using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper
{
    public static Vector3 Test(Vector3 playerpos,float worldWidth, float worldDepth, float minX, float minZ)
    {
        Vector3 result = Vector3.zero;
        result.x = (playerpos.x - minX) / worldWidth;
        result.z = (playerpos.z - minZ) / worldDepth;
        return result;
    }
    public static Vector2 WorldPosToMapPos(Vector3 worldPos, float worldWidth, float worldDepth, float mapWidth, float mapHeight)
    {
        Vector3 result = Vector3.zero;
        result.x = (worldPos.x * mapWidth) / worldWidth;
        result.y = (worldPos.z * mapHeight) / worldDepth;
        return result;
    }

    public static Vector3 MapPosToWorldPos(Vector3 uiPos, float worldWidth, float worldDepth, float mapWidth, float mapHeight)
    {
        Vector3 result = Vector3.zero;
        result.x = (uiPos.x * worldWidth) / mapWidth;
        result.y = (uiPos.y * worldDepth) / mapHeight;
        return result;
    }

    public static void MarkOnAMap(Transform world, Transform UITarget,float worldWidth, float worldDepth, float mapWidth, float mapHeight)
    {
        UITarget.localPosition = WorldPosToMapPos(world.position, worldWidth, worldDepth, mapWidth, mapHeight);
        MarkOnAMap(world, UITarget);
    }

    public static void MarkOnAMap(Transform world, Transform UITarget)
    {
        float angleZ = Mathf.Atan2(world.forward.z, world.forward.x) * Mathf.Rad2Deg;
        UITarget.eulerAngles = new Vector3(0, 0, angleZ - 90);
    }

    public static void MarkOnTheRPGGame(Vector3 world,Transform uiBackground, float worldWidth, float worldDepth, float mapWidth, float mapHeight)
    {
        uiBackground.localPosition = WorldPosToMapPos(world, worldWidth, worldDepth, mapWidth, mapHeight) * -1;
    }
}
