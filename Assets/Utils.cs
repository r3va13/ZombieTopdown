using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class Utils
{
    public static Vector2 GetVector2FromString(string line)
    {
        
        string[] split = line.Split('_');
        return new Vector2(
            Convert.ToSingle(split[0]), 
                Convert.ToSingle(split[1])); 
    }
    
    public static float NormalizeAngle(float angle)
    {
        if (angle >= 0) return angle;
        else return 180 + (180 + angle);
    }
    
    public static void CreateBulletTracer(Vector3 fromPosition, Vector3 toPosition)
    {
        Vector3 direction = (toPosition - fromPosition).normalized;
        float eulerZ = UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
        float distance = Vector3.Distance(fromPosition, toPosition);
        Vector3 tracerSpawnPosition = fromPosition + direction * (distance * 0.5f);
        Material tmpTracerMaterial = new Material(CharactersController.Instance.BulletTracerMaterial);
        float bulletDistance = distance < 50f ? distance : 50f;
        tmpTracerMaterial.SetTextureScale("_MainTex", new Vector2(4f, bulletDistance / 50f));
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 0.25f, distance, tmpTracerMaterial, null, 10000);

        int frame = 0;
        float framerate = 0.035f;
        float timer = framerate;
        
        FunctionUpdater.Create(() =>
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                frame++;
                timer += framerate;
                if (frame >= 4)
                {
                    worldMesh.DestroySelf();
                    return true;
                }
                else tmpTracerMaterial.SetColor("_Color", new Color(0.78f, 0.78f, 0.35f, 1f - 0.25f * frame));
            }

            return false;
        });
    }
}
