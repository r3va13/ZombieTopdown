using System;
using System.Collections;
using System.Collections.Generic;
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
}
